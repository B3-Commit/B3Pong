using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.Assertions;

public class JoystickController : IController
{
    Joystick joystick;

    // Make the constructor private so it can only be called internally
    private JoystickController(Joystick joystick)
    {
        this.joystick = joystick;
    }

    // Static method to create a JoystickController instance
    public static JoystickController Create(PlayerId playerId)
    {
        // Player id and controller id should both be in [0, 1]
        // The first dance pad connected will get controller id 0, which
        // should be for the left player.
        int controllerId = (int)playerId;
        Debug.Assert(controllerId >= 0);
        var joysticks = Joystick.all;
        if (joysticks.Count > controllerId)
        {
            return new JoystickController(joysticks[controllerId]);
        }
        else
        {
            return null;
        }
    }

    public bool IsUpPressed()
    {
        return joystick.allControls[3].IsActuated() || joystick.allControls[8].IsActuated() || joystick.allControls[7].IsActuated();
    }

    public bool IsDownPressed()
    {
        return joystick.allControls[2].IsActuated() || joystick.allControls[6].IsActuated() || joystick.allControls[5].IsActuated();
    }

    public bool IsRightPressed()
    {
        return joystick.allControls[4].IsActuated() || joystick.allControls[8].IsActuated() || joystick.allControls[5].IsActuated();
    }

    public bool IsLeftPressed()
    {
        return joystick.allControls[1].IsActuated() || joystick.allControls[7].IsActuated() || joystick.allControls[6].IsActuated();
    }
}
