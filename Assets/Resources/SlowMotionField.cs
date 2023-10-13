using System.Collections;
using UnityEngine;

public class SlowMotionField : MonoBehaviour
{
    public AnimationCurve speedCurve;
    public Ball ball;

    private float fieldMinX;
    private float fieldMaxX;
    private float startFixedDeltaTime;
    private IEnumerator updateTimeCoroutine;

    void Start()
    {
        startFixedDeltaTime = Time.fixedDeltaTime;
        fieldMinX = transform.position.x - transform.localScale.x / 2;
        fieldMaxX = transform.position.x + transform.localScale.x / 2;
        updateTimeCoroutine = UpdateTime();
    }

    private IEnumerator UpdateTime()
    {
        while(true)
        {
            var ballCenterX = ball.transform.position.x;
            if (ballCenterX < fieldMinX || ballCenterX > fieldMaxX) { yield return null; }

            var percentX = Mathf.Clamp01((ballCenterX - fieldMinX) / (fieldMaxX - fieldMinX));
            var timeScale = speedCurve.Evaluate(percentX);
            SetTimeScale(timeScale);

            yield return null;
        }
    }

    private void SetTimeScale(float newTimeScale)
    {
        Debug.Assert(0 < newTimeScale && newTimeScale <= 1);
        Time.timeScale = newTimeScale;
        Time.fixedDeltaTime = startFixedDeltaTime * newTimeScale;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Ball")
        {
            AudioManager.Instance.SetMusicSpeed(0.3f);
            StartCoroutine(updateTimeCoroutine);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Ball")
        {
            StopCoroutine(updateTimeCoroutine);
            AudioManager.Instance.SetMusicSpeed(1f);
            SetTimeScale(1.0f);
        }
    }
}

