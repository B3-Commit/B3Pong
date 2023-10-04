using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public string lastTouch;
    public const float MINIMUM_SPEED = 4.5f;
    public const float MAXIMUM_SPEED = 20.0f;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // Hack to remove first silent part of the audio clip
        audioSource.time = GetComponent<AudioSource>().clip.length * .15f;

        var rigidBody = GetComponent<Rigidbody2D>();

        float x = Random.Range(0, 2) == 0 ? -5.0f : 5.0f;
        float y = 0.01f * (float)Random.Range(0, 500);

        rigidBody.velocity = new Vector2(x, y);
    }

    void Update()
    {
        var rigidBody = GetComponent<Rigidbody2D>();

        // To keep the flow going, there needs to be a minimum x velocity
        if (System.Math.Abs(rigidBody.velocity.x) < MINIMUM_SPEED)
        {
            float newX =  rigidBody.velocity.x < 0 ? -MINIMUM_SPEED : MINIMUM_SPEED;
            rigidBody.velocity = new Vector2(newX, rigidBody.velocity.y);
        }

        var speed = rigidBody.velocity.magnitude;
        if (speed < MINIMUM_SPEED)
        {
            rigidBody.velocity = rigidBody.velocity / speed * MINIMUM_SPEED;
        } else if (speed > MAXIMUM_SPEED)
        {
            rigidBody.velocity = rigidBody.velocity / speed * MAXIMUM_SPEED;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        audioSource.Play(0);
        var particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Play();

        // Save last touch
        if (collision.gameObject.name is "Player Right" or "Player Left")
        {
            lastTouch = collision.gameObject.name;
        }
    }
}
