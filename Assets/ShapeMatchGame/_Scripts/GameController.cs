using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace ShapeMatchGame
{
    public class GameController : MonoBehaviour
    {
        [Header("Basic settings: ")]
        public Text timerText;
        public Text ScoreText;
        public Text InfoText;
        bool isGameEnd = false;

        [HideInInspector] public float timer = 0f;
        int score = 0;

        [Header("Bubble gen rate range:")]
        [Range(2, 5)] public float genBubbleRateMin;
        [Range(5, 7)] public float genBubbleRateMax;

        public static GameController _instance;

        BubbleGenerator[] bubbleGenerators;
        int generatorCount = 0;
        [HideInInspector] public GameManager gameManager;


        [Header("Mode change related :")]
        public SpriteRenderer playerSpriteRender;
        public Image buttonImage;               // Button info, different from mode
        public Sprite circleSprite;
        public Sprite boxSprite;

        [Header("Particles:")]
        public GameObject particleDestoryPrefab;
        public GameObject particleGameEndPrefab;

        private void Awake()
        {
            _instance = this;
        }

        void Init()
        {
            bubbleGenerators = GetComponentsInChildren<BubbleGenerator>();
            timerText.text = "Timer:\n 0.0 s";
            ScoreText.text = "Score:\n 0";
            gameManager.currentMode = Mode.circle;
            ChangeMode(Mode.circle);
        }

        // Use this for initialization
        void Start()
        {
            gameManager = new GameManager();
            Init();
            GameStart();
        }

        // Update is called once per frame
        void Update()
        {
            timer += Time.deltaTime;
            timerText.text = "Timer: \n" + timer.ToString("f1") + " s";
            if (isGameEnd && Input.GetKeyDown(KeyCode.R))
                ReplayGame();
        }

        public void GameStart()
        {
            InvokeRepeating("StartGenerateBubbleLoop", 2f, Random.Range(genBubbleRateMin, genBubbleRateMax));
        }
        void StartGenerateBubbleLoop()
        {
            generatorCount = generatorCount % bubbleGenerators.Length;
            bubbleGenerators[generatorCount].GenerateBubbles();
            generatorCount++;
        }
        public void GameLevelUp()
        {
            InvokeRepeating("StartGenerateBubbleLoopDouble", 0f, Random.Range(genBubbleRateMin / 2, genBubbleRateMax / 2));
        }
        void StartGenerateBubbleLoopDouble()
        {
            generatorCount = generatorCount % bubbleGenerators.Length;
            bubbleGenerators[generatorCount].GenerateBubbles();
            generatorCount = bubbleGenerators.Length - generatorCount;
        }

        public void AddScore()
        {
            score++;
            ScoreText.text = "Score:\n " + score.ToString();
            if (score == 5)
                GameLevelUp();
        }

        public void GameEnd()
        {
            isGameEnd = true;
            InfoText.text = "Game END\n R to Replay";
            InfoText.color = new Color(1f, 1f, 1f);
            Time.timeScale = 0.0f; // Game Pause
        }

        void ReplayGame()
        {
            SceneManager.LoadScene(0);
            Time.timeScale = 1.0f;
        }


        public void ChangeMode()
        {
            switch (gameManager.currentMode)
            {
                case Mode.circle:
                    buttonImage.sprite = circleSprite;
                    playerSpriteRender.sprite = boxSprite;
                    gameManager.currentMode = Mode.box;
                    break;
                case Mode.box:
                    buttonImage.sprite = boxSprite;
                    playerSpriteRender.sprite = circleSprite;
                    gameManager.currentMode = Mode.circle;
                    break;
                default:
                    break;
            }
        }

        public void ChangeMode(Mode m)
        {
            switch (m)
            {
                case Mode.circle:
                    buttonImage.sprite = boxSprite;
                    playerSpriteRender.sprite = circleSprite;
                    gameManager.currentMode = Mode.circle;
                    break;
                case Mode.box:
                    buttonImage.sprite = circleSprite;
                    playerSpriteRender.sprite = boxSprite;
                    gameManager.currentMode = Mode.box;
                    break;
                default:
                    break;
            }
        }
    }
}