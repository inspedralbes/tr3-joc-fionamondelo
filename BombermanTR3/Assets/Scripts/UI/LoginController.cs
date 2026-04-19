using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System;

public class LoginController : MonoBehaviour
{
    private TextField inputUsuari;
    private TextField inputContrasenya;
    private Button botoLogin;
    private Button botoRegistre;
    private Label textMissatge;
    private Button botoTema;
    private VisualElement contenidor;
    private bool esModoDia = false;
    private const string PrefTema = "TemaDia";

    private void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        inputUsuari = root.Q<TextField>("InputUsuari");
        inputContrasenya = root.Q<TextField>("InputContrasenya");
        botoLogin = root.Q<Button>("BotoLogin");
        botoRegistre = root.Q<Button>("BotoRegistre");
        textMissatge = root.Q<Label>("TextMissatge");
        botoTema = root.Q<Button>("BotoTema");
        contenidor = root.Q<VisualElement>("Contenidor");

        botoLogin.clicked += OnLogin;
        botoRegistre.clicked += OnRegistre;
        botoTema.clicked += AlternarTema;

        CarregarTema();
    }

    private void CarregarTema()
    {
        esModoDia = PlayerPrefs.GetInt(PrefTema, 0) == 1;
        AplicarTema();
    }

    private void AlternarTema()
    {
        esModoDia = !esModoDia;
        PlayerPrefs.SetInt(PrefTema, esModoDia ? 1 : 0);
        PlayerPrefs.Save();
        AplicarTema();
    }

    private void AplicarTema()
    {
        if (esModoDia)
        {
            contenidor.AddToClassList("tema-clau");
            botoTema.text = "Mode Nit";
        }
        else
        {
            contenidor.RemoveFromClassList("tema-clau");
            botoTema.text = "Mode Dia";
        }
    }

    private void OnLogin()
    {
        string nom = inputUsuari.value;
        string pass = inputContrasenya.value;

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

                SceneManager.LoadScene("MenuScene");
            }, 
            (error) => {
                textMissatge.text = "Error: " + error;
                ActivarBotons();
            }
        ));
    }

    private void OnRegistre()
    {
        string nom = inputUsuari.value;
        string pass = inputContrasenya.value;

        if (string.IsNullOrEmpty(nom) || string.IsNullOrEmpty(pass))
        {
            textMissatge.text = "Error: camps buits.";
            return;
        }

        textMissatge.text = "Registrant...";
        DesactivarBotons();

        StartCoroutine(ApiManager.Instance.RegistrarUsuari(nom, pass, 
            (json) => {
                OnLogin();
            }, 
            (error) => {
                textMissatge.text = "Error: " + error;
                ActivarBotons();
            }
        ));
    }

    private void ActivarBotons()
    {
        botoLogin.SetEnabled(true);
        botoRegistre.SetEnabled(true);
    }

    private void DesactivarBotons()
    {
        botoLogin.SetEnabled(false);
        botoRegistre.SetEnabled(false);
    }

    [Serializable]
    private class UserData { public string _id; public string nomUsuari; }
}
