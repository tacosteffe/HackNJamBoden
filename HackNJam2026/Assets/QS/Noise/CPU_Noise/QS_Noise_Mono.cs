using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class QS_Noise_Mono
{

    #region NOISE COMMONS


    private static readonly uint PRIME_1 = 2246822519;
    private static readonly uint PRIME_2 = 3266489917;
    private static readonly uint PRIME_3 = 668265263;
    //private static readonly uint PRIME_4 = 42512;


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint RotateLeft(uint value) => ((value << 17) | (value >> 15));

    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    //private static float Smooth(float x) => ((x) * (x) * (x) * ((x) * ((x) * 6.0f - 15.0f) + 10.0f));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float GET_FLOAT(uint value, int shift) => ((value >> shift) & 255) * (1.0f / 255.0f);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint EAT(uint hash, int data) => RotateLeft(hash + (uint)data * PRIME_2) ^ (uint)data * PRIME_3;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint SEED(uint seed)
    {
        seed ^= seed >> 15;
        seed *= PRIME_1;
        seed ^= seed >> 13;
        seed *= PRIME_2;
        return seed ^= seed >> 16;
    }

    private struct LatticeSpan
    {
        public int p0, p1;
        public float g0, g1;
        public float t;
        public float v;
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static LatticeSpan GetLatticeSpan(float coord, float frequency)
    {
        coord *= frequency;
        float points = Mathf.Floor(coord);

        LatticeSpan span = new LatticeSpan();
        span.p0 = (int)points;
        span.p1 = span.p0 + 1;
        span.g0 = coord - span.p0;
        span.g1 = span.g0 - 1.0f;
        span.t = coord - points;
        span.t = (span.t * span.t * span.t * (span.t * (span.t * 6.0f - 15.0f) + 10.0f)); // smooth
        return span;
    }


    #endregion





    #region PERLIN EVALUATES


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float EvaluatePerlin(uint hash, float x, float y)
    {
        float gx = ((hash & 255) * (1.0f / 255.0f)) * 2.0f - 1.0f;
        float gy = .5f - Mathf.Abs(gx);
        gx -= Mathf.Floor(gx + .5f);
        return (gx * x + gy * y) * (2.0f / 0.53528f);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float EvaluatePerlin(uint hash, float x, float y, float z)
    {
        float gx = ((hash & 255) * (1.0f / 255.0f)) * 2.0f - 1.0f;
        float gy = ((hash >> 24) * (1.0f / 255.0f)) * 2.0f - 1.0f;
        float gz = 1.0f - Mathf.Abs(gx) - Mathf.Abs(gy);
        float offset = Mathf.Max(-gz, 0.0f);

        if (gx < 0f)    gx += offset;
        else            gx += -offset;

        if (gy < 0f)    gy += offset;
        else            gy += -offset;

        return (gx * x + gy * y + gz * z) * (1.0f / 0.56290f);
    }

    #endregion

    #region PERLIN SAMPLE


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float P2(float px, float pz, uint hash, float frequency)
    {
        LatticeSpan x = GetLatticeSpan(px, frequency);
        LatticeSpan z = GetLatticeSpan(pz, frequency);

        uint h0 = EAT(hash, x.p0),
             h1 = EAT(hash, x.p1);

        return Mathf.Lerp(
            Mathf.Lerp(
                EvaluatePerlin(EAT(h0, z.p0), x.g0, z.g0),
                EvaluatePerlin(EAT(h0, z.p1), x.g0, z.g1), z.t),
            Mathf.Lerp(
                EvaluatePerlin(EAT(h1, z.p0), x.g1, z.g0),
                EvaluatePerlin(EAT(h1, z.p1), x.g1, z.g1), z.t), x.t);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float P3(float px, float py, float pz, uint hash, float frequency)
    {
        LatticeSpan x = GetLatticeSpan(px, frequency);
        LatticeSpan y = GetLatticeSpan(py, frequency);
        LatticeSpan z = GetLatticeSpan(pz, frequency);

        uint h0 = EAT(hash, x.p0), h1 = EAT(hash, x.p1);
        uint h00 = EAT(h0, y.p0), h01 = EAT(h0, y.p1);
        uint h10 = EAT(h1, y.p0), h11 = EAT(h1, y.p1);

        return Mathf.Lerp(
            Mathf.Lerp(
                Mathf.Lerp(
                    EvaluatePerlin(EAT(h00, z.p0), x.g0, y.g0, z.g0),
                    EvaluatePerlin(EAT(h00, z.p1), x.g0, y.g0, z.g1), z.t),
                Mathf.Lerp(
                    EvaluatePerlin(EAT(h01, z.p0), x.g0, y.g1, z.g0),
                    EvaluatePerlin(EAT(h01, z.p1), x.g0, y.g1, z.g1), z.t), y.t),
            Mathf.Lerp(
                Mathf.Lerp(
                    EvaluatePerlin(EAT(h10, z.p0), x.g1, y.g0, z.g0),
                    EvaluatePerlin(EAT(h10, z.p1), x.g1, y.g0, z.g1), z.t),
                Mathf.Lerp(
                    EvaluatePerlin(EAT(h11, z.p0), x.g1, y.g1, z.g0),
                    EvaluatePerlin(EAT(h11, z.p1), x.g1, y.g1, z.g1), z.t), y.t), x.t);
    }

    #endregion

    #region PERLIN

    public static float Perlin2D(Vector2 pos, int seed, float frequency, float lacunarity, float persistence, int octaves)
    {
        float ampSum = 0.0f, amplitude = 1.0f;
        float sum = 0.0f;
        uint hash = SEED((uint)seed);
        for (int o = 0; o < octaves; ++o)
        {
            hash = EAT(hash, o);
            sum += amplitude * P2(pos.x, pos.y, hash, frequency);
            frequency *= lacunarity;
            amplitude *= persistence;
            ampSum += amplitude;
        }
        return Mathf.Clamp(sum / ampSum, -1.0f, 1.0f);
    }

    public static float Perlin2D_Turb(Vector2 pos, int seed, float frequency, float lacunarity, float persistence, int octaves)
    {
        float ampSum = 0.0f, amplitude = 1.0f;
        float sum = 0.0f;
        uint hash = SEED((uint)seed);
        for (int o = 0; o < octaves; ++o)
        {
            hash = EAT(hash, o);
            sum += amplitude * P2(pos.x, pos.y, hash, frequency);
            frequency *= lacunarity;
            amplitude *= persistence;
            ampSum += amplitude;
        }
        return Mathf.Abs(Mathf.Clamp(sum / ampSum, -1.0f, 1.0f));
    }

    public static float Perlin3D(Vector3 pos, int seed, float frequency, float lacunarity, float persistence, int octaves)
    {
        float ampSum = 0.0f, amplitude = 1.0f;
        float sum = 0.0f;
        uint hash = SEED((uint)seed);
        for (int o = 0; o < octaves; ++o)
        {
            hash = EAT(hash, o);
            sum += amplitude * P3(pos.x, pos.y, pos.z, hash, frequency);
            frequency *= lacunarity;
            amplitude *= persistence;
            ampSum += amplitude;
        }
        return Mathf.Clamp(sum / ampSum, -1.0f, 1.0f);
    }

    public static float Perlin3D_Turb(Vector3 pos, int seed, float frequency, float lacunarity, float persistence, int octaves)
    {
        float ampSum = 0.0f, amplitude = 1.0f;
        float sum = 0.0f;
        uint hash = SEED((uint)seed);
        for (int o = 0; o < octaves; ++o)
        {
            hash = EAT(hash, o);
            sum += amplitude * P3(pos.x, pos.y, pos.z, hash, frequency);
            frequency *= lacunarity;
            amplitude *= persistence;
            ampSum += amplitude;
        }
        return Mathf.Abs(Mathf.Clamp(sum / ampSum, -1.0f, 1.0f));
    }


    #endregion





    #region VORONOI COMMONS


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float GetDistance(float x, float y)
    {
        return Mathf.Sqrt(x * x + y * y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float GetDistance(float x, float y, float z)
    {
        return Mathf.Sqrt(x * x + y * y + z * z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float GetDistanceManhattan(float x, float y)
    {
        return Mathf.Abs(x) + Mathf.Abs(y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float GetDistanceManhattan(float x, float y, float z)
    {
        return Mathf.Abs(x) + Mathf.Abs(y) + Mathf.Abs(z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Vector2 UpdateVoronoiMinima(Vector2 minima, float distances)
    {
        bool newMinimum = distances < minima.x;
        minima.y = Mathf.Min(minima.y, distances);
        if (newMinimum)
        {
            minima.y = minima.x;
            minima.x = distances;
        }
        return minima;
    }

    #endregion

    #region VORONOI EVALUATES


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float EvaluateVF1(Vector2 distances)
    {
        return distances.x;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float EvaluateVF2(Vector2 distances)
    {
        return distances.y;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float EvaluateVF2MinusF1(Vector2 distances)
    {
        return distances.y - distances.x;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Vector2 Finalize(Vector2 minima)
    {
        minima.x = Mathf.Min(minima.x, 1.0f);
        minima.y = Mathf.Min(minima.y, 1.0f);
        return minima;
    }

    #endregion

    #region VORONOI SAMPLES

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Vector2 V2D(float px, float pz, uint hash, float frequency)
    {
        LatticeSpan x = GetLatticeSpan(px, frequency);
        LatticeSpan z = GetLatticeSpan(pz, frequency);
        Vector2 minima = new Vector2(2.0f, 2.0f);

        for (int u = -1; u <= 1; ++u)
        {
            uint hx = EAT(hash, x.p0 + u);
            float xOffset = u - x.g0;

            for (int v = -1; v <= 1; ++v)
            {
                uint h = EAT(hx, z.p0 + v);
                float zOffset = v - z.g0;
                minima = UpdateVoronoiMinima(minima,
                GetDistance(
                        (h & 255) * (1.0f / 255.0f) + xOffset,
                        (h >> 8 & 255) * (1.0f / 255.0f) + zOffset));
            }
        }

        return Finalize(minima);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Vector2 Manhattan2D(float px, float pz, uint hash, float frequency)
    {
        LatticeSpan x = GetLatticeSpan(px, frequency);
        LatticeSpan z = GetLatticeSpan(pz, frequency);
        Vector2 minima = new Vector2(2.0f, 2.0f);

        for (int u = -1; u <= 1; ++u)
        {
            uint hx = EAT(hash, x.p0 + u);
            float xOffset = u - x.g0;

            for (int v = -1; v <= 1; ++v)
            {
                uint h = EAT(hx, z.p0 + v);
                float zOffset = v - z.g0;

                minima = UpdateVoronoiMinima(minima, GetDistanceManhattan(
                    (h & 255) * (1.0f / 255.0f) + xOffset,
                    ((h >> 8) & 255) * (1.0f / 255.0f) + zOffset));
            }
        }

        return Finalize(minima);
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Vector2 V3D(float px, float py, float pz, uint hash, float frequency)
    {
        LatticeSpan x = GetLatticeSpan(px, frequency);
        LatticeSpan y = GetLatticeSpan(py, frequency);
        LatticeSpan z = GetLatticeSpan(pz, frequency);
        Vector2 minima = new Vector2(2.0f, 2.0f);

        for (int u = -1; u <= 1; ++u)
        {
            uint hx = EAT(hash, x.p0 + u);
            float xOffset = u - x.g0;

            for (int v = -1; v <= 1; ++v)
            {
                uint hy = EAT(hx, y.p0 + v);
                float yOffset = v - y.g0;

                for (int g = -1; g <= 1; ++g)
                {
                    uint hz = EAT(hy, z.p0 + g);
                    float zOffset = g - z.g0;

                    minima = UpdateVoronoiMinima(minima,
                        GetDistance(
                            (hz & 255) * (1.0f / 255.0f) + xOffset,
                            ((hz >> 8) & 255) * (1.0f / 255.0f) + yOffset,
                            ((hz >> 16) & 255) * (1.0f / 255.0f) + zOffset));
                }
            }
        }

        return Finalize(minima);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Vector2 Manhattan3D(float px, float py, float pz, uint hash, float frequency)
    {
        LatticeSpan x = GetLatticeSpan(px, frequency);
        LatticeSpan y = GetLatticeSpan(py, frequency);
        LatticeSpan z = GetLatticeSpan(pz, frequency);
        Vector2 minima = new Vector2(2.0f, 2.0f);

        for (int u = -1; u <= 1; ++u)
        {
            uint hx = EAT(hash, x.p0 + u);
            float xOffset = u - x.g0;

            for (int v = -1; v <= 1; ++v)
            {
                uint hy = EAT(hx, y.p0 + v);
                float yOffset = v - y.g0;

                for (int g = -1; g <= 1; ++g)
                {
                    uint hz = EAT(hy, z.p0 + g);
                    float zOffset = g - z.g0;

                    minima = UpdateVoronoiMinima(minima,
                        GetDistanceManhattan(
                            (hz & 255) * (1.0f / 255.0f) + xOffset,
                            ((hz >> 8) & 255) * (1.0f / 255.0f) + yOffset,
                            ((hz >> 16) & 255) * (1.0f / 255.0f) + zOffset));
                }
            }
        }

        return Finalize(minima);
    }


    #endregion

    #region VORONOI

    public static float Voronoi2D_F1(Vector2 pos, int seed, float frequency, float lacunarity, float persistence, int octaves)
    {
        float ampSum = 0.0f, amplitude = 1.0f;
        float sum = 0.0f;
        uint hash = SEED((uint)seed);
        for (int o = 0; o < octaves; ++o)
        {
            hash = EAT(hash, o);
            sum += amplitude * EvaluateVF1(V2D(pos.x, pos.y, hash, frequency));
            frequency *= lacunarity;
            amplitude *= persistence;
            ampSum += amplitude;
        }
        return Mathf.Clamp(sum / ampSum, -1.0f, 1.0f);
    }

    public static float Voronoi2D_F2(Vector2 pos, int seed, float frequency, float lacunarity, float persistence, int octaves)
    {
        float ampSum = 0.0f, amplitude = 1.0f;
        float sum = 0.0f;
        uint hash = SEED((uint)seed);
        for (int o = 0; o < octaves; ++o)
        {
            hash = EAT(hash, o);
            sum += amplitude * EvaluateVF2(V2D(pos.x, pos.y, hash, frequency));
            frequency *= lacunarity;
            amplitude *= persistence;
            ampSum += amplitude;
        }
        return Mathf.Clamp(sum / ampSum, -1.0f, 1.0f);
    }

    public static float Voronoi2D_F2MinF1(Vector2 pos, int seed, float frequency, float lacunarity, float persistence, int octaves)
    {
        float ampSum = 0.0f, amplitude = 1.0f;
        float sum = 0.0f;
        uint hash = SEED((uint)seed);
        for (int o = 0; o < octaves; ++o)
        {
            hash = EAT(hash, o);
            sum += amplitude * EvaluateVF2MinusF1(V2D(pos.x, pos.y, hash, frequency));
            frequency *= lacunarity;
            amplitude *= persistence;
            ampSum += amplitude;
        }
        return Mathf.Clamp(sum / ampSum, -1.0f, 1.0f);
    }

    public static float Voronoi2D_Manhattan(Vector2 pos, int seed, float frequency, float lacunarity, float persistence, int octaves)
    {
        float ampSum = 0.0f, amplitude = 1.0f;
        float sum = 0.0f;
        uint hash = SEED((uint)seed);
        for (int o = 0; o < octaves; ++o)
        {
            hash = EAT(hash, o);
            sum += amplitude * EvaluateVF2MinusF1(Manhattan2D(pos.x, pos.y, hash, frequency));
            frequency *= lacunarity;
            amplitude *= persistence;
            ampSum += amplitude;
        }
        return Mathf.Clamp(sum / ampSum, -1.0f, 1.0f);
    }

    public static float Voronoi3D_F1(Vector3 pos, int seed, float frequency, float lacunarity, float persistence, int octaves)
    {
        float ampSum = 0.0f, amplitude = 1.0f;
        float sum = 0.0f;
        uint hash = SEED((uint)seed);
        for (int o = 0; o < octaves; ++o)
        {
            hash = EAT(hash, o);
            sum += amplitude * EvaluateVF1(V3D(pos.x, pos.y, pos.z, hash, frequency));
            frequency *= lacunarity;
            amplitude *= persistence;
            ampSum += amplitude;
        }
        return Mathf.Clamp(sum / ampSum, -1.0f, 1.0f);
    }

    public static float Voronoi3D_F2(Vector3 pos, int seed, float frequency, float lacunarity, float persistence, int octaves)
    {
        float ampSum = 0.0f, amplitude = 1.0f;
        float sum = 0.0f;
        uint hash = SEED((uint)seed);
        for (int o = 0; o < octaves; ++o)
        {
            hash = EAT(hash, o);
            sum += amplitude * EvaluateVF2(V3D(pos.x, pos.y, pos.z, hash, frequency));
            frequency *= lacunarity;
            amplitude *= persistence;
            ampSum += amplitude;
        }
        return Mathf.Clamp(sum / ampSum, -1.0f, 1.0f);
    }

    public static float Voronoi3D_F2MinF1(Vector3 pos, int seed, float frequency, float lacunarity, float persistence, int octaves)
    {
        float ampSum = 0.0f, amplitude = 1.0f;
        float sum = 0.0f;
        uint hash = SEED((uint)seed);
        for (int o = 0; o < octaves; ++o)
        {
            hash = EAT(hash, o);
            sum += amplitude * EvaluateVF2MinusF1(V3D(pos.x, pos.y, pos.z, hash, frequency));
            frequency *= lacunarity;
            amplitude *= persistence;
            ampSum += amplitude;
        }
        return Mathf.Clamp(sum / ampSum, -1.0f, 1.0f);
    }

    public static float Voronoi3D_Manhattan(Vector3 pos, int seed, float frequency, float lacunarity, float persistence, int octaves)
    {
        float ampSum = 0.0f, amplitude = 1.0f;
        float sum = 0.0f;
        uint hash = SEED((uint)seed);
        for (int o = 0; o < octaves; ++o)
        {
            hash = EAT(hash, o);
            sum += amplitude * EvaluateVF2MinusF1(Manhattan3D(pos.x, pos.y, pos.z, hash, frequency));
            frequency *= lacunarity;
            amplitude *= persistence;
            ampSum += amplitude;
        }
        return Mathf.Clamp(sum / ampSum, -1.0f, 1.0f);
    }

    #endregion



    #region DOMAIN WARP PERLIN


    public static float DomainWarp1D_Perlin2D(Vector2 pos, int seed, float frequency, float lacunarity, float persistence, int octaves, float warpStrength)
    {
        Vector2 q = new Vector2(
            Perlin2D(pos, seed, frequency, lacunarity, persistence, octaves),
            Perlin2D(pos + new Vector2(5.2f, 1.3f), seed, frequency, lacunarity, persistence, octaves)) * warpStrength;

        return Perlin2D(pos + (new Vector2(4.0f, 4.0f) * q), seed, frequency, lacunarity, persistence, octaves);
    }

    public static float DomainWarp2D_Perlin2D(Vector2 pos, int seed, float frequency, float lacunarity, float persistence, int octaves, float warpStrength)
    {
        Vector2 q = new Vector2(
            Perlin2D(pos, seed, frequency, lacunarity, persistence, octaves),
            Perlin2D(pos + new Vector2(5.2f, 1.3f), seed, frequency, lacunarity, persistence, octaves)) * warpStrength;

        Vector2 r = new Vector2(
            Perlin2D(pos + (4.0f * q) + new Vector2(1.7f, 9.2f), seed, frequency, lacunarity, persistence, octaves),
            Perlin2D(pos + (4.0f * q) + new Vector2(8.3f, 2.8f), seed, frequency, lacunarity, persistence, octaves)) * warpStrength;

        return Perlin2D(pos + (4.0f * r), seed, frequency, lacunarity, persistence, octaves);
    }


    public static float DomainWarp1D_Perlin3D(Vector3 pos, int seed, float frequency, float lacunarity, float persistence, int octaves, float warpStrength)
    {
        Vector3 q = new Vector3(
            Perlin3D(pos, seed, frequency, lacunarity, persistence, octaves),
            0f,
            Perlin3D(pos + new Vector3(5.2f, 2.4f, 1.3f), seed, frequency, lacunarity, persistence, octaves)) * warpStrength;

        return Perlin3D(pos + (new Vector3(4.0f * q.x, 4.0f * q.y, 4.0f * q.z)), seed, frequency, lacunarity, persistence, octaves);
    }

    public static float DomainWarp2D_Perlin3D(Vector3 pos, int seed, float frequency, float lacunarity, float persistence, int octaves, float warpStrength)
    {
        Vector3 q = new Vector3(
            Perlin3D(pos, seed, frequency, lacunarity, persistence, octaves),
            0f,
            Perlin3D(pos + new Vector3(5.2f, 2.4f, 1.3f), seed, frequency, lacunarity, persistence, octaves)) * warpStrength;

        Vector3 r = new Vector3(
            Perlin3D(pos + (4.0f * q) + new Vector3(1.7f, 5.3f, 9.2f), seed, frequency, lacunarity, persistence, octaves),
            0f,
            Perlin3D(pos + (4.0f * q) + new Vector3(8.3f, 9.2f, 2.8f), seed, frequency, lacunarity, persistence, octaves)) * warpStrength;

        return Perlin3D(pos + (4.0f * r), seed, frequency, lacunarity, persistence, octaves);
    }


    #endregion
}
