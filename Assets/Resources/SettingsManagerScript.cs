using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class SettingsManagerScript : MonoBehaviour
{
    public static SettingsManagerScript instance;

    public static event Action<bool> PauseTriggered;


    GameObject ballGameObj = null;
    GameObject ballSpeedTextGameObj = null;
    GameObject pauseTextGameObj = null;
    GameObject pauseText2GameObj = null;

    int ballSpeedAsPercent = 100;
    bool isPaused = false;
    float timeScale;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            // This object should not be destroyed when reloading the scene
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If an instance already exists, destroy this one
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ballGameObj == null)
        {
            ballGameObj = GameObject.Find("Ball");
            ballGameObj.GetComponent<Ball>().ChangeBallSpeed(ballSpeedAsPercent);
        }

        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            ballSpeedAsPercent = ballGameObj.GetComponent<Ball>().ChangeBallSpeed(true);

        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            ballSpeedAsPercent = ballGameObj.GetComponent<Ball>().ChangeBallSpeed(false);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }

        // Pause functionality
        if (Input.GetKeyDown(KeyCode.P))
        {

            TriggerPause(!isPaused);
        }

        // Toggle music and effects
        if (Input.GetKeyDown(KeyCode.M))
        {
            AudioManager.Instance.ToggleMute();
        }
    }

    private void ResetGame()
    {
        GameManagerScript.Instance.OnNewGameEvent();
    }

public void TriggerPause(bool pause)
    {
        if (pauseTextGameObj == null)
        {
            pauseTextGameObj = GameObject.Find("PauseText");
        }
        if (pauseText2GameObj == null)
        {
            pauseText2GameObj = GameObject.Find("PauseText2");
        }
        if (ballSpeedTextGameObj == null)
        {
            ballSpeedTextGameObj = GameObject.Find("BallSpeedText");
        }

        var pauseScript = pauseTextGameObj.GetComponent<ControlsTextScript>();
        var pauseScript2 = pauseText2GameObj.GetComponent<ControlsTextScript>();
        var ballSpeedScript = ballSpeedTextGameObj.GetComponent<BallSpeedTextScript>();

        isPaused = pause;
        if (isPaused)
        {
            // Pause game
            this.timeScale = Time.timeScale;
            Time.timeScale = 0f;
            pauseScript.ShowText();
            pauseScript2.ShowText();
            ballSpeedScript.ShowText();
        }
        else
        {
            // Resume game
            Time.timeScale = this.timeScale;
            pauseScript.TriggerAndFade();
            pauseScript2.TriggerAndFade();
            ballSpeedScript.TriggerAndFade();
        }

        PauseTriggered(isPaused);
    }

    public bool IsPaused() { return isPaused; }
}
