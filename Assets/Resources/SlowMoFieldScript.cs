using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class SlowMoFieldScript : MonoBehaviour
{
    public static event Action SlowMoFieldEnterEvent;
    public static event Action SlowMoFieldExitEvent;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Ball")
        {
            SlowMoFieldEnterEvent();
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Ball")
        {
            SlowMoFieldExitEvent();
        }
    }
}
