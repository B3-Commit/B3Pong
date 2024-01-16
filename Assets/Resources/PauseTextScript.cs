using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ControlsTextScript : FadingText
{
    // Start is called before the first frame update
    override protected void Awake()
    {

        base.Awake();
        Debug.Assert(!string.IsNullOrEmpty(textMesh.text), "Controls text empty");
        base.TriggerAndFade();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
