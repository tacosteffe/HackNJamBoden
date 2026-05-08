using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Unity.Mathematics;
using System.Runtime.CompilerServices;

[System.Serializable]
public class Array3D<TValue>
{
    private TValue[] Values;
    private int ArraySize;
    private int PlaneSize;

    /// <summary>
    /// Create a new 3D array
    /// </summary>
    /// <param name="arrSize">Determines the size of the array in all 3 dimensions</param>
    public Array3D(int arrSize)
    {
        ArraySize = arrSize;
        PlaneSize = arrSize * arrSize;
        Values = new TValue[arrSize * arrSize * arrSize];
    }

    /// <summary>
    /// Create a new 3D array
    /// </summary>
    /// <param name="arrSize">Determines the size of the array in all 3 dimensions</param>
    public Array3D(TValue[] data, int arrSize)
    {
        ArraySize = arrSize;
        PlaneSize = arrSize * arrSize;
        Values = data;
    }


    public ref TValue this[int x, int y, int z]
    {
        get { return ref Values[(x * PlaneSize) + (y * ArraySize) + z]; }
    }
    public ref TValue this[int3 xyz]
    {
        get { return ref Values[(xyz.x * PlaneSize) + (xyz.y * ArraySize) + xyz.z]; }
    }
    public ref TValue this[Vector3Int xyz]
    {
        get { return ref Values[(xyz.x * PlaneSize) + (xyz.y * ArraySize) + xyz.z]; }
    }
    public ref TValue this[int i]
    {
        get { return ref Values[i]; }
    }

    /// <summary>
    /// The singular length of the array
    /// </summary>
    public int Length => Values.Length;
    /// <summary>
    /// The Cube size of the array
    /// </summary>
    public int ArrayLength => ArraySize;
    /// <summary>
    /// Gets the internal array
    /// </summary>
    public ref TValue[] GetArray => ref Values;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetArray(TValue[] arr, int arrSize)
    {
        ArraySize = arrSize;
        PlaneSize = arrSize * arrSize;
        Values = arr;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetArray(TValue[] arr)
    {
        Values = arr;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int3 ToI3D(int id)
    {
        int3 xyz = new int3(0, 0, 0);
        xyz.x = id / (PlaneSize);
        id -= (xyz.x * PlaneSize);
        xyz.y = id / ArraySize;
        xyz.z = id % ArraySize;
        return xyz;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3Int To3D(int id)
    {
        Vector3Int xyz = new Vector3Int(0, 0, 0);
        xyz.x = id / (PlaneSize);
        id -= (xyz.x * PlaneSize);
        xyz.y = id / ArraySize;
        xyz.z = id % ArraySize;
        return xyz;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int To1D(int x, int y, int z)
    {
        return (x * PlaneSize) + (y * ArraySize) + z;
    }
}
