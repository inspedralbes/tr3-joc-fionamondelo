using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private MovementController mc;

    private void Awake()
    {
        mc = GetComponent<MovementController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (mc != null && mc.esMeu)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Explosion"))
            {
                Morir();
            }
        }
    }

    private void Morir()
    {
        gameObject.SetActive(false);

        if (WebSocketManager.Instance != null)
        {
            MortMsg msg = new MortMsg { jugadorId = GameManager.Instance.usuariId };
            WebSocketManager.Instance.SendMessage("jugador_mort", JsonUtility.ToJson(msg));
        }

        GameManager.Instance.CheckWinState();
    }

    [System.Serializable]
    public class MortMsg
    {
        public string jugadorId;
    }
}