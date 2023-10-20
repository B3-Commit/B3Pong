using System;
using UnityEngine;

public class BallEnlargePowerUp : PowerUp
{
    public static Action BallEnlargePickup;

    public static GameObject Create(Vector3 pos, Transform parent)
    {
        GameObject newPowerUpObject = Instantiate(Resources.Load<GameObject>(
            "PowerUps/BallEnlargePrefab"), pos, Quaternion.identity, parent);
        return newPowerUpObject;
    }

    protected override void InternalOnTrigger(Ball ball)
    {
        BallEnlargePickup();
    }
}
