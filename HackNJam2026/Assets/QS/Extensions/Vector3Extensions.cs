using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Mathematics;

public static class Vector3Extensions
{

    #region MATHF EXTENSIONS


    /// <summary>
    /// Per value check of greater than
    /// </summary>
    /// <returns></returns>
    public static bool IsGreater(this Vector3 a, Vector3 b)
    {
        return (a.x > b.x && a.y > b.y && a.z > b.z);
    }
    /// <summary>
    /// Per value check of lesser than
    /// </summary>
    /// <returns></returns>
    public static bool IsLesser(this Vector3 a, Vector3 b)
    {
        return (a.x < b.x && a.y < b.y && a.z < b.z);
    }
    /// <summary>
    /// Per value check of greater or equal than
    /// </summary>
    /// <returns></returns>
    public static bool IsGreaterOrEqual(this Vector3 a, Vector3 b)
    {
        return (a.x >= b.x && a.y >= b.y && a.z >= b.z);
    }
    /// <summary>
    /// Per value check of lesser or equal than
    /// </summary>
    /// <returns></returns>
    public static bool IsLesserOrEqual(this Vector3 a, Vector3 b)
    {
        return (a.x <= b.x && a.y <= b.y && a.z <= b.z);
    }

    /// <summary>
    /// Per value check if p is inside min and max.
    /// Checks done with greater | lesser than only
    /// </summary>
    /// <returns></returns>
    public static bool Inside(this Vector3 p, Vector3 min, Vector3 max)
    {
        return p.IsLesser(max) && p.IsGreater(min);
    }

    /// <summary>
    /// Per value check if p is inside min and max.
    /// Checks done with greater or equal | lesser or equal
    /// </summary>
    /// <returns></returns>
    public static bool InsideOrEqual(this Vector3 p, Vector3 min, Vector3 max)
    {
        return p.IsLesserOrEqual(max) && p.IsGreaterOrEqual(min);
    }

    /// <summary>
    /// Per value abs
    /// </summary>
    /// <returns></returns>
    public static Vector3 Abs(this Vector3 v3)
    {
        v3.x = Mathf.Abs(v3.x);
        v3.y = Mathf.Abs(v3.y);
        v3.z = Mathf.Abs(v3.z);
        return v3;
    }

    #endregion


    #region CONVERSIONS

    /// <summary>
    /// Vector3 to Vector3Int
    /// </summary>
    /// <returns></returns>
    public static Vector3Int ToInt(this Vector3 v)
    {
        return new Vector3Int((int)v.x, (int)v.y, (int)v.z);
    }

    /// <summary>
    /// Vector3 to float3
    /// </summary>
    /// <returns></returns>
    public static float3 ToFloat3(this Vector3 v)
    {
        return math.float3(v.x, v.y, v.z);
    }

    /// <summary>
    /// float3 to Vector3
    /// </summary>
    /// <returns></returns>
    public static Vector3 ToVector3(this float3 f3)
    {
        return new Vector3(f3.x, f3.y, f3.z);
    }

    #endregion


}

public static class Vector3IntExtensions
{

    #region MATHF EXTENSIONS


    /// <summary>
    /// Per value check of greater than
    /// </summary>
    /// <returns></returns>
    public static bool IsGreater(this Vector3Int a, Vector3Int b)
    {
        return (a.x > b.x && a.y > b.y && a.z > b.z);
    }
    /// <summary>
    /// Per value check of lesser than
    /// </summary>
    /// <returns></returns>
    public static bool IsLesser(this Vector3Int a, Vector3Int b)
    {
        return (a.x < b.x && a.y < b.y && a.z < b.z);
    }
    /// <summary>
    /// Per value check of greater or equal than
    /// </summary>
    /// <returns></returns>
    public static bool IsGreaterOrEqual(this Vector3Int a, Vector3Int b)
    {
        return (a.x >= b.x && a.y >= b.y && a.z >= b.z);
    }
    /// <summary>
    /// Per value check of lesser or equal than
    /// </summary>
    /// <returns></returns>
    public static bool IsLesserOrEqual(this Vector3Int a, Vector3Int b)
    {
        return (a.x <= b.x && a.y <= b.y && a.z <= b.z);
    }

    /// <summary>
    /// Per value check if p is inside min and max.
    /// Checks done with greater | lesser than only
    /// </summary>
    /// <returns></returns>
    public static bool Inside(this Vector3Int p, Vector3Int min, Vector3Int max)
    {
        return p.IsLesser(max) && p.IsGreater(min);
    }

    /// <summary>
    /// Per value check if p is inside min and max.
    /// Checks done with greater or equal | lesser or equal
    /// </summary>
    /// <returns></returns>
    public static bool InsideOrEqual(this Vector3Int p, Vector3Int min, Vector3Int max)
    {
        return p.IsLesserOrEqual(max) && p.IsGreaterOrEqual(min);
    }

    /// <summary>
    /// Per value Abs
    /// </summary>
    /// <returns></returns>
    public static Vector3Int Abs(this Vector3Int v3)
    {
        v3.x = Mathf.Abs(v3.x);
        v3.y = Mathf.Abs(v3.y);
        v3.z = Mathf.Abs(v3.z);
        return v3;
    }

    #endregion



    #region CONVERSIONS


    /// <summary>
    /// Vector3Int to Vector3
    /// </summary>
    /// <returns></returns>
    public static Vector3 ToVector3(this Vector3Int v)
    {
        return new Vector3(v.x, v.y, v.z);
    }

    /// <summary>
    /// Vector3Int to float3
    /// </summary>
    /// <returns></returns>
    public static float3 ToFloat3(this Vector3Int v)
    {
        return new float3(v.x, v.y, v.z);
    }

    /// <summary>
    /// Vector3Int to int3
    /// </summary>
    /// <returns></returns>
    public static int3 ToInt3(this Vector3Int v)
    {
        return new int3(v.x, v.y, v.z);
    }

    /// <summary>
    /// float3 to Vector3Int
    /// </summary>
    /// <returns></returns>
    public static Vector3Int ToVector3Int(this float3 f3)
    {
        return new Vector3Int((int)f3.x, (int)f3.y, (int)f3.z);
    }

    /// <summary>
    /// int3 to Vector3Int
    /// </summary>
    /// <returns></returns>
    public static Vector3Int ToVector3Int(this int3 i3)
    {
        return new Vector3Int(i3.x, i3.y, i3.z);
    }

    #endregion
}

