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

        textResultat = root.Q<Label>("TextResultat");
        botoTornar = root.Q<Button>("BotoTornar");

        if (botoTornar != null)
        {
            botoTornar.clicked += VolverAlMenu;
        }

        MostrarResultado();
    }

    private void MostrarResultado()
    {
        if (GameManager.Instance == null) return;

        if (textResultat != null)
        {
            textResultat.text = "Carregant resultats...";
        }

        if (GameManager.Instance.isSinglePlayer)
        {
            if (textResultat != null)
            {
                textResultat.text = GameManager.Instance.guanyadorLocalNom;
            }
        }
        else
        {
            string codiSala = GameManager.Instance.codiSala;

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
                    textResultat.text = "Error al carregar resultats.";
                }
            ));
        }

        if (WebSocketManager.Instance != null)
        {
            WebSocketManager.Instance.Disconnect();
        }
    }

    private void VolverAlMenu()
    {
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }

        SceneManager.LoadScene(nomEscenaMenu);
    }
}