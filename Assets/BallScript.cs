using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using System.Math;

public class Ball : MonoBehaviour
{
    // public Rigidbody2D rigidbody
    // Start is called before the first frame update

    public float velocity = 5.0f;
    void Start()
    {
        Debug.Log("start");
        var rigidBody = GetComponent<Rigidbody2D>();

        float x = Random.Range(4f, 5f);
        float y = 5.0f - (float)(System.Math.Sqrt(System.Math.Abs(x)));

        rigidBody.velocity = new Vector2(x, y);
    }

    // Update is called once per frame
    void Update()
    {
        var rigidBody = GetComponent<Rigidbody2D>();
        var magnitude = rigidBody.velocity.magnitude;
        if (magnitude < 4.5)
        {
            rigidBody.velocity = rigidBody.velocity / magnitude * velocity;
        }
    }
}
