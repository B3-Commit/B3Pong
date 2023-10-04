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

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Base Power-Up collision");
        // Check if the collision object is a ball
        if (collision.gameObject.TryGetComponent<Ball>(out var ball))
        {
            Debug.Log("Base Power-Up is ball");

            // Get ball x velocity to determine the player to get the powerup
            float xVelocity = ball.GetComponent<Rigidbody2D>().velocity.x;
            GameObject player;
            if (xVelocity > 0)
            {
                Debug.Log("Base Power-Up Left");
                player = GameObject.Find("Player Left");
            } else {
                Debug.Log("Base Power-Up Right");
                player = GameObject.Find("Player Right");
            }

            //player.
        }
        Debug.Log("Base Power-Up end");

    }

    public void OnTriggerEnter2D(Collider2D collider)
    {

        // Check if the collider object is a ball
        if (collider.gameObject.TryGetComponent<Ball>(out var ball))
        {

            // Get ball x velocity to determine the player to get the powerup
            float xVelocity = ball.GetComponent<Rigidbody2D>().velocity.x;
            GameObject player;
            if (xVelocity > 0)
            {
                player = GameObject.Find("Player Left");
            }
            else
            {
                player = GameObject.Find("Player Right");
            }

            player.GetComponent<PlayerScript>().GetPowerUp(this);
        } else
        {
            Debug.Assert(false);
        }

        Destroy(gameObject);
    }
}

