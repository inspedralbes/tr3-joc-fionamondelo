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
        if (!gameObject.scene.isLoaded) return;

        if (GameManager.Instance != null && GameManager.Instance.esPrimary)
        {
            if (spawnableItems.Length > 0 && Random.value < itemSpawnChance)
            {
                int randomIndex = Random.Range(0, spawnableItems.Length);
                
                Instantiate(spawnableItems[randomIndex], transform.position, Quaternion.identity);

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

    [System.Serializable]
    public class ItemSpawnMsg
    {
        public float x;
        public float y;
        public int itemIndex;
    }
}