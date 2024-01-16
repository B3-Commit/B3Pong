using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlowMotionManager : MonoBehaviour
{
    public Ball ball;
    public List<SlowMotionField> slowMotionFields;

    public static Action<bool> SlowMotionActive;

    private float startFixedDeltaTime;

    private bool slowMotionWasActive = false;

    private bool enable = true;
    private float timeScale;

    void Awake()
    {
        startFixedDeltaTime = Time.fixedDeltaTime;

        GoalScript.goalEvent += OnGoal;
        MidlineScript.MidlineCrossed += OnMidlineCrossed;
        SettingsManagerScript.PauseTriggered += OnPauseTriggered;
    }

    private void OnDestroy()
    {
        GoalScript.goalEvent -= OnGoal;
        MidlineScript.MidlineCrossed -= OnMidlineCrossed;
        SettingsManagerScript.PauseTriggered -= OnPauseTriggered;
    }

    private void Update()
    {
        if(!enable)
        {
            return;
        }

        var isInsideField = false;

        // Check if we're inside a field and set the new time scale
        foreach (var field in slowMotionFields)
        {
            if (field.IsPointInsideField(ball.transform.position))
            {
                isInsideField = true;
                var curveValue = field.GetCurveValueForPoint(ball.transform.position.x);
                SetTime(curveValue);
                break;
            }
        }

        // If we're inside a filed and slow motion wasn't active before
        if (isInsideField && !slowMotionWasActive)
        {
            SlowMotionActive(true);
        }
        // If we're not inside a field and slow motion was active before
        else if (!isInsideField && slowMotionWasActive)
        {
            RestoreTime();
            SlowMotionActive(false);
        }

        slowMotionWasActive = isInsideField;
    }

    private void OnMidlineCrossed()
    {
        enable = true;
    }

    private void OnGoal(PlayerScript script)
    {
        enable = false;
        if (slowMotionWasActive)
        {
            RestoreTime();
            SlowMotionActive(false);
        }
    }

    private void SetTime(float timeScale)
    {
        Debug.Assert(0 < timeScale && timeScale <= 1);
        Time.timeScale = timeScale;
        Time.fixedDeltaTime = startFixedDeltaTime * timeScale;
        AudioManager.Instance.SetMusicSpeed(timeScale);
        AudioManager.Instance.StartHeartBeat();
    }

    private void RestoreTime()
    {
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = startFixedDeltaTime;
        AudioManager.Instance.SetMusicSpeed(1.0f);
        AudioManager.Instance.StopHeartBeat();
    }

    private void OnPauseTriggered(bool pause)
    {
        enable = !pause;
        if (pause)
        {
            this.timeScale = Time.timeScale;
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }
}
