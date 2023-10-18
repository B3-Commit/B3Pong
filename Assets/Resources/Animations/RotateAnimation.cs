using UnityEngine;

public class RotateAnimation : MonoBehaviour
{
    public float rotationSpeed;

    void Update()
    {
        transform.Rotate(0, 0, Time.deltaTime * rotationSpeed);
    }
}
