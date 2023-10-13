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

    public const float MAX_ANGLE_ACC = 360.0f;
    public const float PADDLE_SPEED = 10.0f;
    public const float Y_POSITION_LIMIT = 4.8f;
    public const float ANGULAR_VELOCITY_LIMIT = 1e3f;
    public const float POWER_UP_SIZE_INCR = 0.2f; // 20 %
    public const float PADDLE_SIZE_RESTORE_TIME = 40f; // seconds
    public const float DEFAULT_SIZE_RECTANGLE = 1.5f;
    public const float DEFAULT_SIZE_ELLIPS = 0.35f;

    Sprite originalSprite;
    Sprite ellipseSprite;
    float paddleSizeRatioFromPowerUps = 1;
    float currentDefaultSize = DEFAULT_SIZE_RECTANGLE;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(playerName != null);
        Debug.Assert(playerId != -1);

        // Save the two sprites as variables
        originalSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        ellipseSprite = Resources.Load<Sprite>("Sprites/ellipsis");

        // Create disabled ellipse collider
        PolygonCollider2D polyCollider = gameObject.AddComponent<PolygonCollider2D>();
        polyCollider.SetPath(0, Utilities.GetEllipseVectorPoints());
        polyCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (paddleSizeRatioFromPowerUps > 1)
        {
            // Shrink back towards normal size
            paddleSizeRatioFromPowerUps -= POWER_UP_SIZE_INCR * Time.deltaTime / PADDLE_SIZE_RESTORE_TIME;
            float newY = currentDefaultSize * paddleSizeRatioFromPowerUps;
            transform.localScale = new Vector3(transform.localScale.x, newY, 0);

        }

        var rigidBody = GetComponent<Rigidbody2D>();

        var gamepad = Gamepad.all.ElementAtOrDefault(gamepadId);
        var keyboard = Keyboard.current;

        Joystick joystick = Joystick.all.ElementAtOrDefault(gamepadId); ;

        if (joystick != null)
        {
            bool right = joystick.allControls[1].IsActuated() || joystick.allControls[8].IsActuated() || joystick.allControls[5].IsActuated();
            bool down = joystick.allControls[2].IsActuated() || joystick.allControls[6].IsActuated() || joystick.allControls[5].IsActuated();
            bool up = joystick.allControls[3].IsActuated() || joystick.allControls[8].IsActuated() || joystick.allControls[7].IsActuated();
            bool left = joystick.allControls[4].IsActuated() || joystick.allControls[7].IsActuated() || joystick.allControls[6].IsActuated(); 

            transform.position += new Vector3(
                0.0f,
                Time.deltaTime * PADDLE_SPEED * ((up ? 1.0f : 0.0f) + (down ? -1.0f : 0.0f)),
                0.0f);

            if (right)
            {
                rigidBody.angularVelocity += Time.deltaTime * MAX_ANGLE_ACC;
            }
            if (left)
            { 
                rigidBody.angularVelocity += Time.deltaTime * MAX_ANGLE_ACC * -1f;
            }
        }
        if (gamepad != null)
        {
            var move = gamepad.rightStick.ReadValue();
            if (System.Math.Abs(move.y) > 0.2f)
            {
                rigidBody.velocity = Vector2.zero;
                transform.position += new Vector3(
                    0.0f,
                    PADDLE_SPEED * Time.deltaTime * move.y,
                    0.0f);
            }
            if (System.Math.Abs(move.x) > 0.2f)
            {
                rigidBody.angularVelocity += Time.deltaTime * MAX_ANGLE_ACC * move.x;
            }
        }
        else if (keyboard != null)
        {
            if (keyboard[up].isPressed)
            {
                rigidBody.velocity = Vector2.zero;
                transform.position += Vector3.up * PADDLE_SPEED * Time.deltaTime;
            }
            if (keyboard[down].isPressed)
            {
                rigidBody.velocity = Vector2.zero;
                transform.position += Vector3.down * PADDLE_SPEED * Time.deltaTime;
            }
            if (keyboard[left].isPressed)
            {
                rigidBody.angularVelocity += MAX_ANGLE_ACC * Time.deltaTime;
            }
            if (keyboard[right].isPressed)
            {
                rigidBody.angularVelocity -= MAX_ANGLE_ACC * Time.deltaTime;
            }
        }
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -Y_POSITION_LIMIT, Y_POSITION_LIMIT), 0);
        rigidBody.angularVelocity = Mathf.Clamp(rigidBody.angularVelocity, -ANGULAR_VELOCITY_LIMIT, ANGULAR_VELOCITY_LIMIT);

    }

    public void GetPowerUp(PowerUp powerUp)
    {
        if (powerUp.powerUpType == PowerUpManagerScript.PowerUpType.PaddleEnlarge)
        {
            paddleSizeRatioFromPowerUps += POWER_UP_SIZE_INCR;
            float newY = currentDefaultSize * paddleSizeRatioFromPowerUps;
            transform.localScale = new Vector3(transform.localScale.x, newY, 0);
        }
        else if (powerUp.powerUpType == PowerUpManagerScript.PowerUpType.RoundedPaddle)
        {
            TurnIntoEllipse();
            StopCoroutine("RestoreToRectangleAfterDelay");
            StartCoroutine("RestoreToRectangleAfterDelay");
        }
        else
        {
            Debug.Assert(false, "Unknown power up hit player");
        }
    }

    void TurnIntoEllipse()
    {
        // Switch collider
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<PolygonCollider2D>().enabled = true;

        // Change the sprite
        gameObject.GetComponent<SpriteRenderer>().sprite = ellipseSprite;

        transform.localScale = new Vector2(0.35f, 0.35f);
        currentDefaultSize = DEFAULT_SIZE_ELLIPS;
    }

    private IEnumerator RestoreToRectangleAfterDelay()
    {
        yield return new WaitForSeconds(10);
        RestoreRectangle();
    }

    private void RestoreRectangle()
    {
        // Switch collider
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        gameObject.GetComponent<PolygonCollider2D>().enabled = false;

        // Change the sprite
        gameObject.GetComponent<SpriteRenderer>().sprite = originalSprite;

        transform.localScale = new Vector2(0.2f, 1.5f);
        currentDefaultSize = DEFAULT_SIZE_RECTANGLE;
    }


}
