using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public PlayerScript lastTouch;
    public float currentMinimumSpeed;
    public float currentMaximumSpeed;
    public GameObject ballSpeedText;

    public const float DEFAULT_MINIMUM_SPEED = 4.5f;
    public const float DEFAULT_MAXIMUM_SPEED = 10.0f;
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

        currentMinimumSpeed = DEFAULT_MINIMUM_SPEED * speedAsPercent / 100;
        currentMaximumSpeed = DEFAULT_MAXIMUM_SPEED * speedAsPercent / 100;

        ToggleGravity(false);

        BallEnlargePowerUp.BallEnlargePickup += OnBallEnlargePickup;
        GravityPowerUp.GravityPickup += OnGravityPickup;
    }

    private void OnDestroy()
    {
        BallEnlargePowerUp.BallEnlargePickup -= OnBallEnlargePickup;
        GravityPowerUp.GravityPickup -= OnGravityPickup;
    }

    private void SetInitialVelocity()
    {
        var rigidBody = GetComponent<Rigidbody2D>();
        float x = Random.Range(0, 2) == 0 ? -currentMinimumSpeed : currentMinimumSpeed;
        float y = 0.01f * (float)Random.Range(0, 100 * currentMinimumSpeed);
        rigidBody.velocity = new Vector2(x, y);

        defaultMass = rigidBody.mass;

    }

    private void SetTestVelocity()
    {
        var rigidBody = GetComponent<Rigidbody2D>();
        float x = currentMaximumSpeed;
        float y = 0;
        rigidBody.velocity = new Vector2(x, y);
        Debug.Log($"{x} {y} {Input.GetKeyDown(KeyCode.T)}");
    }

    void Update()
    {
        if (GetComponent<Rigidbody2D>().velocity.magnitude == 0)
        {
            SetInitialVelocity();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            SetTestVelocity();
        }

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
        if (!useGravity && speed < currentMinimumSpeed)
        {
            rigidBody.velocity *= currentMinimumSpeed / speed;
        }

        // There also needs to be a minimum x-velocity for the sake of the gameplay
        float currentMinimumXSpeed = currentMinimumSpeed / 2.0f;
        float currentXVelocity = rigidBody.velocity.x;
        if (System.Math.Abs(currentXVelocity) < currentMinimumXSpeed)
        {
            float newXSpeed = currentXVelocity > 0 ? currentMinimumXSpeed : -currentMinimumXSpeed;
            rigidBody.velocity = new Vector2(newXSpeed, rigidBody.velocity.y);
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
            player.resetDrag();
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

    private void OnBallEnlargePickup()
    {
        transform.localScale += new Vector3(POWER_UP_SIZE_INCR, POWER_UP_SIZE_INCR, 0);
    }

    private void OnGravityPickup()
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

    public void OnSlowMotionActive(bool active)
    {
        m_accelerationEnabled = !active;
    }
}
