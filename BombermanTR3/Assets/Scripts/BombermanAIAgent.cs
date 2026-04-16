using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.Collections;

public class BombermanAIAgent : Agent
{
    private Rigidbody2D rb;
    private BombController bombController;
    public float moveSpeed = 5f;
    public Vector3 spawnPosition = new Vector3(0.5f, 0.5f, 0);

    // Inicialización del agente
    public override void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
        bombController = GetComponent<BombController>();
        
        if (rb == null) Debug.LogError("¡ERROR! No se encontró Rigidbody2D en " + gameObject.name);
        else Debug.Log("IA Inicializada correctamente en " + gameObject.name);
    }

    public override void OnEpisodeBegin()
    {
        Debug.Log("Episodio iniciado: Reseteando agente y limpiando tablero.");
        rb.linearVelocity = Vector2.zero;
        transform.position = spawnPosition;
        
        // Usamos DestroyImmediate para asegurar que desaparecen ANTES de que el agente reaparezca
        Bomb[] bombs = FindObjectsByType<Bomb>(FindObjectsSortMode.None);
        foreach (var b in bombs) DestroyImmediate(b.gameObject);

        Explosion[] explosions = FindObjectsByType<Explosion>(FindObjectsSortMode.None);
        foreach (var e in explosions) DestroyImmediate(e.gameObject);
    }

    // Observaciones para la red neuronal
    public override void CollectObservations(VectorSensor sensor)
    {
        // Posición actual (X, Y)
        sensor.AddObservation(transform.localPosition.x);
        sensor.AddObservation(transform.localPosition.y);

        // Vector de proximidad básico (podríamos añadir más aquí)
        GameObject player = GameObject.FindWithTag("Player");
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

    // Recepción de acciones de la IA
    public override void OnActionReceived(ActionBuffers actions)
    {
        if (actions.DiscreteActions.Length == 0)
        {
            Debug.LogWarning("¡Atención! No se están recibiendo acciones DISCRETAS. Revisa Behavior Parameters -> Actions -> Space Type.");
            return;
        }

        int movementAction = actions.DiscreteActions[0];
        if (movementAction != 0) Debug.Log("IA Recibió acción: " + movementAction);
        
        Vector2 moveDir = Vector2.zero;

        // 0=Nada, 1=Arriba, 2=Abajo, 3=Izquierda, 4=Derecha, 5=Bomba
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

        // Aplicamos movimiento al Rigidbody usando MovePosition
        Vector2 translation = moveDir * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + translation);

        // Penalización por cada paso para incentivar rapidez/actividad
        AddReward(-0.001f);
    }

    // Control manual para tests
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

    // Detección de colisiones y muerte
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si choca con una explosión
        if (other.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            Debug.Log("IA Murió por: " + other.gameObject.name + " en capa: " + other.gameObject.layer);
            AddReward(-1.0f);
            EndEpisode();
        }
        
        // Si mata al jugador (asumiendo que el jugador tiene tag "Player")
        if (other.CompareTag("Player"))
        {
            Debug.Log("IA Mató al Jugador: " + other.gameObject.name);
            AddReward(1.0f);
            EndEpisode();
        }
    }
}
