using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManagerScript : MonoBehaviour
{
    public GameObject powerUpPrefab;
    public const float Y_RANGE = 400;
    public const float X_RANGE_MIN = 100;
    public const float X_RANGE_MAX = 400;
    public const float TIME_INTERVAL = 5f;

    // Start is called before the first frame update
    void Start()
    {
        // Trigger a timer for power-ups
        StartCoroutine(CreatePowerUp());
        GameManagerScript.NewGameEvent += onNewGameEvent;
    }

    void onNewGameEvent()
    {
        foreach (Transform child in gameObject.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    IEnumerator CreatePowerUp()
    {
        while (true)
        {
            yield return new WaitForSeconds(TIME_INTERVAL);
            GameObject newPowerUp = Instantiate(powerUpPrefab, GetPosition(), Quaternion.identity, gameObject.transform);
            PowerUp powerUpComponent = newPowerUp.GetComponent<PowerUp>();
            powerUpComponent.Activate();
        }
    }
    Vector3 GetPosition()
    {
        // Get a centerboard position, but not too close to center line
        float randY = 0.01f * Random.Range(-Y_RANGE, Y_RANGE);
        float randX = 0.01f * Random.Range(X_RANGE_MIN, X_RANGE_MAX);
        if (Random.Range(0, 2) == 0) randX *= -1;

        return new Vector3(randX, randY, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
