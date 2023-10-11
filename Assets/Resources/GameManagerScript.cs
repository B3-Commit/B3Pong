using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    // Make this class static
    public static GameManagerScript Instance;

    [System.Serializable]
    public class PlayerScore
    {
        public GameObject scoreText;
        [HideInInspector]
        public int score = 0;
    }
    public GameObject gameEndTextGameObj;

    public int maxScore = 5;
    public List<PlayerScore> playerScores;
    bool isGameResetting = false;
    bool isGoalAllowed = true;

    float startFixedDeltaTime;
    float timeScale;

    void Awake()
    {
        if (Instance == null)
        {
            // This class will be recreated on scene load, so no DontDestroyOnLoad should be used
            Instance = this;
            
            GoalScript.goalEvent += OnGoal;
            MidlineScript.MidlineCrossed += EnableGoalAllowed;
            SlowMoManagerScript.SetTimeScale += SetTimeScale;

            startFixedDeltaTime = Time.fixedDeltaTime;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
            GoalScript.goalEvent -= OnGoal;
            MidlineScript.MidlineCrossed -= EnableGoalAllowed;
            SlowMoManagerScript.SetTimeScale -= SetTimeScale;
        }
    }

    private void EnableGoalAllowed()
    {
        isGoalAllowed = true;
    }

    void OnGoal(PlayerScript player)
    {
        // Do not allow goals during reset or from the same interaction
        if (isGameResetting) return;
        if (!isGoalAllowed) return;

        playerScores[player.playerId].score++;
        isGoalAllowed = false;

        // Shake camera
        StartCoroutine(Camera.main.GetComponent<CameraShake>().Shake());

        if (playerScores[player.playerId].score >= maxScore)
        {
            gameEndTextGameObj.GetComponent<TextMeshProUGUI>().text = String.Format("{0} won", player.playerName);

            this.Invoke(() => OnNewGameEvent(), 3f);
            isGameResetting = true;
        }

        UpdateScoreBoard();
    }

    void OnNewGameEvent()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void SetTimeScale(float newTimeScale)
    {

        Debug.Assert(0 < newTimeScale && newTimeScale <= 1);
        Time.timeScale = newTimeScale;
        Time.fixedDeltaTime = startFixedDeltaTime * newTimeScale;
        this.timeScale = newTimeScale;
    }

    void UpdateScoreBoard()
    {
        foreach (var player in playerScores)
        {
            player.scoreText.GetComponent<TextMeshProUGUI>().text = player.score.ToString();
        }
    }

    public bool IsGoalAllowed() { return isGoalAllowed; }
}
