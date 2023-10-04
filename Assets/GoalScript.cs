using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System;

public class GoalScript : MonoBehaviour
{
    public int playerId;
    public static event Action<int> GoalEvent;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Ball>(out var ball))
        {
            GoalEvent(playerId);
        }
    }
}
