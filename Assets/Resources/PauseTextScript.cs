using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ControlsTextScript : FadingText
{
    // Start is called before the first frame update
    override protected void Start()
    {

        base.Start();
        Debug.Assert(!string.IsNullOrEmpty(textMesh.text), "Controls text empty");
        base.TriggerAndFade();
    }

    // Update is called once per frame
    void Update()
    {
    }
}


// TODO
//    fixa pause 2 text
//    visuellt med gravitation
//    Välja dansmatta
//    Långsammare rotation
//    Mycket snabbare inbromsning