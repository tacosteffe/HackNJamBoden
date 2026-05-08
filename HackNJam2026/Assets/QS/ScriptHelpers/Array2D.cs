using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Unity.Mathematics;
using System.Runtime.CompilerServices;


[System.Serializable]
public class Array2D<TValue>
{
    private TValue[] Values;
    private int ArraySize;

    /// <summary>
    /// Create a new 2D array
    /// </summary>
    /// <param name="arrSize">Determines the size of the arrays in both dimensions</param>
    public Array2D(int arrSize)
    {
        ArraySize = arrSize;
        Values = new TValue[arrSize * arrSize];
    }

    /// <summary>
    /// Create a new 2D array
    /// </summary>
    /// <param name="arrSize">Determines the size of the array in both dimensions</param>
    public Array2D(TValue[] data, int arrSize)
    {
        ArraySize = arrSize;
        Values = data;
    }

    public ref TValue this[int x, int y]
    {
        get { return ref Values[(x * ArraySize) + y]; }
    }
    public ref TValue this[int3 xz]
    {
        get { return ref Values[(xz.x * ArraySize) + xz.z]; }
    }
    public ref TValue this[int2 xy]
    {
        get { return ref Values[(xy.x * ArraySize) + xy.y]; }
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
    /// The Quardatic size of the array
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
        Values = arr;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetArray(TValue[] arr)
    {
        Values = arr;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int2 ToI2D(int id)
    {
        int2 xy = 0;
        xy.x = id / ArraySize;
        xy.y = id % ArraySize;
        return xy;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2Int To2D(int id)
    {
        Vector2Int xy = new();
        xy.x = id / ArraySize;
        xy.y = id % ArraySize;
        return xy;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int To1D(int x, int y)
    {
        return (x * ArraySize) + y;
    }


}


