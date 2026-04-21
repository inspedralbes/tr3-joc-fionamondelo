using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private Label textBenvinguda;
    private Button botoIndividual;
    private Button botoMultijugador;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        textBenvinguda = root.Q<Label>("TextBenvinguda");
        botoIndividual = root.Q<Button>("BotoIndividual");
        botoMultijugador = root.Q<Button>("BotoMultijugador");

        textBenvinguda.text = "Benvingut, " + GameManager.Instance.nomUsuari + "!";

        botoIndividual.clicked += CarregaEscenaJoc;
        botoMultijugador.clicked += CarregaEscenaLobby;
    }

    void CarregaEscenaJoc()
    {
        if (GameManager.Instance != null) {
            GameManager.Instance.isSinglePlayer = true;
            GameManager.Instance.esPrimary = true; // El jugador 1 sempre és el local en individual
        }
        SceneManager.LoadScene("GameScene");
    }

    void CarregaEscenaLobby()
    {
        if (GameManager.Instance != null) {
            GameManager.Instance.isSinglePlayer = false;
        }
        SceneManager.LoadScene("LobbyScene");
    }
}
