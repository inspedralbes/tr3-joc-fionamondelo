using UnityEngine;
using System.Collections; 
using System.Collections.Generic;

public class MovementController: MonoBehaviour  
{
    public new Rigidbody2D rigidbody {get; private set;}
    private Vector2 direction = Vector2.zero;
    public float speed = 5f;

    public KeyCode inputUp = KeyCode.W;
    public KeyCode inputDown = KeyCode.S;
    public KeyCode inputLeft = KeyCode.A;
    public KeyCode inputRight = KeyCode.D;

    public AnimatedSpriteRenderer spriteRendererUp;
    public AnimatedSpriteRenderer spriteRendererDown;
    public AnimatedSpriteRenderer spriteRendererLeft;
    public AnimatedSpriteRenderer spriteRendererRight;
    public AnimatedSpriteRenderer spriteRendererDeath;
    private AnimatedSpriteRenderer activeSpriteRenderer;

    public bool esMeu = true;

    private void Start()
    {
        Debug.Log("MovementController: " + gameObject.name + " iniciat. esMeu = " + esMeu);
        
        if (WebSocketManager.Instance != null)
        {
            WebSocketManager.Instance.OnMissatgeRebut += OnMissatgeRebut;
        }
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        activeSpriteRenderer = spriteRendererDown;
    }

    private void Update()
    {
        if (!esMeu) return;

        Vector2 lastDirection = direction;

        if (Input.GetKey(inputUp)) {
            SetDirection(Vector2.up, spriteRendererUp);
        } else if (Input.GetKey(inputDown)) {
            SetDirection(Vector2.down, spriteRendererDown);
        } else if (Input.GetKey(inputLeft)) {
            SetDirection(Vector2.left, spriteRendererLeft);
        } else if (Input.GetKey(inputRight)) {
            SetDirection(Vector2.right, spriteRendererRight);
        } else {
            SetDirection(Vector2.zero, activeSpriteRenderer);
        }

        // Si la direcció ha canviat i ara estem parats, enviem un missatge final per aturar el personatge remot
        if (lastDirection != Vector2.zero && direction == Vector2.zero)
        {
            EnviarPosicio();
        }
    }

    private void FixedUpdate()
    {
        if (!esMeu) return;

        Vector2 position = rigidbody.position;
        Vector2 translation = direction * speed * Time.fixedDeltaTime;

        rigidbody.MovePosition(position + translation);

        if (direction != Vector2.zero)
        {
            EnviarPosicio();
        }
    }

    private void EnviarPosicio()
    {
        if (WebSocketManager.Instance != null)
        {
            string msg = "{\"x\":" + rigidbody.position.x + 
                         ",\"y\":" + rigidbody.position.y + 
                         ",\"dx\":" + direction.x + 
                         ",\"dy\":" + direction.y + "}";
            WebSocketManager.Instance.SendMessage("moure", msg);
        }
    }

    private void OnMissatgeRebut(string tipus, string json)
    {
        if (esMeu) return;

        if (tipus == "moure")
        {
            PosicioData data = JsonUtility.FromJson<PosicioData>(json);
            rigidbody.MovePosition(new Vector2(data.x, data.y));
            
            // Actualitzem la direcció per a les animacions
            Vector2 novaDireccio = new Vector2(data.dx, data.dy);
            if (novaDireccio != direction)
            {
                ActualitzarAnimacioRemota(novaDireccio);
            }
        }
    }

    private void ActualitzarAnimacioRemota(Vector2 novaDireccio)
    {
        direction = novaDireccio;
        if (direction == Vector2.up) activeSpriteRenderer = spriteRendererUp;
        else if (direction == Vector2.down) activeSpriteRenderer = spriteRendererDown;
        else if (direction == Vector2.left) activeSpriteRenderer = spriteRendererLeft;
        else if (direction == Vector2.right) activeSpriteRenderer = spriteRendererRight;

        spriteRendererUp.enabled = activeSpriteRenderer == spriteRendererUp;
        spriteRendererDown.enabled = activeSpriteRenderer == spriteRendererDown;
        spriteRendererLeft.enabled = activeSpriteRenderer == spriteRendererLeft;
        spriteRendererRight.enabled = activeSpriteRenderer == spriteRendererRight;

        activeSpriteRenderer.idle = direction == Vector2.zero;
    }

    private void OnDestroy()
    {
        if (WebSocketManager.Instance != null)
        {
            WebSocketManager.Instance.OnMissatgeRebut -= OnMissatgeRebut;
        }
    }

    private void SetDirection(Vector2 newDirection, AnimatedSpriteRenderer spriteRenderer)
    {
        direction = newDirection;
        
        spriteRendererUp.enabled = spriteRenderer == spriteRendererUp; 
        spriteRendererDown.enabled = spriteRenderer == spriteRendererDown;
        spriteRendererLeft.enabled = spriteRenderer == spriteRendererLeft;
        spriteRendererRight.enabled = spriteRenderer == spriteRendererRight;

        activeSpriteRenderer = spriteRenderer;
        activeSpriteRenderer.idle = direction == Vector2.zero;
    }

     private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Explosion")){
            DeathSequence();
        }
    }

        private void DeathSequence()
    {
        enabled = false;
        GetComponent<BombController>().enabled = false;
        
        spriteRendererUp.enabled = false;
        spriteRendererDown.enabled = false;
        spriteRendererLeft.enabled = false;
        spriteRendererRight.enabled = false;
        spriteRendererDeath.enabled = true;

        Invoke(nameof(OnDeathSequenceEnded), 1.25f);
    }

    private void OnDeathSequenceEnded()
    {
        gameObject.SetActive(false);
        GameManager.Instance.CheckWinState();
    }
    
}

[System.Serializable]
public class PosicioData
{
    public float x;
    public float y;
    public float dx;
    public float dy;
    public string tipus;
}