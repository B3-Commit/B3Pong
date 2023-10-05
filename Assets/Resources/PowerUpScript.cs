using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PowerUp : MonoBehaviour
{

    public static GameObject Create(Vector3 pos, Transform parent)
    {
        return Instantiate(Resources.Load<GameObject>("PaddleEnlargePrefab"), pos, Quaternion.identity, parent);
    }
    public virtual void Activate()
    {
        // Base activation code here
        Debug.Log("Paddle enlarge Power-Up Activated");
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

