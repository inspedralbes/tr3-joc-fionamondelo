using UnityEngine;

public class JugadorController : MonoBehaviour
{
    public float velocitat = 5f;
    public bool esMeu = true;

    private Rigidbody2D rb;
    private Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (esMeu)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            rb.linearVelocity = new Vector2(h * velocitat, v * velocitat);

            // Direccions: avall=0, amunt=1, dreta=2, esquerra=3
            if (v < 0) anim.SetInteger("direccio", 0);
            else if (v > 0) anim.SetInteger("direccio", 1);
            else if (h > 0) anim.SetInteger("direccio", 2);
            else if (h < 0) anim.SetInteger("direccio", 3);
        }
    }

    public void ActualitzarPosicio(float x, float y)
    {
        transform.position = new Vector3(x, y, transform.position.z);
    }
}
