using UnityEngine;

public class SlowMotionField : MonoBehaviour
{
    public AnimationCurve speedCurve;

    private float fieldMinX;
    private float fieldMaxX;

    void Start()
    {
        fieldMinX = transform.position.x - transform.localScale.x / 2;
        fieldMaxX = transform.position.x + transform.localScale.x / 2;
    }

    public bool IsPointInsideField(Vector2 point)
    {
        var boxCollider = GetComponent<BoxCollider2D>();
        return boxCollider.bounds.Contains(point);
    }

    public float GetCurveValueForPoint(float pointX)
    {
        var percentX = Mathf.Clamp01((pointX - fieldMinX) / (fieldMaxX - fieldMinX));
        var curveValue = speedCurve.Evaluate(percentX);
        return curveValue;
    }
}
