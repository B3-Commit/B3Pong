using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class FadingText : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public const float fadeDuration = 1.0f;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        // If textMesh is not set in the editor, use GetComponent to initialize it
        if (textMesh == null)
        {
            textMesh = GetComponent<TextMeshProUGUI>();
        }

        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ShowText()
    {
        IEnumerator function = Fade();
        StopCoroutine(function);
        SetFade(1);
    }

    public void TriggerAndFade()
    {
        IEnumerator function = Fade();

        StopCoroutine(function);
        SetFade(1f);
        StartCoroutine(function);
    }

    public IEnumerator Fade()
    {
        yield return new WaitForSeconds(2.0f);

        // Fade Out
        for (float t = 0; t <= fadeDuration; t += Time.deltaTime)
        {
            float alpha = 1 - (t / fadeDuration);
            SetFade(alpha);
            yield return null;
        }
        SetFade(0);
    }

    public void SetFade(float alpha)
    {
            textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, alpha);
    }
}
