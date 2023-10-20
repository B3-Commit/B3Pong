using UnityEngine;

public class BallSplitPowerUp : PowerUp
{
    public static GameObject Create(Vector3 pos, Transform parent)
    {
        GameObject newPowerUpObject = Instantiate(Resources.Load<GameObject>(
            "PowerUps/BallSplitPrefab"), pos, Quaternion.identity, parent);
        return newPowerUpObject;
    }

    protected override void InternalOnTrigger(Ball ball)
    {
        for (int i = 0; i < 3; i++)
        {
            Instantiate(Resources.Load<GameObject>(
                "BallDecoyPrefab"), ball.transform.position, ball.transform.rotation);
        }
    }
}
