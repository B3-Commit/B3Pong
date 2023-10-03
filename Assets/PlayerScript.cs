using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public KeyCode up = KeyCode.UpArrow;
    public KeyCode down = KeyCode.DownArrow;
    public KeyCode left = KeyCode.LeftArrow;
    public KeyCode right = KeyCode.RightArrow;
    public float angularAcceleration = 1.0f;

    [SerializeField] float speed = 8.0f;


    // Start is called before the first frame update
    void Start()
    {

            var sprite = GetComponent<SpriteRenderer>().sprite.bounds;
            var boxColl = GetComponent<BoxCollider2D>();
            boxColl.size = sprite.size;
            // boxColl.offset = sprite.center;
            
        
    }

    // Update is called once per frame
    void Update()
    {
        var rigidBody = GetComponent<Rigidbody2D>();
        if (Input.GetKey(up))
        {
            rigidBody.velocity = Vector2.zero;
            transform.position += Vector3.up * speed * Time.deltaTime;            
        }
        if (Input.GetKey(down))
        {
            rigidBody.velocity = Vector2.zero;
            transform.position += Vector3.down * speed * Time.deltaTime;
        }
        if (Input.GetKey(left))
        {
            rigidBody.angularVelocity += angularAcceleration;
            // transform.RotateAround(new Vector3(0, 0, 1), 0.01f);
        }
        if (Input.GetKey(right))
        {
            rigidBody.angularVelocity -= angularAcceleration;
            // transform.RotateAround(new Vector3(0, 0, 1), -0.01f);
        }
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.5f, 3.5f), 0);
        rigidBody.angularVelocity = Mathf.Clamp(rigidBody.angularVelocity, -1000f, 1000f);
        
    }
}
