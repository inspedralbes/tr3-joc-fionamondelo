using UnityEngine;

public class Destructible : MonoBehaviour
{
    public float destructionTime = 1f;

    [Range(0f,1f)]
    public float itemSpawnChance = 0.2f;
    public GameObject[] spawnableItems;

    private void Start()
    {
        Destroy(gameObject, destructionTime);
    }

    private void OnDestroy()
    {
        // Evita errores si la escena se está cerrando
        if (!gameObject.scene.isLoaded) return;

        // SOLO EL HOST (esPrimary) decide la suerte
        if (GameManager.Instance != null && GameManager.Instance.esPrimary)
        {
            if (spawnableItems.Length > 0 && Random.value < itemSpawnChance)
            {
                int randomIndex = Random.Range(0, spawnableItems.Length);
                
                // Spawnea localmente en el Host
                Instantiate(spawnableItems[randomIndex], transform.position, Quaternion.identity);

                // Avisa al Player 2 por red
                if (WebSocketManager.Instance != null)
                {
                    ItemSpawnMsg msg = new ItemSpawnMsg 
                    { 
                        x = transform.position.x, 
                        y = transform.position.y, 
                        itemIndex = randomIndex 
                    };
                    WebSocketManager.Instance.SendMessage("spawn_item", JsonUtility.ToJson(msg));
                }
            }
        }
    }

    // Estructura para enviar el mensaje por JSON
    [System.Serializable]
    public class ItemSpawnMsg
    {
        public float x;
        public float y;
        public int itemIndex;
    }
}