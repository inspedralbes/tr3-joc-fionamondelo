using UnityEngine;

public class Bomba : MonoBehaviour
{
    // Prefabs del foc que l'usuari arrossegarà des de Unity
    public GameObject focInici;
    public GameObject focMig;
    public GameObject focPunta;

    // Capa per detectar parets i maons (Brick)
    public LayerMask capaObstacles;

    private void Start()
    {
        // Forçar Z=0 i Sorting Order per a que es vegi sempre
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        GetComponent<SpriteRenderer>().sortingOrder = 50;

        Debug.Log("Bomba activada, explotarà en 2 segons...");
        Invoke("Explotar", 2f);
    }

    private void Explotar()
    {
        Debug.Log("BOOM! La bomba explota.");
        // 1. Crear el foc central i fer que s'esborri en 1 segon
        // Ens assegurem que Z sigui 0
        Vector3 posicioCentral = new Vector3(transform.position.x, transform.position.y, 0f);
        GameObject centro = Instantiate(focInici, posicioCentral, Quaternion.identity);
        Destroy(centro, 1f);

        // 2. Crear explosions en les 4 direccions
        CreaExplosioDireccio(Vector2.up);
        CreaExplosioDireccio(Vector2.down);
        CreaExplosioDireccio(Vector2.left);
        CreaExplosioDireccio(Vector2.right);

        // 3. Destruir l'objecte bomba
        Destroy(gameObject);
    }

    private void CreaExplosioDireccio(Vector2 direccio)
    {
        float angle = Mathf.Atan2(direccio.y, direccio.x) * Mathf.Rad2Deg;
        Quaternion rotacio = Quaternion.Euler(0, 0, angle);

        // Bucle para crear los 2 bloques de alcance (Medio y Punta)
        for (int i = 1; i <= 2; i++)
        {
            Vector3 posicioFoc = new Vector3(
                transform.position.x + (direccio.x * i),
                transform.position.y + (direccio.y * i),
                0f
            );

            // 1. Mirar si hay un obstáculo antes de poner el fuego
            Collider2D hit = Physics2D.OverlapPoint(posicioFoc, capaObstacles);

            if (hit != null)
            {
                // Si es un Muro (pared gris), paramos el rayo sin poner fuego en este bloque
                if (hit.CompareTag("Muro"))
                {
                    break;
                }

                // Si es un Ladrillo, lo borramos y ponemos la Punta de fuego
                if (hit.CompareTag("Ladrillo"))
                {
                    Destroy(hit.gameObject);
                    GameObject punta = Instantiate(focPunta, posicioFoc, rotacio);
                    Destroy(punta, 1f);
                    break; // Paramos el rayo aquí
                }
            }

            // 2. Si no hay nada, ponemos el fuego que toca
            GameObject segment;
            if (i == 1)
            {
                segment = Instantiate(focMig, posicioFoc, rotacio);
            }
            else
            {
                segment = Instantiate(focPunta, posicioFoc, rotacio);
            }

            Destroy(segment, 1f);
        }
    }
}
