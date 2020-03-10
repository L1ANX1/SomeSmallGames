using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ShapeMatchGame
{

    public class PlayerController : MonoBehaviour
    {
        [HideInInspector] public GameController gameController;
        [HideInInspector] public Rigidbody2D rigidbody;
        public static PlayerController _instance;

        [Header("Player move Speed:")]
        public float playerSpeed;
        // Move
        float moveHorizontal = 0, moveVertical = 0;
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
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
                gameController.ChangeMode();

            if (Input.GetKeyDown(KeyCode.Space))
                isAddForce = true;
        }

        void FixedUpdate()
        {
            moveHorizontal = Input.GetAxis("Horizontal");
            moveVertical = Input.GetAxis("Vertical");
            movement = new Vector3(moveHorizontal, moveVertical, 0.0f);
            rigidbody.velocity = movement * playerSpeed;
            if (isAddForce)
            {
                rigidbody.velocity = movement * playerSpeed * 5;
                isAddForce = false;
            }
            this.gameObject.transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime / 5f;
            if (playerSpeed > 10f) playerSpeed -= this.gameObject.transform.localScale.x * Time.deltaTime / 5f;
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            bool isAddScore = false;
            switch (gameController.gameManager.currentMode)
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
                Instantiate(gameController.particleDestoryPrefab, collision.gameObject.transform.position, Quaternion.identity);
                //    collision.gameObject.GetComponent<Transform>().position, Quaternion.identity);
                Destroy(collision.gameObject);
                gameController.AddScore();
                if (this.gameObject.transform.localScale.x > 0.5f)
                {
                    this.gameObject.transform.localScale -= new Vector3(1, 1, 1) * 0.5f;
                    playerSpeed += 3f;
                }
            }
            else
            {
                GameObject particleGameEnd = Instantiate(gameController.particleGameEndPrefab, this.gameObject.transform.position, Quaternion.identity);
                particleGameEnd.transform.localScale = this.gameObject.transform.localScale;
                this.gameObject.SetActive(false);
                gameController.GameEnd();// Invoke("GameEnd", 2f);
            }
        }



    }
}