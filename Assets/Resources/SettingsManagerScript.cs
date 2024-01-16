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
    GameObject countDownTextGameObj = null;

    int ballSpeedAsPercent = 100;
    bool isPaused = false;
    private Coroutine startedCountdown;

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

        // Pause functionality
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isPaused)
        {
            UnpauseWithCountdown();
        }

        // Toggle music and effects
        if (Input.GetKeyDown(KeyCode.M))
        {
            AudioManager.Instance.SetMasterVolume(0f);
        }
    }

    public void UnpauseWithCountdown()
    {
        if (startedCountdown != null)
        {
            StopCoroutine(startedCountdown);
        }
        startedCountdown = StartCoroutine(CountdownToUnpause());
    }

    public IEnumerator CountdownToUnpause()
    {
        float elapsedTime = 0f;
        float duration = 3f;

        if (countDownTextGameObj == null)
        {
            countDownTextGameObj = GameObject.Find("CountdownText");
        }
        countDownTextGameObj.GetComponent<TextMeshProUGUI>().enabled = true;

        while (elapsedTime < duration && SettingsManagerScript.instance.IsPaused())
        {
            countDownTextGameObj.GetComponent<TextMeshProUGUI>().text = ((int)Math.Ceiling(duration - elapsedTime)).ToShortString(1);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        countDownTextGameObj.GetComponent<TextMeshProUGUI>().enabled = false;
        countDownTextGameObj.GetComponent<TextMeshProUGUI>().text = "";
        startedCountdown = null;

        if (SettingsManagerScript.instance.IsPaused())
        { 
            TriggerPause(false);
        }
    }

    private void ResetGame()
    {
        GameManagerScript.Instance.OnNewGameEvent();
    }

    private void TogglePause()
    {
        TriggerPause(!isPaused);
    }

    public void TriggerPause(bool pause)
    {
        if (pause == isPaused) return;

        if (pauseTextGameObj == null)
        {
            pauseTextGameObj = GameObject.Find("PauseText");
        }

        if (ballSpeedTextGameObj == null)
        {
            ballSpeedTextGameObj = GameObject.Find("BallSpeedText");
        }

        var pauseScript = pauseTextGameObj.GetComponent<ControlsTextScript>();
        var ballSpeedScript = ballSpeedTextGameObj.GetComponent<BallSpeedTextScript>();

        isPaused = pause;
        if (isPaused)
        {
            // Pause game
            pauseScript.ShowText();
            ballSpeedScript.ShowText();
        }
        else
        {
            // Resume game
            pauseScript.TriggerAndFade();
            ballSpeedScript.TriggerAndFade();
        }

        PauseTriggered(isPaused);
    }

    public bool IsPaused() { return isPaused; }
}
