using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;

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
    bool reloadSceneOnUnpause = false;

    void Awake()
    {
        if (Instance == null)
        {
            // This class will be recreated on scene load, so no DontDestroyOnLoad should be used
            Instance = this;

            GoalScript.goalEvent += OnGoal;
            MidlineScript.MidlineCrossed += OnMidlineCrossed;
            SettingsManagerScript.PauseTriggered += OnPauseTriggered;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
            GoalScript.goalEvent -= OnGoal;
            MidlineScript.MidlineCrossed -= OnMidlineCrossed;
            SettingsManagerScript.PauseTriggered -= OnPauseTriggered;
        }
    }

    private void OnMidlineCrossed()
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

        // Shake cameras
        StartCoroutine(Camera.main.GetComponent<ShakeAnimation>().Shake());

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
        if (SettingsManagerScript.instance.IsPaused())
        {
            reloadSceneOnUnpause = true;
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void UpdateScoreBoard()
    {
        foreach (var player in playerScores)
        {
            player.scoreText.GetComponent<TextMeshProUGUI>().text = player.score.ToString();
        }
    }

    void OnPauseTriggered(bool paused)
    {
        if (!paused && reloadSceneOnUnpause)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
