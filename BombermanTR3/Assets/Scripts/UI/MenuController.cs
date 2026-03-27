using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public TextMeshProUGUI textBenvinguda;
    public Button botoIndividual;
    public Button botoMultijugador;

    void Start()
    {
        textBenvinguda.text = "Benvingut, " + GameManager.Instance.nomUsuari + "!";

        botoIndividual.onClick.AddListener(CarregaEscenaJoc);
        botoMultijugador.onClick.AddListener(CarregaEscenaLobby);
    }

    void CarregaEscenaJoc()
    {
        SceneManager.LoadScene("GameScene");
    }

    void CarregaEscenaLobby()
    {
        SceneManager.LoadScene("LobbyScene");
    }
}
