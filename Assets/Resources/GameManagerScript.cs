using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public static event Action NewGameEvent;

    [System.Serializable]
    public class Player
    {
        public GameObject scoreText;
        [HideInInspector]
        public int score = 0;
    }
    public GameObject gameEndTextGameObj;

    public int maxScore = 5;
    public List<Player> playerScores;
    bool isGameResetting = false;
    bool isGoalAllowed = true;

    public float slowMotionTimeScale = 0.1f;
    public float normalTimeDelay = 0.05f;
    private float startTimeScale;
    private float startFixedDeltaTimeScale;

    // Start is called before the first frame update
    void Start()
    {
        GoalScript.GoalEvent += OnGoal;
        NewGameEvent += OnNewGameEvent;
        MidlineScript.MidlineCrossed += () => isGoalAllowed = true;
        GoalFieldScript.GoalFieldEnterEvent += OnGoalFieldEnter;
        GoalFieldScript.GoalFieldExitEvent += OnGoalFieldExit;

        startTimeScale = Time.timeScale;
        startFixedDeltaTimeScale = Time.fixedDeltaTime;
    }

    void OnGoal(int playerId)
    {
        // Do not allow goals during reset or from the same interaction
        if (isGameResetting) return;
        if (!isGoalAllowed) return;

        playerScores[playerId].score++;
        isGoalAllowed = false;

        if (playerScores[playerId].score >= maxScore)
        {
            gameEndTextGameObj.GetComponent<TextMeshProUGUI>().text = String.Format("Player {0} Won", playerId + 1);

            this.Invoke(() => NewGameEvent(), 3f);
            isGameResetting = true;
        }

        UpdateScoreBoard();
        // Wait a short moment to trigger normal time
        StartCoroutine(Utilities.WaitAndTriggerFunction(normalTimeDelay, NormalTime));
    }

    void OnNewGameEvent()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnGoalFieldEnter()
    {
        SlowTime();
    }

    void OnGoalFieldExit()
    {
        NormalTime();
    }
    
    private void SlowTime()
    {
        GetComponent<AudioSource>().Play();
        Time.timeScale = slowMotionTimeScale;
        Time.fixedDeltaTime = startFixedDeltaTimeScale * slowMotionTimeScale;
    }

    private void NormalTime()
    {
        GetComponent<AudioSource>().Stop();
        Time.timeScale = startTimeScale;
        Time.fixedDeltaTime = startFixedDeltaTimeScale;
    }

    void UpdateScoreBoard()
    {
        foreach (var player in playerScores)
        {
            player.scoreText.GetComponent<TextMeshProUGUI>().text = player.score.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
