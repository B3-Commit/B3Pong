using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

// Explanation for paddle size decrease:
// There are currently two players and two types of power up, meaning that every player
// will get an increase every forth powerup, currently every 20 seconds. To make the
// paddles increase with time over the game, the paddles should restore in
// PADDLE_SIZE_RESTORE_TIME. That means that every update, we should reduce
// the size with POWER_UP_SIZE_INCR * deltaTime / PADDLE_SIZE_RESTORE_TIME at each update;


public class PlayerScript : MonoBehaviour
{
    public string playerName;
    public int playerId = -1;
    public int gamepadId = 0;

    public Key up = Key.UpArrow;
    public Key down = Key.DownArrow;
    public Key left = Key.LeftArrow;
    public Key right = Key.RightArrow;

    public const float angularAcceleration = 360.0f;
    public const float paddle_speed = 10.0f;
    public const float Y_POSITION_LIMIT = 4.8f;
    public const float ANGULAR_VELOCITY_LIMIT = 1e3f;
    public const float POWER_UP_SIZE_INCR = 0.3f;
    public const float PADDLE_SIZE_RESTORE_TIME = 40f; // seconds
    public const float DEFAULT_SIZE = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(playerName != null);
        Debug.Assert(playerId != -1);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.y > DEFAULT_SIZE)
        {
            // Shrink back towards normal size
            float newY = transform.localScale.y - POWER_UP_SIZE_INCR * Time.deltaTime / PADDLE_SIZE_RESTORE_TIME;
            transform.localScale = new Vector2(transform.localScale.x, newY);
        }

        var rigidBody = GetComponent<Rigidbody2D>();

        var gamepad = Gamepad.all.ElementAtOrDefault(gamepadId);
        var keyboard = Keyboard.current;

        if (gamepad != null)
        {
            var move = gamepad.rightStick.ReadValue();
            if (System.Math.Abs(move.y) > 0.2f)
            {
                rigidBody.velocity = Vector2.zero;
                transform.position += new Vector3(
                    0.0f,
                    paddle_speed * Time.deltaTime * move.y,
                    0.0f);
            }
            if (System.Math.Abs(move.x) > 0.2f)
            {
                rigidBody.angularVelocity += Time.deltaTime * angularAcceleration * move.x;
            }
        }
        else if (keyboard != null)
        {
            if (keyboard[up].isPressed)
            {
                rigidBody.velocity = Vector2.zero;
                transform.position += Vector3.up * paddle_speed * Time.deltaTime;
            }
            if (keyboard[down].isPressed)
            {
                rigidBody.velocity = Vector2.zero;
                transform.position += Vector3.down * paddle_speed * Time.deltaTime;
            }
            if (keyboard[left].isPressed)
            {
                rigidBody.angularVelocity += angularAcceleration * Time.deltaTime;
            }
            if (keyboard[right].isPressed)
            {
                rigidBody.angularVelocity -= angularAcceleration * Time.deltaTime;
            }
        }
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -Y_POSITION_LIMIT, Y_POSITION_LIMIT), 0);
        rigidBody.angularVelocity = Mathf.Clamp(rigidBody.angularVelocity, -ANGULAR_VELOCITY_LIMIT, ANGULAR_VELOCITY_LIMIT);

    }

    public void GetPowerUp(PowerUp powerUp)
    {
        if (powerUp.powerUpType == PowerUpManagerScript.PowerUpType.PaddleEnlarge)
        {
            transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y + POWER_UP_SIZE_INCR);
        }
        else
        {
            Debug.Assert(false, "Unknown power up hit player");
        }
    }
}
