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
        // SOLO MI jugador detecta si se quema. El títere remoto no hace comprobaciones 
        // para que no haya desincronización (esperará a que el servidor le diga que muera).
        if (mc != null && mc.esMeu)
        {
            // Asegúrate de que el prefab de tu explosión tenga asignada la capa (Layer) o Tag correcto.
            // Aquí comprobamos si choca con un objeto que tenga la capa "Explosion".
            if (other.gameObject.layer == LayerMask.NameToLayer("Explosion"))
            {
                Morir();
            }
        }
    }

    private void Morir()
    {
        Debug.Log("¡Me he quemado!");
        gameObject.SetActive(false); // Escondo mi personaje

        // Aviso al otro jugador por red de que he muerto
        if (WebSocketManager.Instance != null)
        {
            MortMsg msg = new MortMsg { jugadorId = GameManager.Instance.usuariId };
            WebSocketManager.Instance.SendMessage("jugador_mort", JsonUtility.ToJson(msg));
        }

        // Aviso a mi propio GameManager para que evalúe si se acabó la partida
        GameManager.Instance.CheckWinState();
    }

    [System.Serializable]
    public class MortMsg
    {
        public string jugadorId;
    }
}