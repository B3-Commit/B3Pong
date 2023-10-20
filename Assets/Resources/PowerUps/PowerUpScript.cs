using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collider)
    {
        // Check if the collider object is a ball
        if (collider.gameObject.TryGetComponent<Ball>(out var ball))
        {
            InternalOnTrigger(ball);
            Destroy(gameObject);
        }
        else if (collider.gameObject.TryGetComponent<BallDecoy>(out var decoy)) {
            // Do nothing
        }
        else
        {
            // Nothing else is supposed to be able to trigger the power up
            Debug.Assert(false);
        }
    }

    protected abstract void InternalOnTrigger(Ball ball);

}
