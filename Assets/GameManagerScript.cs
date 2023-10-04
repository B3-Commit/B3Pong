using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public static event Action NewGameEvent;
    //public static event Action<int> GameFinished;

    [System.Serializable]
    public class Player
    {
        public GameObject scoreText;
        [HideInInspector]
        public int score = 0;
    }

    public int maxScore = 5;
    public List<Player> playerScores;
    bool isGameResetting = false;

    // Start is called before the first frame update
    void Start()
    {
        GoalScript.GoalEvent += onGoal;
        NewGameEvent += onNewGameEvent;
    }

    void onGoal(int playerId)
    {
        if(isGameResetting)
        {
            return;
        }

        playerScores[playerId].score++;

        if (playerScores[playerId].score >= maxScore)
        {   
            // TODO display winner
            Debug.Log(String.Format("Player {0} won!", playerId + 1));
            //GameFinished(playerId);

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
