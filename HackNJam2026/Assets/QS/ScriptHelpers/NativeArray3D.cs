using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using System.Runtime.CompilerServices;
using Unity.Collections;

#region NativeArray3D

[System.Serializable]
public class NativeArray3D<TValue> where TValue : struct
{
    private NativeArray<TValue> Values;
    private int ArraySize;
    private int PlaneSize;

    /// <summary>
    /// Create a new 3D array
    /// </summary>
    /// <param name="arrSize">Determines the size of the array in all 3 dimensions</param>
    public NativeArray3D(int arrSize, Allocator allocator, NativeArrayOptions nativeArrayOptions = NativeArrayOptions.ClearMemory)
    {
        ArraySize = arrSize;
        PlaneSize = arrSize * arrSize;
        Values = new NativeArray<TValue>(arrSize * arrSize * arrSize, allocator, nativeArrayOptions);
    }

    /// <summary>
    /// Create a new 3D array
    /// </summary>
    /// <param name="arrSize">Determines the size of the array in all 3 dimensions</param>
    public NativeArray3D(TValue[] data, int arrSize, Allocator allocator)
    {
        ArraySize = arrSize;
        PlaneSize = arrSize * arrSize;
        Values = new NativeArray<TValue>(data, allocator);
    }

    public void Dispose()
    {
        if (Values.IsCreated) Values.Dispose();
    }


    public TValue this[int x, int y, int z]
    {
        get { return Values[(x * PlaneSize) + (y * ArraySize) + z]; }
    }
    public TValue this[int3 xyz]
    {
        get { return Values[(xyz.x * PlaneSize) + (xyz.y * ArraySize) + xyz.z]; }
    }
    public TValue this[Vector3Int xyz]
    {
        get { return Values[(xyz.x * PlaneSize) + (xyz.y * ArraySize) + xyz.z]; }
    }
    public TValue this[int i]
    {
        get { return Values[i]; }
    }


    public int ArrayLength => Values.Length;
    public int Length3D => ArraySize;
    public int LengthMax => ArraySize * ArraySize * ArraySize;
    public ref NativeArray<TValue> GetNativeArray => ref Values;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetArray(ref NativeArray<TValue> arr, int arrSize)
    {
        ArraySize = arrSize;
        PlaneSize = arrSize * arrSize;
        Values = arr;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetArray(ref NativeArray<TValue> arr)
    {
        Values = arr;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int3 To3D(int id)
    {
        int3 xyz = new int3(0, 0, 0);
        xyz.x = id / (PlaneSize);
        id -= (xyz.x * PlaneSize);
        xyz.y = id / ArraySize;
        xyz.z = id % ArraySize;
        return xyz;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int To1D(int3 xyz)
    {
        return (xyz.x * PlaneSize) + (xyz.y * ArraySize) + xyz.z;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int To1D(int x, int y, int z)
    {
        return (x * PlaneSize) + (y * ArraySize) + z;
    }
}



#endregion
