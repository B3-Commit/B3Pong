using System.Collections;
using System;
using UnityEngine;

public static class Utilities
{
    public static IEnumerator WaitAndTriggerFunction(float waitTime, Action functionToCall)
    {
		yield return new WaitForSeconds(waitTime);
		functionToCall();
    }
}