using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using System.Math;

public class Ball : MonoBehaviour
{
    // public Rigidbody2D rigidbody
    // Start is called before the first frame update

    public float velocity = 5.0f;
    private AudioSource audioSource;

    void Start()
    {
        Debug.Log("start");

        // var particleSystem = GetComponent<ParticleSystem>();
        // particleSystem.stop();

        audioSource = GetComponent<AudioSource>();
        // Hack to remove first silent part of the audio clip
        audioSource.time = GetComponent<AudioSource>().clip.length * .15f;

        var rigidBody = GetComponent<Rigidbody2D>();

        float x = Random.Range(0, 2) == 0 ? -5.0f : 5.0f;
        float y = 0.01f * (float)Random.Range(0, 500);

        rigidBody.velocity = new Vector2(x, y);
    }

    // Update is called once per frame
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
    }
}
