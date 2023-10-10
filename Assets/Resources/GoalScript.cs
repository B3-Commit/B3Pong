using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System;
using UnityEngine.SceneManagement;

public class GoalScript : MonoBehaviour
{
    public PlayerScript player;
    public static event Action<PlayerScript> goalEvent;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Ball>(out var ball))
        {
            goalEvent(player);
        }
    }
}
