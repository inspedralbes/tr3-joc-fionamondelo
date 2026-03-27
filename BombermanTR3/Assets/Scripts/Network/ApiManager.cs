using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Text;

public class ApiManager : MonoBehaviour
{
    public static ApiManager Instance { get; private set; }
    private const string BASE_URL = "http://localhost:8080";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // --- MÈTODES DE L'API ---

    public IEnumerator RegistrarUsuari(string nomUsuari, string contrasenya, Action<string> onSuccess, Action<string> onError)
    {
        string url = BASE_URL + "/api/usuaris/registrar";
        UserRegistration data = new UserRegistration { nomUsuari = nomUsuari, alias = nomUsuari, contrasenya = contrasenya };
        yield return PostRequest(url, JsonUtility.ToJson(data), onSuccess, onError);
    }

    public IEnumerator LoginUsuari(string nomUsuari, string contrasenya, Action<string> onSuccess, Action<string> onError)
    {
        string url = BASE_URL + "/api/usuaris/login";
        UserLogin data = new UserLogin { nomUsuari = nomUsuari, contrasenya = contrasenya };
        yield return PostRequest(url, JsonUtility.ToJson(data), onSuccess, onError);
    }

    public IEnumerator CrearPartida(Action<string> onSuccess, Action<string> onError)
{
    string url = BASE_URL + "/api/partides/crear";
    yield return PostRequest(url, "{}", onSuccess, onError);
}

    public IEnumerator UnirsePartida(string codiSala, string usuariId, Action<string> onSuccess, Action<string> onError)
    {
        string url = BASE_URL + "/api/partides/unirse";
        JoinGame data = new JoinGame { codiSala = codiSala, usuariId = usuariId };
        yield return PostRequest(url, JsonUtility.ToJson(data), onSuccess, onError);
    }

    public IEnumerator FinalitzarPartida(string codiSala, string guanyadorId, Action<string> onSuccess, Action<string> onError)
    {
        string url = BASE_URL + "/api/partides/finalitzar";
        EndGame data = new EndGame { codiSala = codiSala, guanyadorId = guanyadorId };
        yield return PostRequest(url, JsonUtility.ToJson(data), onSuccess, onError);
    }

    // --- HELPER PER FER EL POST AMB JSON ---

    private IEnumerator PostRequest(string url, string jsonData, Action<string> onSuccess, Action<string> onError)
    {
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                onSuccess?.Invoke(request.downloadHandler.text);
            }
            else
            {
                onError?.Invoke(request.error);
            }
        }
    }

    // --- CLASSES PER AL JSON ---

    [Serializable]
    private class UserRegistration { public string nomUsuari; public string alias; public string contrasenya; }
    [Serializable]
    private class UserLogin { public string nomUsuari; public string contrasenya; }
    [Serializable]
    private class JoinGame { public string codiSala; public string usuariId; }
    [Serializable]
    private class EndGame { public string codiSala; public string guanyadorId; }
   
}
