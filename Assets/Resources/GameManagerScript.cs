using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
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

    private float startTimeScale;
    private float startFixedDeltaTime;

    [SerializeField] private AudioClip slowTimeSound;    
    private AudioSource slowTimeAudioSource;

    void Awake()
    {
        GoalScript.goalEvent += OnGoal;
        MidlineScript.MidlineCrossed += EnableGoalAllowed;
        SlowMoManagerScript.SetTimeScale += SetTimeScale;
        startTimeScale = Time.timeScale;
        startFixedDeltaTime = Time.fixedDeltaTime;

        slowTimeAudioSource = AudioManager.Instance.CreateAudioSource(slowTimeSound, true);
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
        GoalScript.goalEvent -= OnGoal;
        MidlineScript.MidlineCrossed -= EnableGoalAllowed;
        SlowMoManagerScript.SetTimeScale -= SetTimeScale;
    }

    private void EnableGoalAllowed()
    {
        isGoalAllowed = true;
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
        if (Time.timeScale == startTimeScale && timeScale != startTimeScale)
        {
            AudioManager.Instance.SetMusicVolume(AudioManager.Instance.GetMusicVolume()/8);
            // AudioManager.Instance.SetMusicPitch(AudioManager.Instance.GetMusicPitch()/2);
            slowTimeAudioSource.Play();
        }
        else if (Time.timeScale != startTimeScale && timeScale == startTimeScale)
        {
            AudioManager.Instance.SetMusicVolume(AudioManager.Instance.GetMusicVolume()*8);
            // AudioManager.Instance.SetMusicPitch(AudioManager.Instance.GetMusicPitch()/2);
            slowTimeAudioSource.Stop();
        }

        Debug.Assert(0 < timeScale && timeScale <= 1);
        Time.timeScale = startTimeScale * timeScale;
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
