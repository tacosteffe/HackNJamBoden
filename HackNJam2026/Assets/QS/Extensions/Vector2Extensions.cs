using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public static class Vector2Extensions
{

    #region MATHF EXTENSIONS

    /// <summary>
    /// Per value check of greater than
    /// </summary>
    /// <returns></returns>
    public static bool IsGreater(this Vector2 a, Vector2 b)
    {
        return (a.x > b.x && a.y > b.y);
    }
    /// <summary>
    /// Per value check of lesser than
    /// </summary>
    /// <returns></returns>
    public static bool IsLesser(this Vector2 a, Vector2 b)
    {
        return (a.x < b.x && a.y < b.y);
    }
    /// <summary>
    /// Per value check of greater or equal than
    /// </summary>
    /// <returns></returns>
    public static bool IsGreaterOrEqual(this Vector2 a, Vector2 b)
    {
        return (a.x >= b.x && a.y >= b.y);
    }
    /// <summary>
    /// Per value check of lesser or equal than
    /// </summary>
    /// <returns></returns>
    public static bool IsLesserOrEqual(this Vector2 a, Vector2 b)
    {
        return (a.x <= b.x && a.y <= b.y);
    }

    /// <summary>
    /// Per value check if p is inside min and max.
    /// Checks done with greater | lesser than only
    /// </summary>
    /// <returns></returns>
    public static bool Inside(this Vector2 p, Vector2 min, Vector2 max)
    {
        return p.IsLesser(max) && p.IsGreater(min);
    }

    /// <summary>
    /// Per value check if p is inside min and max.
    /// Checks done with greater or equal | lesser or equal
    /// </summary>
    /// <returns></returns>
    public static bool InsideOrEqual(this Vector2 p, Vector2 min, Vector2 max)
    {
        return p.IsLesserOrEqual(max) && p.IsGreaterOrEqual(min);
    }


    /// <summary>
    /// Per value Abs
    /// </summary>
    /// <returns></returns>
    public static Vector2 Abs(this Vector2 v3)
    {
        v3.x = Mathf.Abs(v3.x);
        v3.y = Mathf.Abs(v3.y);
        return v3;
    }

    #endregion




    /// <summary>
    /// Vector2 to Vector2Int
    /// </summary>
    /// <returns></returns>
    public static Vector2Int ToInt(this Vector2 v)
    {
        return new Vector2Int((int)v.x, (int)v.y);
    }

    /// <summary>
    /// Vector2 to float2
    /// </summary>
    /// <returns></returns>
    public static float2 ToFloat2(this Vector2 v)
    {
        return math.float2(v.x, v.y);
    }

    /// <summary>
    /// Vector2 to double2
    /// </summary>
    /// <returns></returns>
    public static double2 ToDouble2(this Vector2 v)
    {
        return math.double2(v.x, v.y);
    }
}


public static class Vector2IntExtensions
{
    #region MATHF EXTENSIONS


    /// <summary>
    /// Per value check of greater than
    /// </summary>
    /// <returns></returns>
    public static bool IsGreater(this Vector2Int a, Vector2Int b)
    {
        return (a.x > b.x && a.y > b.y);
    }
    /// <summary>
    /// Per value check of lesser than
    /// </summary>
    /// <returns></returns>
    public static bool IsLesser(this Vector2Int a, Vector2Int b)
    {
        return (a.x < b.x && a.y < b.y);
    }
    /// <summary>
    /// Per value check of greater or equal than
    /// </summary>
    /// <returns></returns>
    public static bool IsGreaterOrEqual(this Vector2Int a, Vector2Int b)
    {
        return (a.x >= b.x && a.y >= b.y);
    }
    /// <summary>
    /// Per value check of lesser or equal than
    /// </summary>
    /// <returns></returns>
    public static bool IsLesserOrEqual(this Vector2Int a, Vector2Int b)
    {
        return (a.x <= b.x && a.y <= b.y);
    }

    /// <summary>
    /// Per value check if p is inside min and max.
    /// Checks done with greater | lesser than only
    /// </summary>
    /// <returns></returns>
    public static bool Inside(this Vector2Int p, Vector2Int min, Vector2Int max)
    {
        return p.IsLesser(max) && p.IsGreater(min);
    }

    /// <summary>
    /// Per value check if p is inside min and max.
    /// Checks done with greater or equal | lesser or equal
    /// </summary>
    /// <returns></returns>
    public static bool InsideOrEqual(this Vector2Int p, Vector2Int min, Vector2Int max)
    {
        return p.IsLesserOrEqual(max) && p.IsGreaterOrEqual(min);
    }

    /// <summary>
    /// Per value Abs
    /// </summary>
    /// <returns></returns>
    public static Vector2Int Abs(this Vector2Int v3)
    {
        v3.x = Mathf.Abs(v3.x);
        v3.y = Mathf.Abs(v3.y);
        return v3;
    }

    #endregion


    #region CONVERSIONS

    /// <summary>
    /// Vector2Int to Vector2
    /// </summary>
    /// <returns></returns>
    public static Vector2 ToVector2(this Vector2Int v)
    {
        return new Vector2(v.x, v.y);
    }


    /// <summary>
    /// Vector2Int to float2
    /// </summary>
    /// <returns></returns>
    public static float2 ToFloat3(this Vector2Int v)
    {
        return new float2(v.x, v.y);
    }

    /// <summary>
    /// Vector2Int to int2
    /// </summary>
    /// <returns></returns>
    public static int2 ToInt2(this Vector2Int v)
    {
        return new int2(v.x, v.y);
    }

    /// <summary>
    /// float2 to Vector2Int
    /// </summary>
    /// <returns></returns>
    public static Vector2Int ToVector2Int(this float2 f3)
    {
        return new Vector2Int((int)f3.x, (int)f3.y);
    }

    /// <summary>
    /// int2 to Vector2Int
    /// </summary>
    /// <returns></returns>
    public static Vector2Int ToVector3Int(this int2 i3)
    {
        return new Vector2Int(i3.x, i3.y);
    }


    #endregion
}