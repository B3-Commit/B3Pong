using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManagerScript : MonoBehaviour
{
    public const float Y_RANGE = 400;
    public const float X_RANGE_MAX = 450;
    public const float TIME_INTERVAL = 5f;

    public enum PowerUpType
    {
        PaddleEnlarge = 0,
        BallEnlarge,
        RoundedPaddle,
    }

    List<int> typeProbabilityWeights = new () { 10, 10, 10 }; // TODO restore
    List<PowerUpType> weightedTypeList = new ();

    // Start is called before the first frame update
    void Start()
    {
        // Populate the weighted list based on the enum values and corresponding weights
        for (int i = 0; i < typeProbabilityWeights.Count; i++)
        {
            PowerUpType type = (PowerUpType)i;
            for (int j = 0; j < typeProbabilityWeights[i]; j++)
            {
                weightedTypeList.Add(type);
            }
        }

        // Trigger a timer for power-ups
        StartCoroutine(CreatePowerUp());
    }

    IEnumerator CreatePowerUp()
    {
        while (true)
        {
            yield return new WaitForSeconds(TIME_INTERVAL);

            switch (GetPowerUpType())
            {
                case PowerUpType.PaddleEnlarge:
                    PaddleEnlargePowerUp.Create(GetPosition(), transform);
                    break;
                case PowerUpType.BallEnlarge:
                    BallEnlargePowerUp.Create(GetPosition(), transform);
                    break;
                case PowerUpType.RoundedPaddle:
                    RoundedPaddlePowerUp.Create(GetPosition(), transform);
                    break;
                default:
                    Debug.LogError("Unknown power up type");
                    break;
            }
        }
    }
    Vector3 GetPosition()
    {
        // Get a centerboard position, but not too close to center line
        float randY = 0.01f * Random.Range(-Y_RANGE, Y_RANGE);
        float randX = 0.01f * Random.Range(-X_RANGE_MAX, X_RANGE_MAX);

        return new Vector3(randX, randY, 0);
    }

    PowerUpType GetPowerUpType()
    {
        int randomIndex = Random.Range(0, weightedTypeList.Count);
        return weightedTypeList[randomIndex];
    }

    // Update is called once per frame
    void Update()
    {

    }
}
