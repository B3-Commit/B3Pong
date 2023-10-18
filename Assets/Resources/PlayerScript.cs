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

    public const float PADDLE_ROTATION_SPEED = 300.0f;
    public const float PADDLE_SPEED = 7.0f;
    public const float Y_POSITION_LIMIT = 4.8f;
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
        Joystick joystick = Joystick.all.ElementAtOrDefault(gamepadId);


        bool rightPressed = false;
        bool downPressed = false;
        bool upPressed = false;
        bool leftPressed = false;

        if (joystick != null)
        {
            leftPressed = joystick.allControls[1].IsActuated() || joystick.allControls[7].IsActuated() || joystick.allControls[6].IsActuated();
            downPressed = joystick.allControls[2].IsActuated() || joystick.allControls[6].IsActuated() || joystick.allControls[5].IsActuated();
            upPressed = joystick.allControls[3].IsActuated() || joystick.allControls[8].IsActuated() || joystick.allControls[7].IsActuated();
            rightPressed = joystick.allControls[4].IsActuated() || joystick.allControls[8].IsActuated() || joystick.allControls[5].IsActuated();
        }
        if (gamepad != null)
        {
            var move = gamepad.rightStick.ReadValue();
            rightPressed |= move.x > 0.2f;
            leftPressed |= move.x < -0.2f;
            upPressed |= move.y > 0.2f;
            downPressed |= move.y < -0.2f;

        }
        if (keyboard != null)
        {
            rightPressed |= keyboard[right].isPressed;
            leftPressed |= keyboard[left].isPressed;
            upPressed |= keyboard[up].isPressed;
            downPressed |= keyboard[down].isPressed;
        }

        rigidBody.velocity = new Vector3(
            0.0f,
            Mathf.Clamp(
                rigidBody.velocity.y + Time.deltaTime * 10.0f * PADDLE_SPEED * ((upPressed ? 1.0f : 0.0f) + (downPressed ? -1.0f : 0.0f)), 
                -PADDLE_SPEED, 
                PADDLE_SPEED),
            0.0f);

        rigidBody.angularVelocity = 
            Mathf.Clamp(
                rigidBody.angularVelocity + Time.deltaTime * 20.0f * PADDLE_ROTATION_SPEED * ((leftPressed ? 1.0f : 0.0f) + (rightPressed ? -1.0f : 0.0f)), 
                -PADDLE_ROTATION_SPEED, 
                PADDLE_ROTATION_SPEED);

        rigidBody.angularDrag = !leftPressed && !rightPressed ? 10.0f : 0.0f;
        rigidBody.drag = !upPressed && !downPressed? 10.0f : 0.0f;

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -Y_POSITION_LIMIT, Y_POSITION_LIMIT), 0);
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
