using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public string usuariId;
    public string nomUsuari;
    public string codiSala;
    public bool esPrimary;

    private void Awake()
    {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
        }
    }
    public void CheckWinState()
    {
        
    }
}