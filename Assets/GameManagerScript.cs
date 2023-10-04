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

    // Start is called before the first frame update
    void Start()
    {
        GoalScript.GoalEvent += onGoal;
        NewGameEvent += onNewGameEvent;
        MidlineScript.MidlineCrossed += () => isGoalAllowed = true;
    }

    void onGoal(int playerId)
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

        updateScoreBoard();
    }

    void onNewGameEvent()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void updateScoreBoard()
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
