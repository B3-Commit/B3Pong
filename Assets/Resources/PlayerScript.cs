using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public KeyCode up = KeyCode.UpArrow;
    public KeyCode down = KeyCode.DownArrow;
    public KeyCode left = KeyCode.LeftArrow;
    public KeyCode right = KeyCode.RightArrow;
    
    public float angularAcceleration = 1.5f;
    public float paddle_speed = 10.0f;
    public const float Y_POSITION_LIMIT = 4.8f;
    public const float ANGULAR_VELOCITY_LIMIT = 1e3f;
    public const float POWER_UP_SIZE_INCR = 0.3f;
    public const float TIME_SIZE_DECR = 3e-5f;
    public const float DEFAULT_SIZE = 1.5f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.y > DEFAULT_SIZE)
        {
            // Shrink back towards normal size
            transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y - TIME_SIZE_DECR);
        }
        var rigidBody = GetComponent<Rigidbody2D>();
        if (Input.GetKey(up))
        {
            rigidBody.velocity = Vector2.zero;
            transform.position += Vector3.up * paddle_speed * Time.deltaTime;            
        }
        if (Input.GetKey(down))
        {
            rigidBody.velocity = Vector2.zero;
            transform.position += Vector3.down * paddle_speed * Time.deltaTime;
        }
        if (Input.GetKey(left))
        {
            rigidBody.angularVelocity += angularAcceleration;
        }
        if (Input.GetKey(right))
        {
            rigidBody.angularVelocity -= angularAcceleration;
        }
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -Y_POSITION_LIMIT, Y_POSITION_LIMIT), 0);
        rigidBody.angularVelocity = Mathf.Clamp(rigidBody.angularVelocity, -ANGULAR_VELOCITY_LIMIT, ANGULAR_VELOCITY_LIMIT);
        
    }

    public void GetPowerUp(PowerUp powerUp)
    {
        transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y + POWER_UP_SIZE_INCR);
    }
}
