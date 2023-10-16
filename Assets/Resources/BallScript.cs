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
    public const float BALL_SIZE_RESTORE_TIME = 4f; // seconds
    public const float DEFAULT_SIZE = 4.0f;
    public const float MINIMUM_ENERGY = 40;

    private int speedAsPercent = 100;
    private float defaultMass;
    private bool useGravity = false;
    private bool m_accelerationEnabled = false;

    [SerializeField] private AudioClip bounceSound;

    void Start()
    {
        var rigidBody = GetComponent<Rigidbody2D>();

        currentMinimumSpeed = DEFAULT_MINIMUM_SPEED * speedAsPercent / 100;
        currentMaximumSpeed = DEFAULT_MAXIMUM_SPEED * speedAsPercent / 100;

        float x = Random.Range(0, 2) == 0 ? -currentMinimumSpeed : currentMinimumSpeed;
        float y = 0.01f * (float)Random.Range(0, 100 * currentMinimumSpeed);

        rigidBody.velocity = new Vector2(x, y);
        defaultMass = rigidBody.mass;
        ToggleGravity(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            ToggleGravity(!useGravity);
        }

        var rigidBody = GetComponent<Rigidbody2D>();
        if (transform.localScale.y > DEFAULT_SIZE)
        {
            // Shrink back towards normal size
            float shrinkage = POWER_UP_SIZE_INCR * Time.deltaTime / BALL_SIZE_RESTORE_TIME;
            transform.localScale -= new Vector3(shrinkage, shrinkage, 0);
        }

        // To avoid energy loss, there needs to be a minimum velocity
        float timeScaler = 2f;
        var speed = rigidBody.velocity.magnitude;
        if (!useGravity && m_accelerationEnabled && speed < currentMinimumSpeed)
        {
            rigidBody.velocity += rigidBody.velocity * Time.deltaTime * timeScaler;
        }

        // There also needs to be a minimum x-velocity for the sake of the gameplay
        if (m_accelerationEnabled && System.Math.Abs(rigidBody.velocity.x) < currentMinimumSpeed / 2.0f)
        {
            var speedDelta = Mathf.Sign(rigidBody.velocity.x) * Time.deltaTime * timeScaler;
            rigidBody.velocity = new Vector2(
                rigidBody.velocity.x + speedDelta,
                rigidBody.velocity.y);
        }

        if (useGravity)
        {
            // If the energy of the ball is too low, increase the velocity along y axis.
            // This is to increase bounce, not just speed. Use the default mass to not
            // reduce movement if the ball is bigger. E = mv^2/2 + mgh
            var heightFromFloor = transform.position.y + 5; // Floor = -5
            float energy = defaultMass * (speed * speed / 2 + Physics2D.gravity.magnitude * heightFromFloor);
            if (energy < MINIMUM_ENERGY)
            {
                float deltaY = Mathf.Sign(rigidBody.velocity.y) * rigidBody.velocity.magnitude * Time.deltaTime * timeScaler;
                rigidBody.velocity += new Vector2(0, deltaY);
            }

            // Do we need to handle a maximum energy case?
        }
        else if (speed > currentMaximumSpeed)
        {
            rigidBody.velocity *= currentMaximumSpeed / speed;
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
        else if (powerUp.powerUpType == PowerUpManagerScript.PowerUpType.Gravity)
        {
            // If gravity is enabled, disable it
            // Otherwise, enable it and schedule its disabling.
            StopCoroutine(DisableGravityAfterDelay());
            
            if (useGravity)
            {
                ToggleGravity(false);
            }
            else
            {
                ToggleGravity(true);
                StartCoroutine(DisableGravityAfterDelay());

            }
        }
        else
        {
            Debug.Assert(false, "Unknown power up hit ball: " + powerUp.powerUpType);
        }
    }

    public void ToggleGravity(bool enable)
    {
        useGravity = enable;
        GetComponent<Rigidbody2D>().gravityScale = useGravity ? 1.0f : 0;
        AudioManager.Instance.UseGravityMusic(enable);
    }

    public IEnumerator DisableGravityAfterDelay()
    {
        yield return new WaitForSeconds(10);
        ToggleGravity(false);
    }

    public void EnableAccelerationX(bool enable)
    {
        m_accelerationEnabled = enable;
    }
}
