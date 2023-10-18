using UnityEngine;
using static PowerUpManagerScript;

public class PaddleEnlargePowerUp : PowerUp
{
    public static GameObject Create(Vector3 pos, Transform parent)
    {
        GameObject newPowerUpObject = Instantiate(Resources.Load<GameObject>(
            "PowerUps/PaddleEnlargePrefab"), pos, Quaternion.identity, parent);
        newPowerUpObject.GetComponent<PowerUp>().Activate();
        return newPowerUpObject;
    }

    public override void Activate()
    {
        powerUpType = PowerUpType.PaddleEnlarge;
    }

    protected override void InternalOnTrigger(Collider2D collider)
    {
        // Check if the collider object is a ball
        if (collider.gameObject.TryGetComponent<Ball>(out var ball))
        {
            PlayerScript player = ball.lastTouch;
            if (player != null)
            {
                player.GetPowerUp(this);
            }
        }
    }
}
