using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // public Rigidbody2D rigidbody
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("start");
        var rigidBody = GetComponent<Rigidbody2D>();

        float x = Random.Range(0, 2) == 0 ? -3 : 3;
        float y = Random.Range(0, 2) == 0 ? -3 : 3;
        rigidBody.velocity = new Vector2(x, y);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
