using System.Collections;
using System;
using UnityEngine;

public static class Utilities
{
    public static IEnumerator WaitAndTriggerFunction(float waitTime, Action functionToCall)
    {
		yield return new WaitForSeconds(waitTime);
		functionToCall.Invoke();
    }

    public static void Invoke(this MonoBehaviour mb, Action f, float delay)
    {
        mb.StartCoroutine(InvokeRoutine(f, delay));
    }

    private static IEnumerator InvokeRoutine(System.Action f, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        f();
    }
}