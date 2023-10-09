using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BallSpeedTextScript : MonoBehaviour
{
    public TextMeshProUGUI textMesh;

    // Start is called before the first frame update
    void Start()
    {
        // If textMesh is not set in the editor, use GetComponent to initialize it
        if (textMesh == null)
        {
            textMesh = GetComponent<TextMeshProUGUI>();
        }

        gameObject.SetActive(true);
        StopCoroutine("SlowFade");
        StartCoroutine("SlowFade");
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Update is called once per frame
    public void UpdateText(int speed)
    {
        GetComponent<TextMeshProUGUI>().text =
            String.Format("Ball speed: {0} %", speed);
        StopCoroutine("SlowFade");
        StartCoroutine("SlowFade");

    }

    IEnumerator SlowFade()
    {
        SetFade(1f);
        yield return new WaitForSeconds(2.0f);

        float duration = 1.0f;
        // Fade Out
        for (float t = 0; t <= duration; t += Time.deltaTime)
        {
            float alpha = 1 - (t / duration);
            SetFade(alpha);
            yield return null;
        }
        SetFade(0);
    }

    void SetFade(float alpha)
    {
            textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, alpha);
    }
}
