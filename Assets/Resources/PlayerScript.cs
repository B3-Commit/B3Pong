using System;
using System.Collections;
using UnityEngine;


public enum PlayerId
{
    NONE = -1,
    LEFT = 0,
    RIGHT = 1,
}

public class PlayerScript : MonoBehaviour
{
    public string playerName;
    public PlayerId playerId = PlayerId.NONE;

    public const float PADDLE_ROTATION_SPEED = 200.0f;
    public const float PADDLE_SPEED = 7.0f;
    public const float Y_POSITION_LIMIT = 4.8f;
    public const float POWER_UP_SIZE_INCR = 0.5f; // 20 %
    public const float PADDLE_SIZE_RESTORE_TIME = 20f; // seconds
    public const float DEFAULT_SIZE_RECTANGLE = 1.5f;
    public const float DEFAULT_SIZE_ELLIPS = 0.35f;

    Sprite originalSprite;
    Sprite ellipseSprite;
    float paddleSizeRatioFromPowerUps = 1;
    float currentDefaultSize = DEFAULT_SIZE_RECTANGLE;


    public float MAX_PRESS_TIME = 1.0f; // second
    public float MINIMUM_DRAG = 10.0f; // Drag when the press time is MAX_PRESS_TIME or more
    public float MAXIMUM_DRAG = 50.0f; // Drag when the press time is 0
    public float BALL_CONTACT_DRAG = 7.0f; // Extra low drag when ball hits

    private float pressTimeUpDown = 0.0f;
    private float pressTimeLeftRight = 0.0f;

    private IController activeController;
    private IController keyboardController;
    private IController joystickController;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(playerName != null);
        Debug.Assert(playerId != PlayerId.NONE);
        keyboardController = new KeyboardController(playerId);
        joystickController = JoystickController.Create(playerId);

        PaddleEnlargePowerUp.PaddleEnlargePickup += OnPaddleEnlargePickup;
        RoundedPaddlePowerUp.RoundedPaddlePickup += OnRoundedPaddlePickup;

        // Save the two sprites as variables
        originalSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        ellipseSprite = Resources.Load<Sprite>("Sprites/ellipsis");

        // Create disabled ellipse collider
        PolygonCollider2D polyCollider = gameObject.AddComponent<PolygonCollider2D>();
        polyCollider.SetPath(0, Utilities.GetEllipseVectorPoints());
        polyCollider.enabled = false;
    }

    IController GetActiveController()
    {

        if (keyboardController.IsUpPressed())
        {
            return keyboardController;
        }
        else if (joystickController != null && joystickController.IsUpPressed())
        {
            return joystickController;
        }
        //else if (gamepadController != null && gamepadController.IsUpPressed())
        //{
        //    activeController = gamepadController;
        //    upPressed = true;
        //}

        return null;
    }

    private void OnDestroy()
    {
        PaddleEnlargePowerUp.PaddleEnlargePickup -= OnPaddleEnlargePickup;
        RoundedPaddlePowerUp.RoundedPaddlePickup -= OnRoundedPaddlePickup;
    }

    // Update is called once per frame
    void Update()
    {
        // If activeController is not set, keep waiting for an input to set it
        if (activeController == null)
        {
            // If any available controller presses up, that is now the active controller
            activeController = GetActiveController();
            return;
        }

        if (paddleSizeRatioFromPowerUps > 1)
        {
            // Shrink back towards normal size
            paddleSizeRatioFromPowerUps -= POWER_UP_SIZE_INCR * Time.deltaTime / PADDLE_SIZE_RESTORE_TIME;
            float newY = currentDefaultSize * paddleSizeRatioFromPowerUps;
            transform.localScale = new Vector3(transform.localScale.x, newY, 0);

        }

        var rigidBody = GetComponent<Rigidbody2D>();

        bool rightPressed = activeController.IsRightPressed();
        bool leftPressed = activeController.IsLeftPressed();
        bool upPressed = activeController.IsUpPressed();
        bool downPressed = activeController.IsDownPressed();

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

        // When up-down key is released, the drag should be set depending on the time the key has been pressed
        if (upPressed || downPressed)
        {
            // If a key is pressed, increment pressTime and set drag to 0
            pressTimeUpDown += Time.deltaTime;
            rigidBody.drag = 0.0f;
        }
        else if (pressTimeUpDown > 0)
        {
            // If no key is pressed and pressTime is greater than 0, a key was just released
            // Calculate and apply dynamic drag based on pressTime, then reset pressTime
            float pressFraction = Mathf.Clamp(pressTimeUpDown / MAX_PRESS_TIME, 0.0f, 1.0f);
            rigidBody.drag = Mathf.Lerp(MAXIMUM_DRAG, MINIMUM_DRAG, pressFraction);
            pressTimeUpDown = 0.0f;
        }

        // Angular drag is set in the same way as translational drag
        if (leftPressed || rightPressed)
        {
            pressTimeLeftRight += Time.deltaTime;
            rigidBody.angularDrag = 0.0f;
        }
        else if (pressTimeLeftRight > 0)
        {
            float pressFraction = Mathf.Clamp(pressTimeLeftRight / MAX_PRESS_TIME, 0.0f, 1.0f);
            rigidBody.angularDrag = Mathf.Lerp(MAXIMUM_DRAG, MINIMUM_DRAG, pressFraction);
            pressTimeLeftRight = 0.0f;
        }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -Y_POSITION_LIMIT, Y_POSITION_LIMIT), 0);
    }

    public bool IsUpPressed()
    {
        return activeController != null && activeController.IsUpPressed();
    }

    public void resetDrag()
    {
        var rigidBody = GetComponent<Rigidbody2D>();

        rigidBody.angularDrag = BALL_CONTACT_DRAG;
        rigidBody.drag = BALL_CONTACT_DRAG;
    }

    private void OnPaddleEnlargePickup(PlayerId playerId)
    {
        if (playerId != this.playerId) { return; }
        paddleSizeRatioFromPowerUps += POWER_UP_SIZE_INCR;
        float newY = currentDefaultSize * paddleSizeRatioFromPowerUps;
        transform.localScale = new Vector3(transform.localScale.x, newY, 0);
    }

    private void OnRoundedPaddlePickup(PlayerId playerId)
    {
        if (playerId != this.playerId) { return; }
        TurnIntoEllipse();
        StopCoroutine("RestoreToRectangleAfterDelay");
        StartCoroutine("RestoreToRectangleAfterDelay");
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
