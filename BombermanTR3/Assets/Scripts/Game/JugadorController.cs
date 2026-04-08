using UnityEngine;

public class JugadorController : MonoBehaviour
{

    public float velocitat = 5f;
    public bool esMeu = true;
    
    // Prefab de la bomba para arrastrar desde Unity
    public GameObject bombaPrefab;

    private Rigidbody2D rb;

    private Animator anim;
    private Vector2 moviment;
    private Vector2 ultimaDireccio;

    private void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        rb.gravityScale = 0f;
     
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        ultimaDireccio = Vector2.down;
    }

    private void Update()
    {
    
        if (!esMeu) return;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        moviment = new Vector2(h, v);


        bool isMoving = moviment.magnitude > 0.01f;

        anim.SetBool("isMoving", isMoving);

        if (isMoving)
        {

            ultimaDireccio = moviment;
            anim.SetFloat("MoveX", moviment.x);
            anim.SetFloat("MoveY", moviment.y);
        }
        else
        {
            anim.SetFloat("MoveX", ultimaDireccio.x);
            anim.SetFloat("MoveY", ultimaDireccio.y);
            
            rb.linearVelocity = Vector2.zero;
        }

        // 4. Poner bomba al pulsar Espacio
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PosaBomba();
        }
    }

    private void PosaBomba()
    {
        if (bombaPrefab != null)
        {
            // Redondegem la posicio i fixem Z=0 per a que sigui visible en 2D
            Vector3 posicioBomba = new Vector3(
                Mathf.RoundToInt(transform.position.x),
                Mathf.RoundToInt(transform.position.y),
                0f
            );

            // Instanciem la bomba
            Instantiate(bombaPrefab, posicioBomba, Quaternion.identity);
            Debug.Log("Bomba instanciada en: " + posicioBomba);
        }
    }

    private void FixedUpdate()
    {
        if (!esMeu) return;

        if (moviment.magnitude > 0.01f)
        {
            Vector2 direccionNormalizada = moviment.normalized;
            
            Vector2 nuevaPos = rb.position + direccionNormalizada * velocitat * Time.fixedDeltaTime;
            
            rb.MovePosition(nuevaPos);
        }
    }

    public void ActualitzarPosicio(float x, float y)
    {
        transform.position = new Vector3(x, y, transform.position.z);
    }
}