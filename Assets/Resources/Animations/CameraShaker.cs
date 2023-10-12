using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public AnimationCurve expandCurve;
    public Vector3 originalPosition;
    public float duration;
    public float magnitude;

    public IEnumerator Shake()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float curveValue = expandCurve.Evaluate(elapsedTime / duration);
            float xOffset = Random.Range(-0.5f, 0.5f) * magnitude * curveValue;
            float yOffset = Random.Range(-0.5f, 0.5f) * magnitude * curveValue;
            transform.localPosition = new Vector3(xOffset, yOffset, originalPosition.z);
            elapsedTime += Time.deltaTime;

            // Wait until next frame
            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}
