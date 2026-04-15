using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class ResultsController : MonoBehaviour
{
    public string nomEscenaMenu = "LoginScene"; 

    private UIDocument uiDocument;
    private Label textResultat;
    private Button botoTornar;

    private void OnEnable()
    {
        uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null) return;

        var root = uiDocument.rootVisualElement;

        // Buscamos los elementos del UXML por su atributo "name"
        textResultat = root.Q<Label>("TextResultat");
        botoTornar = root.Q<Button>("BotoTornar");

        // Asignamos la acción al botón
        if (botoTornar != null)
        {
            botoTornar.clicked += VolverAlMenu;
        }

        MostrarResultado();
    }

    private void MostrarResultado()
    {
        if (GameManager.Instance == null) return;

        string codiSala = GameManager.Instance.codiSala;

        if (textResultat != null)
        {
            textResultat.text = "Carregant resultats...";
        }

        StartCoroutine(ApiManager.Instance.GetPartida(codiSala, 
            (json) => {
                ApiManager.PartidaData partida = JsonUtility.FromJson<ApiManager.PartidaData>(json);
                if (partida != null && partida.guanyador != null)
                {
                    textResultat.text = partida.guanyador.nomUsuari;
                }
                else
                {
                    textResultat.text = "Partida finalitzada (Empat o sense dades).";
                }
            },
            (error) => {
                Debug.LogError("Error recuperant resultats: " + error);
                textResultat.text = "Error al carregar resultats.";
            }
        ));

        // Desconectar el WebSocket
        if (WebSocketManager.Instance != null)
        {
            WebSocketManager.Instance.Disconnect();
        }
    }

    private void VolverAlMenu()
    {
        Debug.Log("Tornant al menú principal...");

        // Destruir el GameManager actual para tener uno limpio al volver
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }

        SceneManager.LoadScene(nomEscenaMenu);
    }
}