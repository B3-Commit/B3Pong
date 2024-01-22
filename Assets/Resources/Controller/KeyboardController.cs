using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardController : IController
{
    readonly KeyCode upKey;
    readonly KeyCode downKey;
    readonly KeyCode rightKey;
    readonly KeyCode leftKey;

    public KeyboardController(PlayerId playerId)
    {
        if (playerId == PlayerId.LEFT)
        {
            upKey = KeyCode.W;
            downKey = KeyCode.S;
            rightKey = KeyCode.D;
            leftKey = KeyCode.A;
        }
        else
        {
            upKey = KeyCode.UpArrow;
            downKey = KeyCode.DownArrow;
            rightKey = KeyCode.RightArrow;
            leftKey = KeyCode.LeftArrow;
        }
    }

    public bool IsUpPressed()
    {
        return Input.GetKey(upKey);
    }

    public bool IsDownPressed()
    {
        return Input.GetKey(downKey);
    }

    public bool IsRightPressed()
    {
        return Input.GetKey(rightKey);
    }

    public bool IsLeftPressed()
    {
        return Input.GetKey(leftKey);
    }
}
