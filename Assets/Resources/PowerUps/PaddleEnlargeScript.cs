using System;
using UnityEngine;

public class PaddleEnlargePowerUp : PowerUp
{
    public static Action<PlayerId> PaddleEnlargePickup;

    public static GameObject Create(Vector3 pos, Transform parent)
    {
        GameObject newPowerUpObject = Instantiate(Resources.Load<GameObject>(
            "PowerUps/PaddleEnlargePrefab"), pos, Quaternion.identity, parent);
        return newPowerUpObject;
    }

    protected override void InternalOnTrigger(Ball ball)
    {
        PlayerScript player = ball.lastTouch;
        if (player != null)
        {
            PaddleEnlargePickup(player.playerId);
        }
    }
}
