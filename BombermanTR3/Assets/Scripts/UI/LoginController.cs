using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class LoginController : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField inputUsuari;
    public TMP_InputField inputContrasenya;
    public Button botoLogin;
    public Button botoRegistre;
    public TextMeshProUGUI textMissatge;

    private void Start()
    {
        botoLogin.onClick.AddListener(OnLogin);
        botoRegistre.onClick.AddListener(OnRegistre);
    }

    private void OnLogin()
    {
        string nom = inputUsuari.text;
        string pass = inputContrasenya.text;

        if (string.IsNullOrEmpty(nom) || string.IsNullOrEmpty(pass))
        {
            textMissatge.text = "Error: camps buits.";
            return;
        }

        textMissatge.text = "Connectant...";
        DesactivarBotons();

        StartCoroutine(ApiManager.Instance.LoginUsuari(nom, pass, 
            (json) => {
                
                UserData data = JsonUtility.FromJson<UserData>(json);
                GameManager.Instance.usuariId = data._id;
                GameManager.Instance.nomUsuari = data.nomUsuari;


                SceneManager.LoadScene("LobbyScene");
            }, 
            (error) => {
                textMissatge.text = "Error: " + error;
                ActivarBotons();
            }
        ));
    }

    private void OnRegistre()
    {
        string nom = inputUsuari.text;
        string pass = inputContrasenya.text;

        if (string.IsNullOrEmpty(nom) || string.IsNullOrEmpty(pass))
        {
            textMissatge.text = "Error: camps buits.";
            return;
        }

        textMissatge.text = "Registrant...";
        DesactivarBotons();

        // Crida al registre de l'ApiManager
        StartCoroutine(ApiManager.Instance.RegistrarUsuari(nom, pass, 
            (json) => {
                // Registre OK, fem login automàtic
                OnLogin();
            }, 
            (error) => {
    textMissatge.text = "Error: " + error;
    Debug.LogError("Error registre: " + error);
    ActivarBotons();
}
        ));
    }

    private void ActivarBotons()
    {
        botoLogin.interactable = true;
        botoRegistre.interactable = true;
    }

    private void DesactivarBotons()
    {
        botoLogin.interactable = false;
        botoRegistre.interactable = false;
    }

    // Classe auxiliar per al JSON
    [Serializable]
    private class UserData { public string _id; public string nomUsuari; }
}
