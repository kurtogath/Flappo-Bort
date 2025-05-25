using UnityEngine;


public class BirdScript : MonoBehaviour
{

    public Rigidbody2D myRigidBody;
    public float flapStrength;
    public LogicScript logic;
    public bool birdsIsAlive = true;
    public bool canFlap = false;

    public SpriteRenderer spriteRenderer;
    public Sprite birdSprite;
    public Sprite headSprite;
    public Sprite damagedHeadSprite;
    public Sprite damagedBirdSprite;

    void Start()
    {
        birdsIsAlive = false; // el juego comienza inactivo
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();

        // freeze bird
        myRigidBody.bodyType = RigidbodyType2D.Static;


        //Bird Skin
        int skinIndex = PlayerPrefs.GetInt("BirdSkin", 0);
        CircleCollider2D collider = GetComponent<CircleCollider2D>();

        if (skinIndex == 0)
        {
            spriteRenderer.sprite = birdSprite;
            transform.localScale = Vector3.one;
            collider.radius = 2.18f;
            collider.offset = new Vector2(-0.124f, -0.45f);
        }
        else
        {
            spriteRenderer.sprite = headSprite;
            transform.localScale = new Vector3(0.58f, 0.58f, 0.58f);
            collider.radius = 3.25f;
            collider.offset = new Vector2(-0.04f, -0.1f);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) == true && birdsIsAlive && canFlap)
        {
            myRigidBody.linearVelocity = Vector2.up * flapStrength;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!birdsIsAlive ||
            !(collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Pipe")))
            return;

        logic.gameOver();
        birdsIsAlive = false;

        UpdateSpriteOnDeath();
        ApplyKnockbackEffect();
    }

    private void UpdateSpriteOnDeath()
    {
        int skinIndex = PlayerPrefs.GetInt("BirdSkin", 0);
        spriteRenderer.sprite = (skinIndex == 1) ? damagedHeadSprite : damagedBirdSprite;
    }

    private void ApplyKnockbackEffect()
    {
        myRigidBody.bodyType = RigidbodyType2D.Dynamic;
        myRigidBody.linearVelocity = Vector2.zero;

        Vector2 knockbackDirection = new Vector2(-1, 1).normalized;
        myRigidBody.AddForce(knockbackDirection * 250);
        myRigidBody.AddTorque(10f);
    }


}
