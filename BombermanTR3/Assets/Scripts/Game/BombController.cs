using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BombController : MonoBehaviour
{
    [Header("Bomb")]
    public KeyCode inputKey = KeyCode.LeftShift;
    public GameObject bombPrefab;
    public float bombFuseTime = 3f;
    public int bombAmount = 1;
    private int bombsRemaining;

    [Header("Explosion")]
    public Explosion explosionPrefab;
    public LayerMask explosionLayerMask;
    public float explosionDuration = 1f;
    public int explosionRadius = 1;

    [Header("Destructible")]
    public Tilemap destructibleTiles;
    public Destructible destructiblePrefab;

    private void OnEnable()
    {
        bombsRemaining = bombAmount;
    }

    private void Update()
    {
        if (bombsRemaining > 0 && Input.GetKeyDown(inputKey)) {
            StartCoroutine(PlaceBomb());
        }
    }

    private IEnumerator PlaceBomb()
    {
        // 1. Calculamos la posición exacta del centro de la celda actual
        Vector2 position = transform.position;
        position.x = Mathf.Floor(position.x) + 0.5f;
        position.y = Mathf.Floor(position.y) + 0.5f;

        // 2. Guardamos esta posición para la explosión posterior (posició fixa)
        Vector2 bombPosition = position;

        // 3. Instanciamos la bomba en esa celda
        GameObject bomb = Instantiate(bombPrefab, bombPosition, Quaternion.identity);
        bombsRemaining--;

        yield return new WaitForSeconds(bombFuseTime);

        // 4. La explosión ocurre exactamente en el centro de la celda donde se soltó
        Explosion explosion = Instantiate(explosionPrefab, bombPosition, Quaternion.identity);
        explosion.SetActiveRenderer(explosion.start);
        explosion.DestroyAfter(explosionDuration);

        // Disparamos la explosión en cruz
        Explode(bombPosition, Vector2.up, explosionRadius);
        Explode(bombPosition, Vector2.down, explosionRadius);
        Explode(bombPosition, Vector2.left, explosionRadius);
        Explode(bombPosition, Vector2.right, explosionRadius);

        if (bomb != null) {
            Destroy(bomb);
        }
        
        bombsRemaining++;
        
        Debug.Log("Bomba soltada en: " + bombPosition);
        Debug.Log("Jugador ahora en: " + transform.position);
    }

    private void Explode(Vector2 position, Vector2 direction, int length)
    {
        if (length <= 0) {
            return;
        }

        position += direction;

        if (Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, explosionLayerMask))
        {
            ClearDestructible(position);
            return;
        }

        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(length > 1 ? explosion.middle : explosion.end);
        explosion.SetDirection(direction);
        explosion.DestroyAfter(explosionDuration);

        Explode(position, direction, length - 1);
        Debug.Log("Explosio a: " + position);
    }

    private void ClearDestructible(Vector2 position)
    {
        Vector3Int cell = destructibleTiles.WorldToCell(position);
        TileBase tile = destructibleTiles.GetTile(cell);

        if (tile != null)
        {
            Instantiate(destructiblePrefab, position, Quaternion.identity);
            destructibleTiles.SetTile(cell, null);
        }
    }

    public void AddBomb()
    {
        bombAmount++;
        bombsRemaining++;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bomb")) {
            other.isTrigger = false;
        }
    }

}