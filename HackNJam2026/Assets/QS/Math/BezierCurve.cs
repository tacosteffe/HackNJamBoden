using UnityEngine;

public struct BezierCurve
{
    public Vector3 P0;
    public Vector3 P1;
    public Vector3 P2;

    /// <summary>
    /// Creates the bezier curve with the 3 given control points
    /// </summary>
    /// <param name="p0">Start</param>
    /// <param name="p1">Mid</param>
    /// <param name="p2">End</param>
    public BezierCurve(Vector3 p0, Vector3 p1, Vector3 p2) => (P0, P1, P2) = (p0, p1, p2);
    /// <summary>
    /// Creates the bezier curve with the 3 given control points
    /// </summary>
    /// <param name="points">Start, Mid, End points in an array</param>
    public BezierCurve(Vector3[] points) => (P0, P1, P2) = (points[0], points[1], points[2]);


    public Vector3 Evaluate(float t)
    {
        return Vector3.Lerp(Vector3.Lerp(P0, P1, t), Vector3.Lerp(P1, P2, t), t);
    }
}
