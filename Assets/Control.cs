using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Control : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float speed = 8.0f;
    [SerializeField] GameObject paddle;
    // public Rigidbody body;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 
        var rigidBody = GetComponent<Rigidbody2D>();
        // rigidBody.velocity = Vector3.zero;
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        transform.position += move * speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.Z))
        {
            rigidBody.angularVelocity = 0f;
            transform.RotateAround(new Vector3(0, 0, 1), 0.01f);
            
        }
        if (Input.GetKey(KeyCode.X))
        {
            rigidBody.angularVelocity = 0f;
            transform.RotateAround(new Vector3(0, 0, 1), -0.01f);
        }
        // transform.Rotate()
    }
}
