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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        birdsIsAlive = false; // el juego comienza inactivo
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();

        // freeze bird
        myRigidBody.bodyType = RigidbodyType2D.Static;


        //Bird Skin
        int skinIndex = PlayerPrefs.GetInt("BirdSkin", 0);

        if (skinIndex == 0)
        {
            spriteRenderer.sprite = birdSprite;
            transform.localScale = Vector3.one;
        }
        else
        {
            spriteRenderer.sprite = headSprite;
            transform.localScale = new Vector3(0.58f, 0.58f, 0.58f);
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
        if (!birdsIsAlive) return;

        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Pipe"))
        {
            logic.gameOver();
            birdsIsAlive = false;
        }
    }

}
