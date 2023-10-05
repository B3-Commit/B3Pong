using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class GoalFieldScript : MonoBehaviour
{
    public static event Action GoalFieldEnterEvent;
    public static event Action GoalFieldExitEvent;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Ball")
        {
            Debug.Log("GoalFieldEnter event triggered");
            GoalFieldEnterEvent();
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Ball")
        {
            Debug.Log("GoalFieldExit event triggered");
            GoalFieldExitEvent();
        }
    }
}
