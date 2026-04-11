using UnityEngine;
using System.Collections; 
using System.Collections.Generic;


public class BombController : MonoBehaviour
{

    [Header("Bomba")]
    public GameObject bombPrefab;
    public KeyCode inputKey = KeyCode.Space;
    public float bombFuseTime = 3f;
    public int bombAmount = 1;
    private int bombsRemaining;

[Header("Explosion")]
    public Explosion explosionPrefab;
    public float explosionDuration = 1f;
    public int explosionRadius = 1;

    private void OnEnable()
    {
        bombsRemaining = bombAmount;
    }

    private void Update()
    {
        if (bombsRemaining> 0 && Input.GetKeyDown(inputKey)){
            StartCoroutine(PlaceBomb());
        }
    }

    private IEnumerator PlaceBomb()
    {
        Vector2 position = transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        GameObject bomb = Instantiate(bombPrefab,position, Quaternion.identity);
        bombsRemaining --;

        yield return new WaitForSeconds(bombFuseTime);

        position = bomb.transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        Explosion explosion = Instantiate(explosionPrefab,position,Quaternion.identity);
        explosion.SetActiveRenderer(explosion.start);
        Destroy(explosion.gameObject,explosionDuration);
        
        Destroy(bomb);
        bombsRemaining++;
        }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Bomb")){
            other.isTrigger = false; 
        }
    }
}
