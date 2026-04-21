using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BombController : MonoBehaviour
{
    [Header("Bomb")]
    public GameObject bombPrefab;
    public KeyCode inputKey = KeyCode.Space;
    public float bombFuseTime = 3f;
    public int bombAmount = 1;
    private int bombsRemaining;

    [Header("Explosion (Se pasará a la bomba)")]
    public Explosion explosionPrefab;
    public LayerMask explosionLayerMask;
    public float explosionDuration = 1f;
    public int explosionRadius = 1;

    [Header("Destructible (Se pasará a la bomba)")]
    public Tilemap destructibleTiles;
    public Destructible destructiblePrefab;

    private MovementController movementController;

    private void Awake()
    {
        movementController = GetComponent<MovementController>();
    }

    private void OnEnable()
    {
        bombsRemaining = bombAmount;
        
        if (WebSocketManager.Instance != null)
        {
            WebSocketManager.Instance.OnMissatgeRebut += ProcessarMissatgeXarxa;
        }
    }

    private void OnDisable()
    {
        if (WebSocketManager.Instance != null)
        {
            WebSocketManager.Instance.OnMissatgeRebut -= ProcessarMissatgeXarxa;
        }
    }

    private void ProcessarMissatgeXarxa(string tipus, string json)
    {
        if (movementController != null && movementController.esMeu) return;

        if (tipus == "posar_bomba")
        {
            PositionalMessage msg = JsonUtility.FromJson<PositionalMessage>(json);
            Vector2 pos = new Vector2(msg.x, msg.y);

            GameObject bombObj = Instantiate(bombPrefab, pos, Quaternion.identity);
            Bomb bombScript = bombObj.GetComponent<Bomb>();
            if (bombScript == null) bombScript = bombObj.AddComponent<Bomb>();
             
            bombScript.fuseTime = bombFuseTime;
            bombScript.explosionPrefab = explosionPrefab;
            bombScript.explosionLayerMask = explosionLayerMask;
            bombScript.explosionDuration = explosionDuration;
            bombScript.explosionRadius = explosionRadius;
            bombScript.destructibleTiles = destructibleTiles;
            bombScript.destructiblePrefab = destructiblePrefab;
        }
    }

    public void TryPlaceBomb()
    {
        if (bombsRemaining > 0)
        {
            StartCoroutine(PlaceBomb());
        }
    }

    private void Update()
    {
        if (movementController != null && !movementController.esMeu) return;

        if (Input.GetKeyDown(inputKey))
        {
            TryPlaceBomb();
        }
    }

    public IEnumerator PlaceBomb()
    {
        Vector2 position = transform.position;
        position.x = Mathf.Floor(position.x) + 0.5f;
        position.y = Mathf.Floor(position.y) + 0.5f;

        GameObject bombObj = Instantiate(bombPrefab, position, Quaternion.identity);
        Bomb bombScript = bombObj.GetComponent<Bomb>();
        if (bombScript == null) bombScript = bombObj.AddComponent<Bomb>();
         
        bombScript.fuseTime = bombFuseTime;
        bombScript.explosionPrefab = explosionPrefab;
        bombScript.explosionLayerMask = explosionLayerMask;
        bombScript.explosionDuration = explosionDuration;
        bombScript.explosionRadius = explosionRadius;
        bombScript.destructibleTiles = destructibleTiles;
        bombScript.destructiblePrefab = destructiblePrefab;

        bombsRemaining--;

        if (WebSocketManager.Instance != null) {
            string msgJson = "{\"x\":" + position.x.ToString(System.Globalization.CultureInfo.InvariantCulture) +
                             ",\"y\":" + position.y.ToString(System.Globalization.CultureInfo.InvariantCulture) + "}";
            WebSocketManager.Instance.SendMessage("posar_bomba", msgJson);
        }

        yield return new WaitForSeconds(bombFuseTime);
        bombsRemaining++;
    }

    public void AddBomb()
    {
        bombAmount++;
        bombsRemaining++;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            other.isTrigger = false;
        }
    }

    [System.Serializable]
    public class PositionalMessage
    {
        public float x;
        public float y;
    }
}