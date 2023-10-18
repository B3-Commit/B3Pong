using UnityEngine;
using static PowerUpManagerScript;

public class BallSplitPowerUp : PowerUp
{
    public static GameObject Create(Vector3 pos, Transform parent)
    {
        GameObject newPowerUpObject = Instantiate(Resources.Load<GameObject>(
            "PowerUps/BallSplitPrefab"), pos, Quaternion.identity, parent);
        newPowerUpObject.GetComponent<PowerUp>().Activate();
        return newPowerUpObject;
    }

    public override void Activate()
    {
        powerUpType = PowerUpType.BallSplit;
    }

    protected override void InternalOnTrigger(Collider2D collider)
    {
        // Check if the collider object is a ball
        if (collider.gameObject.TryGetComponent<Ball>(out var ball))
        {
            for (int i = 0; i < 3; i++)
            {
                Instantiate(Resources.Load<GameObject>(
                    "BallDecoyPrefab"), ball.transform.position, ball.transform.rotation);
            }
        }
    }
}
