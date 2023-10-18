using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    public PowerUpManagerScript.PowerUpType powerUpType;

    public abstract void Activate();

    public void OnTriggerEnter2D(Collider2D collider)
    {
        // Check if the collider object is a ball
        if (collider.gameObject.TryGetComponent<Ball>(out var ball))
        {
            InternalOnTrigger(collider);
            Destroy(gameObject);
        }
        else
        {
            // Nothing else is supposed to be able to trigger the power up
            Debug.Assert(false);
        }
    }

    protected abstract void InternalOnTrigger(Collider2D collider);

}
