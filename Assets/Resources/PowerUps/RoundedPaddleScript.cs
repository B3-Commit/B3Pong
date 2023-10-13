using UnityEngine;
using static PowerUpManagerScript;

public class RoundedPaddlePowerUp : PowerUp
{
    public static GameObject Create(Vector3 pos, Transform parent)
    {
        GameObject newPowerUpObject = Instantiate(Resources.Load<GameObject>(
            "PowerUps/RoundedPaddlePrefab"), pos, Quaternion.identity, parent);
        newPowerUpObject.GetComponent<PowerUp>().Activate();
        return newPowerUpObject;
    }
    public override void Activate()
    {
        powerUpType = PowerUpType.RoundedPaddle;
    }

    protected override void InternalOnTrigger(Collider2D collider)
    {
        // Check if the collider object is a ball
        var ball = collider.gameObject.GetComponent<Ball>();

        PlayerScript player = ball.lastTouch;
        if (player != null)
        {
            player.GetPowerUp(this);
        }
    }
}