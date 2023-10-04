using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public virtual void Activate()
    {
        // Base activation code here
        Debug.Log("Base Power-Up Activated");
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        // Check if the collider object is a ball
        if (collider.gameObject.TryGetComponent<Ball>(out var ball))
        {
            GameObject player = GameObject.Find(ball.lastTouch);
            if (player != null)
            {
                player.GetComponent<PlayerScript>().GetPowerUp(this);
                Debug.Log(ball.lastTouch + " triggered a power up!");
            }
        }
        else
        {
            Debug.Assert(false);
        }
        // Destroy powerup
        Destroy(gameObject);
    }
}

