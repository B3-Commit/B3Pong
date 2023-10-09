using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SettingsManagerScript : MonoBehaviour
{
    public static SettingsManagerScript instance;

    GameObject ballGameObj = null;
    GameObject ballSpeedTextGameObj = null;

    int ballSpeedAsPercent = 100;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            // This object should not be destroyed when reloading the scene
            instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If an instance already exists, destroy this one
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ballGameObj == null)
        {
            ballGameObj = GameObject.Find("Ball");
            ballGameObj.GetComponent<Ball>().ChangeBallSpeed(ballSpeedAsPercent);
        }
        if (ballSpeedTextGameObj == null)
        {
            ballSpeedTextGameObj = GameObject.Find("BallSpeedText");
        }

        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            ballSpeedAsPercent = ballGameObj.GetComponent<Ball>().ChangeBallSpeed(true);

        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            ballSpeedAsPercent = ballGameObj.GetComponent<Ball>().ChangeBallSpeed(false);
        }


    }
}
