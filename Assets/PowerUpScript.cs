using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public virtual void Activate()
    {
        // Base activation code here
        Debug.Log("Base Power-Up Activated");
    }
}

