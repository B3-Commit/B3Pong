using UnityEngine;

public class BallDecoy : MonoBehaviour
{
    [SerializeField] private AudioClip bounceSound;

    public const float currentMinimumSpeed = 4.5f;
    public const float currentMaximumSpeed = 20.0f;

    void Start()
    {
        var rigidBody = GetComponent<Rigidbody2D>();

        float x = Random.Range(0, 2) == 0 ? -currentMinimumSpeed : currentMinimumSpeed;
        float y = 0.01f * (float)Random.Range(0, 100 * currentMinimumSpeed);

        rigidBody.velocity = new Vector2(x, y);
    }

    void Update()
    {
        var rigidBody = GetComponent<Rigidbody2D>();

        // To avoid energy loss, there needs to be a minimum velocity
        float timeScaler = 2f;
        var speed = rigidBody.velocity.magnitude;
        if (speed < currentMinimumSpeed)
        {
            rigidBody.velocity += rigidBody.velocity * Time.deltaTime * timeScaler;
        }

        // There also needs to be a minimum x-velocity for the sake of the gameplay
        if (System.Math.Abs(rigidBody.velocity.x) < currentMinimumSpeed / 2.0f)
        {
            var speedDelta = Mathf.Sign(rigidBody.velocity.x) * Time.deltaTime * timeScaler;
            rigidBody.velocity = new Vector2(
                rigidBody.velocity.x + speedDelta,
                rigidBody.velocity.y);
        }

        if (speed > currentMaximumSpeed)
        {
            rigidBody.velocity *= currentMaximumSpeed / speed;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        AudioManager.Instance.PlayEffect(bounceSound);
        var particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Play();
        // Check if the collider object is a Goal
        if (collision.gameObject.CompareTag("Goal"))
        {
            Destroy(gameObject);
        }
    }
}
