using UnityEngine;
using static PowerUpManagerScript;

public class PaddleEnlargePowerUp : PowerUp
{
    public static GameObject Create(Vector3 pos, Transform parent)
    {
        return Instantiate(Resources.Load<GameObject>(
            "PaddleEnlargePrefab"), pos, Quaternion.identity, parent);
    }
    public override void Activate()
    {
        powerUpType = PowerUpType.PaddleEnlarge;
    }

    protected override void InternalOnTrigger(Collider2D collider)
    {
        // Check if the collider object is a ball
        var ball = collider.gameObject.GetComponent<Ball>();

        GameObject player = GameObject.Find(ball.lastTouch);
        if (player != null)
        {
            player.GetComponent<PlayerScript>().GetPowerUp(this);
            Debug.Log(ball.lastTouch + " got larger paddle!");
        }
    }
}

