using UnityEngine;

public class GameSceneController : MonoBehaviour
{
    private GameObject player1;
    private GameObject player2;

    void Awake()
    {
        Debug.Log("GameSceneController: Iniciant escena...");

        if (WebSocketManager.Instance != null)
        {
            WebSocketManager.Instance.OnMissatgeRebut += ProcessarMissatge;
        }

        player1 = GameObject.Find("Player 1");
        if (player1 == null) player1 = GameObject.Find("Player1");

        player2 = GameObject.Find("Player 2");
        if (player2 == null) player2 = GameObject.Find("Player2");

        if (GameManager.Instance != null && player1 != null && player2 != null)
        {
            MovementController m1 = player1.GetComponent<MovementController>();
            MovementController m2 = player2.GetComponent<MovementController>();

            if (GameManager.Instance.esPrimary)
            {
                m1.esMeu = true;
                m2.esMeu = false;
            }
            else
            {
                m1.esMeu = false;
                m2.esMeu = true;
            }
        }
    }

    private void ProcessarMissatge(string tipus, string json)
    {
        // Ara GameManager s'encarrega de les bombes ("bomb-placed").
        // Aquí només deixem la lògica de mort o altres missatges si els tens.
        
        if (tipus == "jugador_mort")
        {
            MortMessage data = JsonUtility.FromJson<MortMessage>(json);
            Debug.Log(">>> JUGADOR MORT: " + data.jugadorId);
            // Lògica de mort
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