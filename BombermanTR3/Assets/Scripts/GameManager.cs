using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject[] players;

    public string usuariId;
    public string nomUsuari;
    public string codiSala;
    public bool esPrimary;

    [Header("Items Sincronizados")]
    public GameObject[] spawnableItems; // Arrastra los prefabs de los items aquí en el Inspector

    private void Awake()
    {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        if (WebSocketManager.Instance != null)
        {
            WebSocketManager.Instance.OnMissatgeRebut += HandleWebSocketMessage;
        }
    }

    private void HandleWebSocketMessage(string tipus, string json)
    {
        // Si recibimos el aviso del Host, creamos el item
        if (tipus == "spawn_item")
        {
            Debug.Log("[RADAR ITEM] Mensaje recibido del Host: " + json);
            Destructible.ItemSpawnMsg msg = JsonUtility.FromJson<Destructible.ItemSpawnMsg>(json);
            SpawnRemoteItem(msg.x, msg.y, msg.itemIndex);
        }
    }

    private void SpawnRemoteItem(float x, float y, int itemIndex)
    {
  
        if (spawnableItems != null && spawnableItems.Length > 0)
        {
            if (itemIndex >= 0 && itemIndex < spawnableItems.Length)
            {
                Instantiate(spawnableItems[itemIndex], new Vector2(x, y), Quaternion.identity);
                Debug.Log("[RADAR ITEM] ¡Item creado con éxito en la pantalla del rival!");
            }
            else
            {
                Debug.LogError($"[ERROR ITEM] El Host dice que cree el item {itemIndex}, pero tu GameManager solo tiene {spawnableItems.Length} items en la lista.");
            }
        }
        else
        {
            Debug.LogError("[ERROR ITEM] La lista 'Spawnable Items' de tu GameManager está vacía en el Inspector. ¡Arrastra los prefabs de los items!");
        }
    }

    public void CheckWinState()
    {
        int aliveCount = 0;
        foreach (GameObject player in players)
        {
            if(player.activeSelf) aliveCount++;
        }
        if(aliveCount <= 1) Invoke(nameof(NewRound), 3f);
    }

    private void NewRound()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDestroy()
    {
        if (WebSocketManager.Instance != null)
        {
            WebSocketManager.Instance.OnMissatgeRebut -= HandleWebSocketMessage;
        }
    }
}