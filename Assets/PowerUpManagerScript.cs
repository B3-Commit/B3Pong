using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManagerScript : MonoBehaviour
{
    public GameObject powerUpPrefab;

    // Start is called before the first frame update
    void Start()
    {
        // Trigger a timer for power-ups
        StartCoroutine(CreatePowerUp());
    }

    IEnumerator CreatePowerUp()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            GameObject newPowerUp = Instantiate(powerUpPrefab, GetPosition(), Quaternion.identity);
            PowerUp powerUpComponent = newPowerUp.GetComponent<PowerUp>();
            powerUpComponent.Activate();
        }
    }
    Vector3 GetPosition()
    {
        // Get a centerboard position, but not too close to center line
        float randY = 0.01f * Random.Range(-450, 450);
        float randX = 0.01f * Random.Range(100, 450);
        if (Random.Range(0, 2) == 0) randX *= -1;

        return new Vector3(randX, randY, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
