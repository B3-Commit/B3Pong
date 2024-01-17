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

    int ballSpeedAsPercent = 100;
    bool isPaused = false;
    float timeScale;

    // Start is called before the first frame update
    void Start()
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
        if (ballSpeedTextGameObj == null)
        {
            ballSpeedTextGameObj = GameObject.Find("BallSpeedText");
        }

        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            ballSpeedAsPercent = ballGameObj.GetComponent<Ball>().ChangeBallSpeed(true);

        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            ballSpeedAsPercent = ballGameObj.GetComponent<Ball>().ChangeBallSpeed(false);
        }

        // Pause functionality
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (pauseTextGameObj == null)
            {
                pauseTextGameObj = GameObject.Find("PauseText");
            }

            TogglePause();
        }

        // Toggle music and effects
        if (Input.GetKeyDown(KeyCode.M))
        {
            AudioManager.Instance.ToggleMute();
        }
    }

    private void TogglePause()
    {
        var pauseScript = pauseTextGameObj.GetComponent<ControlsTextScript>();
        var ballSpeedScript = ballSpeedTextGameObj.GetComponent<BallSpeedTextScript>();

        isPaused = !isPaused;
        if (isPaused)
        {
            // Pause game
            this.timeScale = Time.timeScale;
            Time.timeScale = 0f;
            pauseScript.ShowText();
            ballSpeedScript.ShowText();
        }
        else
        {
            // Resume game
            Time.timeScale = this.timeScale;
            pauseScript.TriggerAndFade();
            ballSpeedScript.TriggerAndFade();
        }

        PauseTriggered(isPaused);
    }

    public bool IsPaused() { return isPaused; }
}
