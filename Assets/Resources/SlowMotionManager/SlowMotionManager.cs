using System.Collections.Generic;
using UnityEngine;

public class SlowMotionManager : MonoBehaviour
{
    public Ball ball;
    public List<SlowMotionField> slowMotionFields;

    private float startFixedDeltaTime;

    private bool slowMotionActive = false;

    private bool enable = true;

    void Awake()
    {
        startFixedDeltaTime = Time.fixedDeltaTime;

        GoalScript.goalEvent += OnGoal;
        MidlineScript.MidlineCrossed += OnMidlineCrossed;
    }

    private void OnDestroy()
    {
        GoalScript.goalEvent -= OnGoal;
        MidlineScript.MidlineCrossed -= OnMidlineCrossed;
    }

    private void Update()
    {
        if(!enable)
        {
            return;
        }

        var isInsideField = false;
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

        // Slow motion was active but we're no longer inside a field
        if (slowMotionActive && !isInsideField)
        {
            RestoreTime();
        }

        slowMotionActive = isInsideField;
    }

    private void OnMidlineCrossed()
    {
        enable = true;
    }

    private void OnGoal(PlayerScript script)
    {
        enable = false;
        if (slowMotionActive)
        {
            RestoreTime();
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
}
