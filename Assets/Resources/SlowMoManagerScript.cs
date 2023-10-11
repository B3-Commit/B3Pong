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

    public const float DEFAULT_SPEED = 1f;

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
        // This counter will be accurate at all times regardless of if goals have been scored
        numberOfActiveZones--;
        Debug.Assert(numberOfActiveZones >= 0);

        SendUpdate();
    }

    void SendUpdate()
    {
        if (GameManagerScript.Instance.IsGoalAllowed())
        {
            // If no goal has just recently been scored, send update based on number of active zones.
            float newSpeed = slowMotionTimeScale[numberOfActiveZones];
            SetTimeScale(newSpeed);
        }
        else
        {
            // We should not send any normal update if a goal has recently been scored,
            // as indicated by IsGoalAllowed(). Instead, the speed should always be restored.
            SetTimeScale(DEFAULT_SPEED);
        }
    }
}
