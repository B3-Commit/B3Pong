using System;
using UnityEngine;

public class GravityPowerUp : PowerUp
{
    public static Action GravityPickup;

    public static GameObject Create(Vector3 pos, Transform parent)
    {
        GameObject newPowerUpObject = Instantiate(Resources.Load<GameObject>(
            "PowerUps/GravityPowerUpPrefab"), pos, Quaternion.identity, parent);
        return newPowerUpObject;
    }

    protected override void InternalOnTrigger(Ball ball)
    {
        GravityPickup();
    }
}
