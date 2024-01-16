using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BallSpeedTextScript : FadingText
{
    

    // Start is called before the first frame update
    override protected void Awake()
    {
        base.Awake();
        base.TriggerAndFade();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UpdateText(int speed)
    {
        GetComponent<TextMeshProUGUI>().text =
            String.Format("Ball speed: {0} %", speed);
        base.TriggerAndFade();
    }
}
