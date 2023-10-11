using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathingAnimation : MonoBehaviour
{
    public AnimationCurve expandCurve;
    public float expandAmount;
    public float expandSpeed;

    private Vector3 startSize;
    private Vector3 targetSize;
    private float scrollAmount;

    void Start()
    {
        startSize = transform.localScale;
        targetSize = startSize * expandAmount;
        // Random animation start offset.
        scrollAmount += Random.Range(0.0f, 1.0f) / expandSpeed;
    }

    void Update()
    {
        scrollAmount += Time.deltaTime * expandSpeed;
        float percent = expandCurve.Evaluate(scrollAmount);
        transform.localScale = Vector2.Lerp(startSize, targetSize, percent);
    }
}
