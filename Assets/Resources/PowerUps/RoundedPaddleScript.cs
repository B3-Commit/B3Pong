using System;
using UnityEngine;

public class RoundedPaddlePowerUp : PowerUp
{
    public static Action<int> RoundedPaddlePickup;

    public static GameObject Create(Vector3 pos, Transform parent)
    {
        GameObject newPowerUpObject = Instantiate(Resources.Load<GameObject>(
            "PowerUps/RoundedPaddlePrefab"), pos, Quaternion.identity, parent);
        return newPowerUpObject;
    }

    protected override void InternalOnTrigger(Ball ball)
    {
        PlayerScript player = ball.lastTouch;
        if (player != null)
        {
            RoundedPaddlePickup(player.playerId);
        }
    }
}
