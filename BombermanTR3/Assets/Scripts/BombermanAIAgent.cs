using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.Collections;

public class BombermanAIAgent : Agent
{
    private Rigidbody2D rb;
    private BombController bombController;
    private MovementController movementController;
    public Vector3 spawnPosition = new Vector3(0.5f, 0.5f, 0);

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
        bombController = GetComponent<BombController>();
        movementController = GetComponent<MovementController>();
    }

    public override void OnEpisodeBegin()
    {
        if (rb != null) rb.linearVelocity = Vector2.zero;
        transform.position = spawnPosition;
        
        // No destruïm objectes si estem en joc real, ML-Agents s'encarrega d'això en training
        // Però per si de cas, comprovem si estem en mode training (podríem afegir un flag si cal)
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition.x);
        sensor.AddObservation(transform.localPosition.y);

        // Buscar el jugador humà principal
        GameObject player = null;
        if (GameManager.Instance != null && GameManager.Instance.players != null) {
            foreach(var p in GameManager.Instance.players) {
                if (p != null && p != gameObject) {
                    player = p;
                    break;
                }
            }
        }
        
        if (player == null) player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            Vector2 toPlayer = player.transform.position - transform.position;
            sensor.AddObservation(toPlayer.normalized);
        }
        else
        {
            sensor.AddObservation(Vector2.zero);
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        if (actions.DiscreteActions.Length == 0)
        {
            return;
        }

        int movementAction = actions.DiscreteActions[0];
        Vector2 moveDir = Vector2.zero;

        switch (movementAction)
        {
            case 1: moveDir = Vector2.up; break;
            case 2: moveDir = Vector2.down; break;
            case 3: moveDir = Vector2.left; break;
            case 4: moveDir = Vector2.right; break;
            case 5:
                if (bombController != null)
                {
                    bombController.TryPlaceBomb();
                }
                break;
        }

        if (movementController != null) {
            movementController.SetAIDirection(moveDir);
        }

        AddReward(-0.001f);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = 0;

        if (Input.GetKey(KeyCode.UpArrow)) discreteActions[0] = 1;
        else if (Input.GetKey(KeyCode.DownArrow)) discreteActions[0] = 2;
        else if (Input.GetKey(KeyCode.LeftArrow)) discreteActions[0] = 3;
        else if (Input.GetKey(KeyCode.RightArrow)) discreteActions[0] = 4;
        else if (Input.GetKey(KeyCode.Space)) discreteActions[0] = 5;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            AddReward(-1.0f);
            EndEpisode();
        }
        
        if (other.CompareTag("Player"))
        {
            AddReward(1.0f);
            EndEpisode();
        }
    }
}
