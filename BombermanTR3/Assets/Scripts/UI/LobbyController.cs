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
        botoIniciar.style.display = DisplayStyle.None;
        botoIniciar.clicked += OnIniciar;
        inputCodiSala = root.Q<TextField>("InputCodiSala");
        textCodiSala = root.Q<Label>("TextCodiSala");
        textEstat = root.Q<Label>("TextEstat");
        textError = root.Q<Label>("TextError");

        textCodiSala.style.display = DisplayStyle.None;
        textEstat.style.display = DisplayStyle.None;
        textError.style.display = DisplayStyle.None;

        botoCrearPartida.clicked += OnCrearPartida;
        botoUnirse.clicked += OnUnirse;
    }
    private void OnCrearPartida()
    {
        StartCoroutine(ApiManager.Instance.CrearPartida(
            (json) =>
            {
                Debug.Log("Partida creada: " + json);
                PartidaData data = JsonUtility.FromJson<PartidaData>(json);
                GameManager.Instance.codiSala = data.codiSala;
                GameManager.Instance.esPrimary = true;

                textEstat.style.display = DisplayStyle.Flex;
                textCodiSala.style.display = DisplayStyle.Flex;
                textCodiSala.text = "Codi: " + data.codiSala;

                StartCoroutine(EsperarJugador());
            },
            (error) =>
            {
                textError.text = "Error: " + error;
                textError.style.display = DisplayStyle.Flex;
            }
        ));
    }

    private void OnUnirse()
    {
        string codi = inputCodiSala.value;

        if (string.IsNullOrEmpty(codi))
        {
            textError.text = "Introdueix un codi de sala!";
            textError.style.display = DisplayStyle.Flex;
            return;
        }

        StartCoroutine(ApiManager.Instance.UnirsePartida(codi, GameManager.Instance.usuariId,
            (json) =>
            {
                GameManager.Instance.codiSala = codi;
                GameManager.Instance.esPrimary = false;
                _ = WebSocketManager.Instance.ConnectAsync(codi);
                SceneManager.LoadScene("GameScene");
            },
            (error) =>
            {
                textError.text = "Error: " + error;
                textError.style.display = DisplayStyle.Flex;
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
                    PartidaData data = JsonUtility.FromJson<PartidaData>(request.downloadHandler.text);

                    if (data.jugadors != null && data.jugadors.Length >= 1)
                    {
                        botoIniciar.style.display = DisplayStyle.Flex;
                        yield break;
                    }
                }
            }
        }
    }
    private void OnIniciar()
    {
        _ = WebSocketManager.Instance.ConnectAsync(GameManager.Instance.codiSala);
        SceneManager.LoadScene("GameScene");
    }

    [Serializable]
    private class PartidaData
    {
        public string codiSala;
        public string[] jugadors;
    }
}