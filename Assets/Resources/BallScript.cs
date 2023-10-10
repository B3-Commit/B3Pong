using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public PlayerScript lastTouch;
    public float currentMinimumSpeed;
    public float currentMaximumSpeed;
    public GameObject ballSpeedText;

    public const float DEFAULT_MINIMUM_SPEED = 4.5f;
    public const float DEFAULT_MAXIMUM_SPEED = 20.0f;
    public const float POWER_UP_SIZE_INCR = 2f;
    public const float TIME_SIZE_DECR = 2e-3f;
    public const float DEFAULT_SIZE = 4.0f;

    private int speedAsPercent = 100;

    [SerializeField] private AudioClip bounceSound;

    void Start()
    {
        var rigidBody = GetComponent<Rigidbody2D>();

        currentMinimumSpeed = DEFAULT_MINIMUM_SPEED * speedAsPercent / 100;
        currentMaximumSpeed = DEFAULT_MAXIMUM_SPEED * speedAsPercent / 100;

        float x = Random.Range(0, 2) == 0 ? -currentMinimumSpeed : currentMinimumSpeed;
        float y = 0.01f * (float)Random.Range(0, 100 * currentMinimumSpeed);

        rigidBody.velocity = new Vector2(x, y);
    }

    void Update()
    {
        var rigidBody = GetComponent<Rigidbody2D>();
        if (transform.localScale.y > DEFAULT_SIZE)
        {
            // Shrink back towards normal size
            transform.localScale -= new Vector3(TIME_SIZE_DECR, TIME_SIZE_DECR, 0);
        }


        // To keep the flow going, there needs to be a minimum x velocity
        if (System.Math.Abs(rigidBody.velocity.x) < currentMinimumSpeed)
        {
            float newX =  rigidBody.velocity.x < 0 ? -currentMinimumSpeed : currentMinimumSpeed;
            rigidBody.velocity = new Vector2(newX, rigidBody.velocity.y);
        }

        var speed = rigidBody.velocity.magnitude;
        if (speed < currentMinimumSpeed)
        {
            rigidBody.velocity = rigidBody.velocity / speed * currentMinimumSpeed;
        } else if (speed > currentMaximumSpeed)
        {
            rigidBody.velocity = rigidBody.velocity / speed * currentMaximumSpeed;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        AudioManager.Instance.PlayEffect(bounceSound);
        var particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Play();

        // Save last touch
        if (collision.gameObject.TryGetComponent<PlayerScript>(out PlayerScript player))
        {
            lastTouch = player;
        }
    }

    // If argument increase is false, it is a decrease
    public int ChangeBallSpeed(bool increase)
    {
        return ChangeBallSpeed(speedAsPercent + (increase ? 10 : -10));
    }

    public int ChangeBallSpeed(int newSpeedAsPercent)
    {
        newSpeedAsPercent = Mathf.Clamp(newSpeedAsPercent, 10, 500);
        InternalChangeBallSpeed(newSpeedAsPercent);
        return speedAsPercent;
    }

    private void InternalChangeBallSpeed(int newSpeedAsPercent)
    {
        float speedChange = 1.0f * newSpeedAsPercent / speedAsPercent;
        speedAsPercent = newSpeedAsPercent;

        // Adjust current speed
        var rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.velocity *= speedChange;
        ballSpeedText.GetComponent< BallSpeedTextScript>().UpdateText(speedAsPercent);

        // Adjust min and max
        currentMinimumSpeed = DEFAULT_MINIMUM_SPEED * speedAsPercent / 100;
        currentMaximumSpeed = DEFAULT_MAXIMUM_SPEED * speedAsPercent / 100;
    }

    public void GetPowerUp(PowerUp powerUp)
    {
        if (powerUp.powerUpType == PowerUpManagerScript.PowerUpType.BallEnlarge)
        {
            transform.localScale += new Vector3(POWER_UP_SIZE_INCR, POWER_UP_SIZE_INCR, 0);
        }
        else
        {
            Debug.Assert(false, "Unknown power up hit ball");
        }
    }
}
