using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
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

    public const float NORMAL_TIME = 1f;
    private float startFixedDeltaTime;

    [SerializeField] private AudioClip slowTimeSound;
    private AudioSource slowTimeAudioSource;

    void Awake()
    {
        GoalScript.goalEvent += OnGoal;
        MidlineScript.MidlineCrossed += EnableGoalAllowed;
        SlowMoManagerScript.SetTimeScale += SetTimeScale;

        startFixedDeltaTime = Time.fixedDeltaTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        slowTimeAudioSource = AudioManager.Instance.CreateAudioSource(slowTimeSound, true);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnDestroy()
    {
        GoalScript.goalEvent -= OnGoal;
        MidlineScript.MidlineCrossed -= EnableGoalAllowed;
        SlowMoManagerScript.SetTimeScale -= SetTimeScale;
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

    private void SetTimeScale(float timeScale)
    {
        if (Time.timeScale == NORMAL_TIME && timeScale != NORMAL_TIME)
        {
            AudioManager.Instance.SetMusicVolume(AudioManager.Instance.GetMusicVolume()/8);
            // AudioManager.Instance.SetMusicPitch(AudioManager.Instance.GetMusicPitch()/2);
            slowTimeAudioSource.Play();
        }
        else if (Time.timeScale != NORMAL_TIME && timeScale == NORMAL_TIME)
        {
            AudioManager.Instance.SetMusicVolume(AudioManager.Instance.GetMusicVolume()*8);
            // AudioManager.Instance.SetMusicPitch(AudioManager.Instance.GetMusicPitch()/2);
            slowTimeAudioSource.Stop();
        }

        Debug.Assert(0 < timeScale && timeScale <= 1);
        Time.timeScale = timeScale;
        Time.fixedDeltaTime = startFixedDeltaTime * timeScale;
    }

    void UpdateScoreBoard()
    {
        foreach (var player in playerScores)
        {
            player.scoreText.GetComponent<TextMeshProUGUI>().text = player.score.ToString();
        }
    }
}
