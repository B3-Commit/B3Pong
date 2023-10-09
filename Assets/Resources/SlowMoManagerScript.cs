using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlowMoManagerScript : MonoBehaviour
{
    public static event Action<float> SetTimeScale;

    public List<float> slowMotionTimeScale = new() {0, 0, 0};
    public float normalTimeDelay = 0.02f;

    int numberOfActiveZones;

    void Awake()
    {
        SlowMoFieldScript.SlowMoFieldEnterEvent += OnSlowMoFieldEnter;
        SlowMoFieldScript.SlowMoFieldExitEvent += OnSlowMoFieldExit;
    }

    // Start is called before the first frame update
    void Start()
    {

        // Add a zeroth element for when no slow mo zones are active.
        slowMotionTimeScale.Insert(0, 1f);
        numberOfActiveZones = 0;
        SendUpdate();
    }

    private void OnDestroy()
    {
        SlowMoFieldScript.SlowMoFieldEnterEvent -= OnSlowMoFieldEnter;
        SlowMoFieldScript.SlowMoFieldExitEvent -= OnSlowMoFieldExit;
    }

    void OnSlowMoFieldEnter()
    {
        numberOfActiveZones++;
        Debug.Assert(numberOfActiveZones <= slowMotionTimeScale.Count);

        SendUpdate();
    }

    void OnSlowMoFieldExit()
    {
        if (numberOfActiveZones == 0) return;
        if (numberOfActiveZones == 3)
        {
            // Assume a goal has been scored
            numberOfActiveZones = 0;
        }
        else
        {
            numberOfActiveZones--;
        }

        SendUpdate();
    }

    void SendUpdate()
    {
        float newSpeed = slowMotionTimeScale[numberOfActiveZones];
        SetTimeScale(newSpeed);
    }
}
