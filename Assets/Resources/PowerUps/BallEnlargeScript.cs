using UnityEngine;
using static PowerUpManagerScript;

public class BallEnlargePowerUp : PowerUp
{
    public static GameObject Create(Vector3 pos, Transform parent)
    {
        GameObject newPowerUpObject = Instantiate(Resources.Load<GameObject>(
            "PowerUps/BallEnlargePrefab"), pos, Quaternion.identity, parent);
        newPowerUpObject.GetComponent<PowerUp>().Activate();
        return newPowerUpObject;
    }
    public override void Activate()
    {
        powerUpType = PowerUpType.BallEnlarge;
    }

    protected override void InternalOnTrigger(Collider2D collider)
    {
        // Check if the collider object is a ball
        var ball = collider.gameObject.GetComponent<Ball>();
        ball.GetPowerUp(this);
    }
}
