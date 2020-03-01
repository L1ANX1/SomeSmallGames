using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Text timerText;
    public Text ScoreText;
    public Text InfoText;
    [HideInInspector] public float timer = 0f;
    int score = 0;

    public float genBubbleRateMin;
    public float genBubbleRateMax;

    public static GameController _instance;

    public BubbleGenerator[] bubbleGenerators;
    int generatorCount = 0;
    bool isGameEnd = false;

    private void Awake()
    {
        _instance = this;
    }

    // Use this for initialization
    void Start()
    {
        timerText.text = "Timer:\n 0.0 s";
        ScoreText.text = "Score:\n 0";
        bubbleGenerators = GetComponentsInChildren<BubbleGenerator>();
        GameStart();
    }

    void StartGenerateBubbleLoop()
    {
        generatorCount = generatorCount % bubbleGenerators.Length;
        bubbleGenerators[generatorCount].GenerateBubble();
        generatorCount++;
    }

    void StartGenerateBubbleLoopDouble()
    {
        generatorCount = generatorCount % bubbleGenerators.Length;
        bubbleGenerators[generatorCount].GenerateBubble();
        generatorCount = bubbleGenerators.Length - generatorCount;
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
    public void GameLevelUp()
    {
        InvokeRepeating("StartGenerateBubbleLoopDouble", 0f, Random.Range(genBubbleRateMin / 2, genBubbleRateMax / 2));
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
}
