using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [HideInInspector] public GameObject[] players;

    public string usuariId;
    public string nomUsuari;
    public string codiSala;
    public bool esPrimary;
    public bool isSinglePlayer;
    public string guanyadorLocalNom;

    public GameObject bombPrefab;
    public GameObject[] spawnableItems;

    public string nomEscenaResultats = "ResultsScene"; 

    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        } 
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (WebSocketManager.Instance != null)
        {
            WebSocketManager.Instance.OnMissatgeRebut += HandleWebSocketMessage;
        }
        
        SceneManager.sceneLoaded += OnSceneLoaded;
        EncontrarJugadoresEnEscena();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        if (WebSocketManager.Instance != null)
        {
            WebSocketManager.Instance.OnMissatgeRebut -= HandleWebSocketMessage;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        EncontrarJugadoresEnEscena();
    }

    public void EncontrarJugadoresEnEscena()
    {
        GameObject p1 = GameObject.Find("Player 1") ?? GameObject.Find("Player1");
        GameObject p2 = GameObject.Find("Player 2") ?? GameObject.Find("Player2");

        if (p1 != null && p2 != null)
        {
            players = new GameObject[] { p1, p2 };
        }
    }

    private void HandleWebSocketMessage(string tipus, string json)
    {
        if (tipus == "spawn_item")
        {
            Destructible.ItemSpawnMsg msg = JsonUtility.FromJson<Destructible.ItemSpawnMsg>(json);
            SpawnRemoteItem(msg.x, msg.y, msg.itemIndex);
        }
        else if (tipus == "posar_bomba")
        {
            BombController.PositionalMessage msg = JsonUtility.FromJson<BombController.PositionalMessage>(json);
            CreateRemoteBomb(msg.x, msg.y);
        }
        else if (tipus == "jugador_mort")
        {
            foreach (GameObject player in players)
            {
                if (player != null)
                {
                    MovementController mc = player.GetComponent<MovementController>();
                    if (mc != null && !mc.esMeu) 
                    {
                        player.SetActive(false);
                    }
                }
            }
            CheckWinState();
        }
    }

    private void SpawnRemoteItem(float x, float y, int itemIndex) {
        if (spawnableItems != null && itemIndex >= 0 && itemIndex < spawnableItems.Length) {
            Instantiate(spawnableItems[itemIndex], new Vector2(x, y), Quaternion.identity);
        }
    }

    private void CreateRemoteBomb(float x, float y) {
        if (bombPrefab != null) {
            GameObject bombObj = Instantiate(bombPrefab, new Vector3(x, y, 0), Quaternion.identity);
            BombController localBC = null;
            foreach (BombController b in FindObjectsByType<BombController>(FindObjectsSortMode.None)) {
                if (b.GetComponent<MovementController>()?.esMeu == true) {
                    localBC = b;
                    break;
                }
            }
            if (localBC != null) {
                Bomb b = bombObj.GetComponent<Bomb>() ?? bombObj.AddComponent<Bomb>();
                b.fuseTime = localBC.bombFuseTime;
                b.explosionPrefab = localBC.explosionPrefab;
                b.explosionLayerMask = localBC.explosionLayerMask;
                b.explosionDuration = localBC.explosionDuration;
                b.explosionRadius = localBC.explosionRadius;
                b.destructibleTiles = localBC.destructibleTiles;
                b.destructiblePrefab = localBC.destructiblePrefab;
            }
        }
    }

    public void CheckWinState()
    {
        if (players == null || players.Length == 0) EncontrarJugadoresEnEscena();

        int aliveCount = 0;
        GameObject winner = null;

        foreach (GameObject player in players)
        {
            if (player != null && player.activeSelf)
            {
                aliveCount++;
                winner = player;
            }
        }

        if (aliveCount <= 1)
        {
            if (winner != null)
            {
                MovementController mc = winner.GetComponent<MovementController>();
                
                if (isSinglePlayer)
                {
                    guanyadorLocalNom = (mc != null && mc.controlledByAI) ? "IA" : nomUsuari;
                }
                else if (mc != null && mc.esMeu)
                {
                    StartCoroutine(ApiManager.Instance.FinalitzarPartida(codiSala, usuariId, 
                        (exit) => {},
                        (err) => {}
                    ));
                }
            }
            else
            {
                guanyadorLocalNom = "Empat";
            }

            Invoke(nameof(LoadResultsScene), 2f);
        }
    }

    private void LoadResultsScene()
    {
        players = null;
        SceneManager.LoadScene(nomEscenaResultats);
    }
}