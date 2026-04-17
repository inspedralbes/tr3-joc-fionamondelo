using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

public class MovementController : MonoBehaviour
{
    private float lastSendTime;
    public float networkSendRate = 0.033f;
    private Vector2 targetPosition;

    public new Rigidbody2D rigidbody { get; private set; }
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
    public bool controlledByAI = false;

    private void Start()
    {
        if (WebSocketManager.Instance != null)
        {
            WebSocketManager.Instance.OnMissatgeRebut += OnMissatgeRebut;
        }
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        activeSpriteRenderer = spriteRendererDown;
        targetPosition = rigidbody.position;
    }

    private void Update()
    {
        if (!esMeu || controlledByAI) return;

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

        if (lastDirection != Vector2.zero && direction == Vector2.zero)
        {
            EnviarPosicio();
        }
    }

    private void FixedUpdate()
    {
        if (esMeu)
        {
            Vector2 position = rigidbody.position;
            Vector2 translation = direction * speed * Time.fixedDeltaTime;
            rigidbody.MovePosition(position + translation);

            if (direction != Vector2.zero && Time.time - lastSendTime > networkSendRate)
            {
                EnviarPosicio();
                lastSendTime = Time.time;
            }
        }
        else
        {
            Vector2 newPos = Vector2.MoveTowards(
                rigidbody.position,
                targetPosition,
                speed * 1.2f * Time.fixedDeltaTime
            );
            rigidbody.MovePosition(newPos);
        }
    }

    private void EnviarPosicio()
    {
        if (GameManager.Instance != null && GameManager.Instance.isSinglePlayer) return;

        if (WebSocketManager.Instance != null)
        {
            string msg = "{\"x\":" + rigidbody.position.x.ToString(CultureInfo.InvariantCulture) +
                         ",\"y\":" + rigidbody.position.y.ToString(CultureInfo.InvariantCulture) +
                         ",\"dx\":" + direction.x.ToString(CultureInfo.InvariantCulture) +
                         ",\"dy\":" + direction.y.ToString(CultureInfo.InvariantCulture) + "}";
            WebSocketManager.Instance.SendMessage("moure", msg);
        }
    }

    private void OnMissatgeRebut(string tipus, string json)
    {
        if (esMeu) return;

        if (tipus == "moure")
        {
            try
            {
                PosicioData data = JsonUtility.FromJson<PosicioData>(json);
                targetPosition = new Vector2(data.x, data.y);

                Vector2 novaDireccio = new Vector2(data.dx, data.dy);
                ActualitzarAnimacioRemota(novaDireccio);
            }
            catch (System.Exception)
            {
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

    public void SetDirection(Vector2 newDirection, AnimatedSpriteRenderer spriteRenderer)
    {
        direction = newDirection;

        spriteRendererUp.enabled = spriteRenderer == spriteRendererUp;
        spriteRendererDown.enabled = spriteRenderer == spriteRendererDown;
        spriteRendererLeft.enabled = spriteRenderer == spriteRendererLeft;
        spriteRendererRight.enabled = spriteRenderer == spriteRendererRight;

        activeSpriteRenderer = spriteRenderer;
        activeSpriteRenderer.idle = direction == Vector2.zero;
    }

    public void SetAIDirection(Vector2 newDirection)
    {
        AnimatedSpriteRenderer targetSprite = activeSpriteRenderer;
        if (newDirection == Vector2.up) targetSprite = spriteRendererUp;
        else if (newDirection == Vector2.down) targetSprite = spriteRendererDown;
        else if (newDirection == Vector2.left) targetSprite = spriteRendererLeft;
        else if (newDirection == Vector2.right) targetSprite = spriteRendererRight;

        SetDirection(newDirection, targetSprite);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
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