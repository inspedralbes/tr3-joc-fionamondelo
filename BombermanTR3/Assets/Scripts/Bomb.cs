using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bomb : MonoBehaviour
{
    // Todas estas variables las rellenan el GameManager y el BombController automáticamente
    [HideInInspector] public float fuseTime;
    [HideInInspector] public Explosion explosionPrefab;
    [HideInInspector] public LayerMask explosionLayerMask;
    [HideInInspector] public float explosionDuration;
    [HideInInspector] public int explosionRadius;
    [HideInInspector] public Tilemap destructibleTiles;
    [HideInInspector] public Destructible destructiblePrefab;

    private void Start()
    {
        // En cuanto la bomba aparece en escena, empieza la cuenta atrás
        StartCoroutine(ExplosionRoutine());
    }

    private IEnumerator ExplosionRoutine()
    {
        // Espera el tiempo exacto que tienes configurado en Unity
        yield return new WaitForSeconds(fuseTime);

        Vector2 position = transform.position;

        // 1. Explosión en el centro
        Explosion centerExplosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        centerExplosion.SetActiveRenderer(centerExplosion.start);
        centerExplosion.DestroyAfter(explosionDuration);

        // 2. Explosiones en cruz
        ExplodeDirection(position, Vector2.up, explosionRadius);
        ExplodeDirection(position, Vector2.down, explosionRadius);
        ExplodeDirection(position, Vector2.left, explosionRadius);
        ExplodeDirection(position, Vector2.right, explosionRadius);

        // 3. Destruir el objeto bomba
        Destroy(gameObject);
    }

    private void ExplodeDirection(Vector2 position, Vector2 direction, int length)
    {
        if (length <= 0) return;

        position += direction;

        // Si choca con un muro u obstáculo
        if (Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, explosionLayerMask))
        {
            ClearDestructible(position);
            return; // Detiene el fuego en esta dirección
        }

        // Crear fuego
        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(length > 1 ? explosion.middle : explosion.end);
        explosion.SetDirection(direction);
        explosion.DestroyAfter(explosionDuration);

        // Llamada recursiva para seguir expandiendo el fuego
        ExplodeDirection(position, direction, length - 1);
    }

    private void ClearDestructible(Vector2 position)
    {
        if (destructibleTiles == null) return;

        Vector3Int cell = destructibleTiles.WorldToCell(position);
        TileBase tile = destructibleTiles.GetTile(cell);

        if (tile != null)
        {
            Instantiate(destructiblePrefab, position, Quaternion.identity);
            destructibleTiles.SetTile(cell, null);
        }
    }
}