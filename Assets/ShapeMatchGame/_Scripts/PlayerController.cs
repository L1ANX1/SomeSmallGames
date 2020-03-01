using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Mode { circle, box };

public class PlayerController : MonoBehaviour
{
    public Mode currentMode = Mode.circle;

    [HideInInspector] public Rigidbody2D rigidbody;
    public float speed;

    // Mode change, change below
    SpriteRenderer playerSpriteRender;
    public Image buttonImage;               // Button info, different from mode
    public Sprite circleSprite;
    public Sprite boxSprite;

    [HideInInspector] public GameController gameController;
    public static PlayerController _instance;


    public GameObject particleDestoryPrefab;
    public GameObject particleGameEndPrefab;

    // Move
    float moveHorizontal, moveVertical;
    Vector3 movement;
    bool isAddForce = false;

    private void Awake()
    {
        _instance = this;
    }

    // Use this for initialization
    void Start()
    {
        gameController = GameController._instance;
        rigidbody = GetComponent<Rigidbody2D>();
        playerSpriteRender = GetComponent<SpriteRenderer>();
        currentMode = Mode.circle;
        playerSpriteRender.sprite = circleSprite;
        buttonImage.sprite = boxSprite;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            OnClick();

        if (Input.GetKeyDown(KeyCode.Space))
            isAddForce = true;
    }

    void FixedUpdate()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        movement = new Vector3(moveHorizontal, moveVertical, 0.0f);
        rigidbody.velocity = movement * speed;
        if (isAddForce)
        {
            rigidbody.velocity = movement * speed * 5;
            isAddForce = false;
        }
        this.gameObject.transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime / 5f;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        bool isAddScore = false;
        switch (currentMode)
        {
            case Mode.circle:
                if (collision.name.StartsWith("BubbleCircle"))
                    isAddScore = true;
                break;
            case Mode.box:
                if (collision.name.StartsWith("BubbleBox"))
                    isAddScore = true;
                break;
            default:
                break;
        }
        if (isAddScore)
        {
            Instantiate(particleDestoryPrefab, collision.gameObject.transform.position, Quaternion.identity);
            //    collision.gameObject.GetComponent<Transform>().position, Quaternion.identity);
            Destroy(collision.gameObject);
            gameController.AddScore();
            if (this.gameObject.transform.localScale.x > 0.5f)
                this.gameObject.transform.localScale -= new Vector3(1, 1, 1) * 0.5f;

        }
        else
        {
            GameObject particleGameEnd = Instantiate(particleGameEndPrefab, this.gameObject.transform.position, Quaternion.identity);
            particleGameEnd.transform.localScale = this.gameObject.transform.localScale;
            this.gameObject.SetActive(false);
            gameController.GameEnd();// Invoke("GameEnd", 2f);
        }
    }

    public void OnClick()
    {
        switch (currentMode)
        {
            case Mode.circle:
                buttonImage.sprite = circleSprite;
                playerSpriteRender.sprite = boxSprite;
                currentMode = Mode.box;
                break;
            case Mode.box:
                buttonImage.sprite = boxSprite;
                playerSpriteRender.sprite = circleSprite;
                currentMode = Mode.circle;
                break;
            default:
                break;
        }
    }

}