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

    // These vector points are adjusted to fit the ellipse.png
    public static Vector2[] GetEllipseVectorPoints()
    {
        return new Vector2[] {
            new(0.44f, -1.535f), new(0.5f, -1.295f), new(0.56f, -0.945f), new(0.58f, -0.785f),
            new(0.59f, -0.685f), new(0.6f, -0.565f), new(0.6f, 0.575f), new(0.58f, 0.785f),
            new(0.56f, 0.945f), new(0.52f, 1.195f), new(0.46f, 1.475f), new(0.41f, 1.635f),
            new(0.34f, 1.815f), new(0.29f, 1.925f), new(0.23f, 2.015f), new(0.16f, 2.085f),
            new(0.09f, 2.135f), new(-0.11f, 2.135f), new(-0.19f, 2.065f), new(-0.29f, 1.925f),
            new(-0.34f, 1.815f), new(-0.42f, 1.635f), new(-0.49f, 1.385f), new(-0.5f, 1.295f),
            new(-0.55f, 1.015f), new(-0.56f, 0.945f), new(-0.58f, 0.785f), new(-0.59f, 0.685f),
            new(-0.6f, 0.565f), new(-0.6f, -0.575f), new(-0.58f, -0.785f), new(-0.56f, -0.945f),
            new(-0.52f, -1.195f), new(-0.49f, -1.335f), new(-0.46f, -1.465f), new(-0.41f, -1.635f),
            new(-0.35f, -1.825f), new(-0.26f, -1.975f), new(-0.16f, -2.085f), new(-0.09f, -2.135f),
            new(0.11f, -2.135f), new(0.19f, -2.055f), new(0.29f, -1.925f), new(0.38f, -1.755f)
        };
    }
}