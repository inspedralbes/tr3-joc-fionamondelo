using UnityEngine;

public class GameSceneController : MonoBehaviour
{
    private GameObject player1;
    private GameObject player2;
    public GameObject bombPrefab;

    void Awake()
    {
        Debug.Log("GameSceneController: Iniciant escena...");

        player1 = GameObject.Find("Player 1");
        if (player1 == null) player1 = GameObject.Find("Player1");

        player2 = GameObject.Find("Player 2");
        if (player2 == null) player2 = GameObject.Find("Player2");

        if (player1 == null || player2 == null)
        {
            Debug.LogError("GameSceneController: No s'han trobat els objectes dels jugadors. Revisa els noms!");
            return;
        }

        if (GameManager.Instance != null)
        {
            Debug.Log("GameSceneController: GameManager trobat. esPrimary = " + GameManager.Instance.esPrimary);

            MovementController m1 = player1.GetComponent<MovementController>();
            MovementController m2 = player2.GetComponent<MovementController>();

            if (GameManager.Instance.esPrimary)
            {
                m1.esMeu = true;
                m2.esMeu = false;
                Debug.Log("GameSceneController: Ets el Jugador 1 (Local)");
            }
            else
            {
                m1.esMeu = false;
                m2.esMeu = true;
                Debug.Log("GameSceneController: Ets el Jugador 2 (Local)");
            }
        }
        else
        {
            Debug.LogWarning("GameSceneController: No s'ha trobat GameManager.Instance!");
        }

        if (WebSocketManager.Instance != null)
        {
            WebSocketManager.Instance.OnMissatgeRebut += ProcessarMissatge;
        }
    }

    private void ProcessarMissatge(string tipus, string json)
    {
        if (tipus == "jugador_mort")
        {
            MortMessage msg = JsonUtility.FromJson<MortMessage>(json);

            if (msg.jugadorId == "1")
            {
                player1.SetActive(false);
            }
            else if (msg.jugadorId == "2")
            {
                player2.SetActive(false);
            }
        }
        else if (tipus == "posar_bomba")
        {
            PosicioData data = JsonUtility.FromJson<PosicioData>(json);
            // Redondeamos para que la bomba aparezca en el centro de la celda
            Vector2 bombPos = new Vector2(Mathf.Floor(data.x) + 0.5f, Mathf.Floor(data.y) + 0.5f);
            Instantiate(bombPrefab, bombPos, Quaternion.identity);
        }
    }

    private void OnDestroy()
    {
        if (WebSocketManager.Instance != null)
        {
            WebSocketManager.Instance.OnMissatgeRebut -= ProcessarMissatge;
        }
    }

    [System.Serializable]
    public class MortMessage
    {
        public string tipus;
        public string jugadorId;
    }
}
