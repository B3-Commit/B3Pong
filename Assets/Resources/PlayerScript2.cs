using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript2 : PlayerScript
{
    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.y > DEFAULT_SIZE)
        {
            // Shrink back towards normal size
            transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y - TIME_SIZE_DECR);
        }

        var angularAcceleration2 = 50 * 360.0f;
        var rigidBody = GetComponent<Rigidbody2D>();
        var keyboard = Keyboard.current;

        // var gamepad = Gamepad.all.ElementAtOrDefault(gamepadId);
        // if (gamepad != null)
        // {
        //     var move = gamepad.rightStick.ReadValue();
        //     if (System.Math.Abs(move.y) > 0.2f)
        //     {
        //         rigidBody.velocity = Vector2.zero;
        //         transform.position += new Vector3(
        //             0.0f,
        //             paddle_speed * Time.deltaTime * move.y,
        //             0.0f);
        //     }
        //     if (System.Math.Abs(move.x) > 0.2f)
        //     {
        //         rigidBody.angularVelocity += Time.deltaTime * angularAcceleration * move.x;
        //     }
        // }
        // else
        if (keyboard != null)
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
                if (rigidBody.angularVelocity < 0.0f) rigidBody.angularVelocity = 0.0f;
                rigidBody.angularVelocity += angularAcceleration2 * Time.deltaTime;
            }
            if (keyboard[right].isPressed)
            {
                if (rigidBody.angularVelocity > 0.0f) rigidBody.angularVelocity = 0.0f;
                rigidBody.angularVelocity -= angularAcceleration2 * Time.deltaTime;
            }
        }
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -Y_POSITION_LIMIT, Y_POSITION_LIMIT), 0);
        rigidBody.angularVelocity = Mathf.Clamp(rigidBody.angularVelocity, -angularAcceleration2, angularAcceleration2);


        float angle = rigidBody.transform.eulerAngles.z < 180.0f ? rigidBody.transform.eulerAngles.z : rigidBody.transform.eulerAngles.z - 360.0f;
        rigidBody.angularVelocity -= angle * angle * math.sign(angle) * 10.0f * Time.deltaTime;

    }
}
