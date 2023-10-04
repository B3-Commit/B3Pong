using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManagerScript : MonoBehaviour
{
    [System.Serializable]
    public class Player
    {
        public GameObject scoreText;
        [HideInInspector]
        public int score = 0;
    }

    [SerializeField] public List<Player> playerScores;
    // Start is called before the first frame update
    void Start()
    {
        GoalScript.GoalEvent += onGoal;
    }

    void onGoal(int playerId)
    {
        playerScores[playerId].score++;
        updateScoreBoard();
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
