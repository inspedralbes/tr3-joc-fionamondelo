using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;
using System;

public class LobbyController : MonoBehaviour
{
    private Button botoCrearPartida;
    private Button botoUnirse;
    private Button botoIniciar;
    private TextField inputCodiSala;
    private Label textCodiSala;
    private Label textEstat;
    private Label textError;

    private void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        botoCrearPartida = root.Q<Button>("BotoCrearPartida");
        botoUnirse = root.Q<Button>("BotoUnirse");
        botoIniciar = root.Q<Button>("BotoIniciar");
        inputCodiSala = root.Q<TextField>("InputCodiSala");
        textCodiSala = root.Q<Label>("TextCodiSala");
        textEstat = root.Q<Label>("TextEstat");
        textError = root.Q<Label>("TextError");

        botoIniciar.style.display = DisplayStyle.None;
        botoIniciar.clicked += OnIniciar;

        textCodiSala.style.display = DisplayStyle.None;
        textEstat.style.display = DisplayStyle.None;

        botoCrearPartida.clicked += OnCrearPartida;
        botoUnirse.clicked += OnUnirse;

        if (WebSocketManager.Instance != null)
        {
            WebSocketManager.Instance.OnMissatgeRebut += PotserComencarPartida;
        }
    }

    private void PotserComencarPartida(string tipus, string json)
    {
        if (tipus == "comencar_partida")
        {
            SceneManager.LoadScene("GameScene");
        }
    }

    private void OnDestroy()
    {
        if (WebSocketManager.Instance != null)
        {
            WebSocketManager.Instance.OnMissatgeRebut -= PotserComencarPartida;
        }
    }

    private void OnCrearPartida()
    {
        textError.text = "Creant sala...";
        textError.style.display = DisplayStyle.Flex;
        DesactivarBotons();

        StartCoroutine(ApiManager.Instance.CrearPartida(
            (json) =>
            {
                ApiManager.PartidaData data = JsonUtility.FromJson<ApiManager.PartidaData>(json);
                GameManager.Instance.codiSala = data.codiSala;
                GameManager.Instance.esPrimary = true;

                textCodiSala.style.display = DisplayStyle.Flex;
                textCodiSala.text = "Sala: " + data.codiSala;

                StartCoroutine(ApiManager.Instance.UnirsePartida(data.codiSala, GameManager.Instance.usuariId,
                    async (joinJson) => {
                        textEstat.style.display = DisplayStyle.Flex;
                        textEstat.text = "Esperant rival...";
                        textError.style.display = DisplayStyle.None;
                        
                        await WebSocketManager.Instance.ConnectAsync(data.codiSala);
                        StartCoroutine(EsperarJugador());
                    },
                    (joinError) => {
                        textError.text = "Error al unir-se: " + joinError;
                        ActivarBotons();
                    }
                ));
            },
            (error) => {
                textError.text = "Error: " + error;
                ActivarBotons();
            }
        ));
    }

    private void OnUnirse()
    {
        string codi = inputCodiSala.value;
        if (string.IsNullOrEmpty(codi)) return;

        textError.text = "Unint-se...";
        textError.style.display = DisplayStyle.Flex;
        DesactivarBotons();

        StartCoroutine(ApiManager.Instance.UnirsePartida(codi, GameManager.Instance.usuariId,
            async (json) =>
            {
                GameManager.Instance.codiSala = codi;
                GameManager.Instance.esPrimary = false;

                textEstat.style.display = DisplayStyle.Flex;
                textEstat.text = "Esperant que l'amfitrió comenci...";
                textError.style.display = DisplayStyle.None;

                await WebSocketManager.Instance.ConnectAsync(codi);
                
                botoCrearPartida.style.display = DisplayStyle.None;
                botoUnirse.style.display = DisplayStyle.None;
                inputCodiSala.style.display = DisplayStyle.None;
            },
            (error) =>
            {
                textError.text = "Error: " + error;
                ActivarBotons();
            }
        ));
    }

    private IEnumerator EsperarJugador()
    {
        string url = "http://localhost:8080/api/partides/" + GameManager.Instance.codiSala;
        while (true)
        {
            yield return new WaitForSeconds(2f);
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                yield return request.SendWebRequest();
                if (request.result == UnityWebRequest.Result.Success)
                {
                    ApiManager.PartidaData data = JsonUtility.FromJson<ApiManager.PartidaData>(request.downloadHandler.text);
                    if (data.jugadors != null && data.jugadors.Length >= 2)
                    {
                        textEstat.text = "Rival connectat!";
                        botoIniciar.style.display = DisplayStyle.Flex;
                        yield break;
                    }
                }
            }
        }
    }

    private void OnIniciar()
    {
        if (WebSocketManager.Instance != null)
        {
            WebSocketManager.Instance.SendMessage("comencar_partida", "{}");
        }
        SceneManager.LoadScene("GameScene");
    }

    private void DesactivarBotons()
    {
        botoCrearPartida.SetEnabled(false);
        botoUnirse.SetEnabled(false);
    }

    private void ActivarBotons()
    {
        botoCrearPartida.SetEnabled(true);
        botoUnirse.SetEnabled(true);
    }
}