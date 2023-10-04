using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float velocity = 5.0f;
    public string lastTouch;
    private AudioSource audioSource;

    void Start()
    {
        Debug.Log("start");

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
        if (System.Math.Abs(rigidBody.velocity.x) < 2.0)
        {
            float newX =  rigidBody.velocity.x < 0 ? -2.0f : 2.0f;
            rigidBody.velocity = new Vector2(newX, rigidBody.velocity.y);
        }

        var magnitude = rigidBody.velocity.magnitude;
        if (magnitude < 4.5)
        {
            rigidBody.velocity = rigidBody.velocity / magnitude * velocity;
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
