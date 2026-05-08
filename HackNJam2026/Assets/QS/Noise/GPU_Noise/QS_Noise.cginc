///=================NOISE COMMONS=================///

#define B255 (1.0 / 255.0)
#define GET_FLOAT(value, shift) (((value >> shift) & 255) * B255)

#define LOOP(function)\
float ampSum = 0.0, amplitude = 1.0; \
float sum = 0.0; \
uint hash = seed; \
SEED(hash); \
for (uint o = 0; o < octaves; ++o) \
{ \
    hash = EAT(hash, o); \
    sum += amplitude * function; \
    frequency *= lacunarity; \
    amplitude *= persistence; \
    ampSum += amplitude; \
} \
noise = clamp(sum / ampSum, -1.0, 1.0)

#define LOOP_RETURN(function)\
float ampSum = 0.0, amplitude = 1.0; \
float sum = 0.0; \
uint hash = seed; \
SEED(hash); \
for (uint o = 0; o < octaves; ++o) \
{ \
    hash = EAT(hash, o); \
    sum += amplitude * function; \
    frequency *= lacunarity; \
    amplitude *= persistence; \
    ampSum += amplitude; \
} \
return clamp(sum / ampSum, -1.0, 1.0)

///=================NOISE COMMONS=================///


///=================HASHING=================///


#define PRIME_1 2246822519
#define PRIME_2 3266489917
#define PRIME_3 668265263
#define PRIME_4 42512

#define SEED(value) \
value ^= value >> 15;\
value *= PRIME_1;\
value ^= value >> 13;\
value *= PRIME_2;\
value ^= value >> 16

#define SMOOTH(x) ((x) * (x) * (x) * ((x) * ((x) * 6.0 - 15.0) + 10.0))



#define ROTATE_LEFT(value) ((value << 17) | (value >> 15))

#define EAT(hash, data) ROTATE_LEFT((hash) + (uint)(data) * PRIME_2) ^ (uint)(data) * PRIME_3

///=================HASHING=================///

///=================HASHING STRUCTS=================///


struct LatticeSpan
{
    int p0, p1;
    float g0, g1;
    float t;
    float v;
};

LatticeSpan GetLatticeSpan(float coord, float frequency)
{
    coord *= frequency;
    float points = floor(coord);

    LatticeSpan span;
    span.p0 = points;
    span.p1 = span.p0 + 1.0;
    span.g0 = coord - span.p0;
    span.g1 = span.g0 - 1.0;
    span.t = coord - points;
    span.t = SMOOTH(span.t);
    return span;
}

///=================HASHING STRUCTS=================///







//=========== PERLIN EVALUATES ===========//


float EvaluatePerlin(uint hash, float x, float y)
{
    float gx = GET_FLOAT(hash, 0) * 2.0 - 1.0; //((hash & 255) * 1.0 / 255.0) * 2.0 - 1.0;
    float gy = .5 - abs(gx);
    gx -= floor(gx + .5);
    return (gx * x + gy * y) * (2.0 / 0.53528);
}

float EvaluatePerlin(uint hash, float x, float y, float z)
{
    float gx = GET_FLOAT(hash, 0) * 2.0 - 1.0; //((hash & 255) * 1.0 / 255.0) * 2.0 - 1.0;
    float gy = GET_FLOAT(hash, 24) * 2.0 - 1.0; //((hash >> 24) * 1.0 / 255.0) * 2.0 - 1.0;
    float gz = 1.0 - abs(gx) - abs(gy);
    float offset = max(-gz, 0.0);
	
    if (gx < 0)
        gx += offset;
    else
        gx += -offset;
	
    if (gy < 0)
        gy += offset;
    else
        gy += -offset;
	
    return (gx * x + gy * y + gz * z) * (1.0 / 0.56290);
}

//=========== PERLIN EVALUATES ===========//


//===========PERLIN SAMPLE ===========//
float P2(float px, float pz, uint hash, float frequency)
{
    LatticeSpan x = GetLatticeSpan(px, frequency);
    LatticeSpan z = GetLatticeSpan(pz, frequency);

    uint h0 = EAT(hash, x.p0),
         h1 = EAT(hash, x.p1);

    return lerp(
		lerp(
			EvaluatePerlin(EAT(h0, z.p0), x.g0, z.g0),
			EvaluatePerlin(EAT(h0, z.p1), x.g0, z.g1), z.t),
		lerp(
			EvaluatePerlin(EAT(h1, z.p0), x.g1, z.g0),
			EvaluatePerlin(EAT(h1, z.p1), x.g1, z.g1), z.t), x.t);
}

float P3(float px, float py, float pz, uint hash, float frequency)
{
    LatticeSpan x = GetLatticeSpan(px, frequency);
    LatticeSpan y = GetLatticeSpan(py, frequency);
    LatticeSpan z = GetLatticeSpan(pz, frequency);

    uint h0 = EAT(hash, x.p0), h1 = EAT(hash, x.p1);
    uint h00 = EAT(h0, y.p0), h01 = EAT(h0, y.p1);
    uint h10 = EAT(h1, y.p0), h11 = EAT(h1, y.p1);

    return lerp(
		lerp(
			lerp(
				EvaluatePerlin(EAT(h00, z.p0), x.g0, y.g0, z.g0),
				EvaluatePerlin(EAT(h00, z.p1), x.g0, y.g0, z.g1), z.t),
			lerp(
				EvaluatePerlin(EAT(h01, z.p0), x.g0, y.g1, z.g0),
				EvaluatePerlin(EAT(h01, z.p1), x.g0, y.g1, z.g1), z.t), y.t),
		lerp(
			lerp(
				EvaluatePerlin(EAT(h10, z.p0), x.g1, y.g0, z.g0),
				EvaluatePerlin(EAT(h10, z.p1), x.g1, y.g0, z.g1), z.t),
			lerp(
				EvaluatePerlin(EAT(h11, z.p0), x.g1, y.g1, z.g0),
				EvaluatePerlin(EAT(h11, z.p1), x.g1, y.g1, z.g1), z.t), y.t), x.t);
}
//===========PERLIN SAMPLE ===========//


//===========PERLIN CS FUNCTIONS ===========//
float Perlin2D(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves)
{
    LOOP_RETURN(P2(pos.x, pos.z, hash, frequency));
}

float Perlin2D_Turb(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves)
{
    LOOP_RETURN(abs(P2(pos.x, pos.z, hash, frequency)));
}

float Perlin3D(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves)
{
    LOOP_RETURN(P3(pos.x, pos.y, pos.z, hash, frequency));
}

float Perlin3D_Turb(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves)
{
    LOOP_RETURN(abs(P3(pos.x, pos.y, pos.z, hash, frequency)));
}
//===========PERLIN CS FUNCTIONS ===========//===//


//===========VORONOI COMMONS ===========//

float GetDistance(float x, float y)
{
    return sqrt(x * x + y * y);
}
float GetDistance(float x, float y, float z)
{
    return sqrt(x * x + y * y + z * z);
}
float GetDistanceManhattan(float x, float y)
{
    return abs(x) + abs(y);
}
float GetDistanceManhattan(float x, float y, float z)
{
    return abs(x) + abs(y) + abs(z);
}

float2 UpdateVoronoiMinima(float2 minima, float distances)
{
    bool newMinimum = distances < minima.x;
    minima.y = min(minima.y, distances);
    if (newMinimum)
        minima.y = minima.x;
    if (newMinimum)
        minima.x = distances;
    return minima;
}

float UpdateVoronoiCell(float2 minima, float cvs, float distances, float cv)
{
    bool newMinimum = distances < minima.x;
    if (newMinimum)
        cvs = cv;
    return cvs;
}

#define EVALUATE_F1(distances) (distances.x)
#define EVALUATE_F2(distances) (distances.y)
#define EVALUATE_F2_MIN_F1(distances) ((distances.y) - (distances.x))

#define FINALIZE(minima) (min((minima), 1.0))

float EvaluateVF1(float2 distances)
{
    return distances.x;
}
float EvaluateVF2(float2 distances)
{
    return distances.y;
}
float EvaluateVF2MinusF1(float2 distances)
{
    return distances.y - distances.x;
}

float2 Finalize(float2 minima)
{
    return min(minima, 1.0);
}

//===========VORONOI COMMONS ===========//



//===========VORONOI2D SAMPLE ===========//
float2 V2D(float px, float pz, uint hash, float frequency)
{
    LatticeSpan x = GetLatticeSpan(px, frequency);
    LatticeSpan z = GetLatticeSpan(pz, frequency);
    float2 minima = 2.0;

    for (int u = -1; u <= 1; ++u)
    {
        uint hx = EAT(hash, x.p0 + u);
        float xOffset = u - x.g0;

        for (int v = -1; v <= 1; ++v)
        {
            uint h = EAT(hx, z.p0 + v);
            float zOffset = v - z.g0;

            minima = UpdateVoronoiMinima(minima, GetDistance(GET_FLOAT(h, 0) + xOffset, GET_FLOAT(h, 8) + zOffset));
        }
    }

    return FINALIZE(minima);
}

float2 Manhattan2D(float px, float pz, uint hash, float frequency)
{
    LatticeSpan x = GetLatticeSpan(px, frequency);
    LatticeSpan z = GetLatticeSpan(pz, frequency);
    float2 minima = 2.0;

    for (int u = -1; u <= 1; ++u)
    {
        uint hx = EAT(hash, x.p0 + u);
        float xOffset = u - x.g0;

        for (int v = -1; v <= 1; ++v)
        {
            uint h = EAT(hx, z.p0 + v);
            float zOffset = v - z.g0;

            minima = UpdateVoronoiMinima(minima, GetDistanceManhattan(GET_FLOAT(h, 0) + xOffset, GET_FLOAT(h, 8) + zOffset));
        }
    }

    return FINALIZE(minima);
}
//===========VORONOI2D SAMPLE ===========//

//===========VORONOI2D CS FUNCTIONS ===========//
float Voronoi2D_F1(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves)
{
    LOOP_RETURN(EVALUATE_F1(V2D(pos.x, pos.z, hash, frequency)));
}
float Voronoi2D_F2(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves)
{
    LOOP_RETURN(EVALUATE_F2(V2D(pos.x, pos.z, hash, frequency)));
}
float Voronoi2D_F2MinF1(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves)
{
    LOOP_RETURN(EVALUATE_F2_MIN_F1(V2D(pos.x, pos.z, hash, frequency)));
}
float Voronoi2D_Manhattan(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves)
{
    LOOP_RETURN(EVALUATE_F2_MIN_F1(Manhattan2D(pos.x, pos.z, hash, frequency)));
}
//===========VORONOI2D CS FUNCTIONS ===========//



//=========== VORONOI3D SAMPLE ===========//
float2 V3D(float px, float py, float pz, uint hash, float frequency)
{
    LatticeSpan x = GetLatticeSpan(px, frequency);
    LatticeSpan y = GetLatticeSpan(py, frequency);
    LatticeSpan z = GetLatticeSpan(pz, frequency);
    float2 minima = 2.0;

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
					GetDistance(GET_FLOAT(hz, 0) + xOffset, GET_FLOAT(hz, 8) + yOffset, GET_FLOAT(hz, 16) + zOffset));
            }
        }
    }

    return FINALIZE(minima);
}

float2 Manhattan3D(float px, float py, float pz, uint hash, float frequency)
{
    LatticeSpan x = GetLatticeSpan(px, frequency);
    LatticeSpan y = GetLatticeSpan(py, frequency);
    LatticeSpan z = GetLatticeSpan(pz, frequency);
    float2 minima = 2.0;

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
					GetDistanceManhattan(GET_FLOAT(hz, 0) + xOffset, GET_FLOAT(hz, 8) + yOffset, GET_FLOAT(hz, 16) + zOffset));
            }
        }
    }

    return FINALIZE(minima);
}
//=========== VORONOI3D SAMPLE ===========//



//=========== VORONOI3D CS FUNCTIONS ===========//
float Voronoi3D_F1(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves)
{
    LOOP_RETURN(EVALUATE_F1(V3D(pos.x, pos.y, pos.z, hash, frequency)));
}
float Voronoi3D_F2(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves)
{
    LOOP_RETURN(EVALUATE_F2(V3D(pos.x, pos.y, pos.z, hash, frequency)));
}
float Voronoi3D_F2MinF1(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves)
{
    LOOP_RETURN(EVALUATE_F2_MIN_F1(V3D(pos.x, pos.y, pos.z, hash, frequency)));
}
float Voronoi3D_Manhattan(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves)
{
    LOOP_RETURN(EVALUATE_F2_MIN_F1(Manhattan3D(pos.x, pos.y, pos.z, hash, frequency)));
}
//=========== VORONOI3D CS FUNCTIONS ===========//




//=========== PERLIN WARP SAMPLES ===========//
float DomainWarp1D_Perlin2D(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength)
{
    float3 q = float3(
		Perlin2D(pos, seed, frequency, lacunarity, persistence, octaves),
		0,
		Perlin2D(pos + float3(5.2, 0, 1.3), seed, frequency, lacunarity, persistence, octaves)) * warpStrength;

    return Perlin2D(pos + (float3(4.0, 0, 4.0) * q), seed, frequency, lacunarity, persistence, octaves);
}

float DomainWarp2D_Perlin2D(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength)
{
    float3 q = float3(
		Perlin2D(pos, seed, frequency, lacunarity, persistence, octaves),
		0,
		Perlin2D(pos + float3(5.2, 0, 1.3), seed, frequency, lacunarity, persistence, octaves)) * warpStrength;

    float3 r = float3(
		Perlin2D(pos + (4.0 * q) + float3(1.7, 0, 9.2), seed, frequency, lacunarity, persistence, octaves),
		0,
		Perlin2D(pos + (4.0 * q) + float3(8.3, 0, 2.8), seed, frequency, lacunarity, persistence, octaves)) * warpStrength;

    return Perlin2D(pos + (4.0 * r), seed, frequency, lacunarity, persistence, octaves);
}
//=========== PERLIN WARP SAMPLES ===========//


//=========== PERLIN WARP  SAMPLES ===========//
float DomainWarp1D_Perlin3D(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength)
{
    float3 q = float3(
		Perlin3D(pos, seed, frequency, lacunarity, persistence, octaves),
		0,
		Perlin3D(pos + float3(5.2, 2.4, 1.3), seed, frequency, lacunarity, persistence, octaves)) * warpStrength;

    return Perlin3D(pos + (float3(4.0, 4.0, 4.0) * q), seed, frequency, lacunarity, persistence, octaves);
}

float DomainWarp2D_Perlin3D(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength)
{
    float3 q = float3(
		Perlin3D(pos, seed, frequency, lacunarity, persistence, octaves),
		0,
		Perlin3D(pos + float3(5.2, 2.4, 1.3), seed, frequency, lacunarity, persistence, octaves)) * warpStrength;

    float3 r = float3(
		Perlin3D(pos + (4.0 * q) + float3(1.7, 5.3, 9.2), seed, frequency, lacunarity, persistence, octaves),
		0,
		Perlin3D(pos + (4.0 * q) + float3(8.3, 9.2, 2.8), seed, frequency, lacunarity, persistence, octaves)) * warpStrength;

    return Perlin3D(pos + (4.0 * r), seed, frequency, lacunarity, persistence, octaves);
}
//=========== PERLIN WARP  SAMPLES ===========//








//=========== VORONOI2D WARP SAMPLES ===========//
//F1
float DomainWarp1D_Voronoi2D_F1(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength)
{
    float3 q = float3(
		Voronoi2D_F1(pos, seed, frequency, lacunarity, persistence, octaves),
		0,
		Voronoi2D_F1(pos + float3(5.2, 0, 1.3), seed, frequency, lacunarity, persistence, octaves)) * warpStrength;

    return Voronoi2D_F1(pos + (float3(4.0, 0, 4.0) * q), seed, frequency, lacunarity, persistence, octaves);
}

float DomainWarp2D_Voronoi2D_F1(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength)
{
    float3 q = float3(
		Voronoi2D_F1(pos, seed, frequency, lacunarity, persistence, octaves),
		0,
		Voronoi2D_F1(pos + float3(5.2, 0, 1.3), seed, frequency, lacunarity, persistence, octaves)) * warpStrength;

    float3 r = float3(
		Voronoi2D_F1(pos + (4.0 * q) + float3(1.7, 0, 9.2), seed, frequency, lacunarity, persistence, octaves),
		0,
		Voronoi2D_F1(pos + (4.0 * q) + float3(8.3, 0, 2.8), seed, frequency, lacunarity, persistence, octaves)) * warpStrength;

    return Voronoi2D_F1(pos + (4.0 * r), seed, frequency, lacunarity, persistence, octaves);
}

//F2
float DomainWarp1D_Voronoi2D_F2(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength)
{
    float3 q = float3(
		Voronoi2D_F2(pos, seed, frequency, lacunarity, persistence, octaves),
		0,
		Voronoi2D_F2(pos + float3(5.2, 0, 1.3), seed, frequency, lacunarity, persistence, octaves)) * warpStrength;

    return Voronoi2D_F2(pos + (float3(4.0, 0, 4.0) * q), seed, frequency, lacunarity, persistence, octaves);
}

float DomainWarp2D_Voronoi2D_F2(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength)
{
    float3 q = float3(
		Voronoi2D_F2(pos, seed, frequency, lacunarity, persistence, octaves),
		0,
		Voronoi2D_F2(pos + float3(5.2, 0, 1.3), seed, frequency, lacunarity, persistence, octaves)) * warpStrength;

    float3 r = float3(
		Voronoi2D_F2(pos + (4.0 * q) + float3(1.7, 0, 9.2), seed, frequency, lacunarity, persistence, octaves),
		0,
		Voronoi2D_F2(pos + (4.0 * q) + float3(8.3, 0, 2.8), seed, frequency, lacunarity, persistence, octaves)) * warpStrength;

    return Voronoi2D_F2(pos + (4.0 * r), seed, frequency, lacunarity, persistence, octaves);
}

//F2 minus F1
float DomainWarp1D_Voronoi2D_F2MinF1(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength)
{
    float3 q = float3(
		Voronoi2D_F2MinF1(pos, seed, frequency, lacunarity, persistence, octaves),
		0,
		Voronoi2D_F2MinF1(pos + float3(5.2, 0, 1.3), seed, frequency, lacunarity, persistence, octaves)) * warpStrength;

    return Voronoi2D_F2MinF1(pos + (float3(4.0, 0, 4.0) * q), seed, frequency, lacunarity, persistence, octaves);
}

float DomainWarp2D_Voronoi2D_F2MinF1(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength)
{
    float3 q = float3(
		Voronoi2D_F2MinF1(pos, seed, frequency, lacunarity, persistence, octaves),
		0,
		Voronoi2D_F2MinF1(pos + float3(5.2, 0, 1.3), seed, frequency, lacunarity, persistence, octaves)) * warpStrength;

    float3 r = float3(
		Voronoi2D_F2MinF1(pos + (4.0 * q) + float3(1.7, 0, 9.2), seed, frequency, lacunarity, persistence, octaves),
		0,
		Voronoi2D_F2MinF1(pos + (4.0 * q) + float3(8.3, 0, 2.8), seed, frequency, lacunarity, persistence, octaves)) * warpStrength;

    return Voronoi2D_F2MinF1(pos + (4.0 * r), seed, frequency, lacunarity, persistence, octaves);
}

//Manhattan
float DomainWarp1D_Voronoi2D_Manhattan(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength)
{
    float3 q = float3(
		Voronoi2D_Manhattan(pos, seed, frequency, lacunarity, persistence, octaves),
		0,
		Voronoi2D_Manhattan(pos + float3(5.2, 0, 1.3), seed, frequency, lacunarity, persistence, octaves)) * warpStrength;

    return Voronoi2D_Manhattan(pos + (float3(4.0, 0, 4.0) * q), seed, frequency, lacunarity, persistence, octaves);
}

float DomainWarp2D_Voronoi2D_Manhattan(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength)
{
    float3 q = float3(
		Voronoi2D_Manhattan(pos, seed, frequency, lacunarity, persistence, octaves),
		0,
		Voronoi2D_Manhattan(pos + float3(5.2, 0, 1.3), seed, frequency, lacunarity, persistence, octaves)) * warpStrength;

    float3 r = float3(
		Voronoi2D_Manhattan(pos + (4.0 * q) + float3(1.7, 0, 9.2), seed, frequency, lacunarity, persistence, octaves),
		0,
		Voronoi2D_Manhattan(pos + (4.0 * q) + float3(8.3, 0, 2.8), seed, frequency, lacunarity, persistence, octaves)) * warpStrength;

    return Voronoi2D_Manhattan(pos + (4.0 * r), seed, frequency, lacunarity, persistence, octaves);
}
//=========== VORONOI2D WARP SAMPLES ===========//



//=========== VORONOI3D WARP SAMPLES ===========//
//F1
float DomainWarp1D_Voronoi3D_F1(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength)
{
    float3 q = float3(
		Voronoi3D_F1(pos, seed, frequency, lacunarity, persistence, octaves),
		0,
		Voronoi3D_F1(pos + float3(5.2, 0, 1.3), seed, frequency, lacunarity, persistence, octaves)) * warpStrength;

    return Voronoi3D_F1(pos + (float3(4.0, 0, 4.0) * q), seed, frequency, lacunarity, persistence, octaves);
}

float DomainWarp2D_Voronoi3D_F1(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength)
{
    float3 q = float3(
		Voronoi3D_F1(pos, seed, frequency, lacunarity, persistence, octaves),
		0,
		Voronoi3D_F1(pos + float3(5.2, 0, 1.3), seed, frequency, lacunarity, persistence, octaves)) * warpStrength;

    float3 r = float3(
		Voronoi3D_F1(pos + (4.0 * q) + float3(1.7, 0, 9.2), seed, frequency, lacunarity, persistence, octaves),
		0,
		Voronoi3D_F1(pos + (4.0 * q) + float3(8.3, 0, 2.8), seed, frequency, lacunarity, persistence, octaves)) * warpStrength;

    return Voronoi3D_F1(pos + (4.0 * r), seed, frequency, lacunarity, persistence, octaves);
}

//F2
float DomainWarp1D_Voronoi3D_F2(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength)
{
    float3 q = float3(
		Voronoi3D_F2(pos, seed, frequency, lacunarity, persistence, octaves),
		0,
		Voronoi3D_F2(pos + float3(5.2, 0, 1.3), seed, frequency, lacunarity, persistence, octaves)) * warpStrength;

    return Voronoi3D_F2(pos + (float3(4.0, 0, 4.0) * q), seed, frequency, lacunarity, persistence, octaves);
}

float DomainWarp2D_Voronoi3D_F2(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength)
{
    float3 q = float3(
		Voronoi3D_F2(pos, seed, frequency, lacunarity, persistence, octaves),
		0,
		Voronoi3D_F2(pos + float3(5.2, 0, 1.3), seed, frequency, lacunarity, persistence, octaves)) * warpStrength;

    float3 r = float3(
		Voronoi3D_F2(pos + (4.0 * q) + float3(1.7, 0, 9.2), seed, frequency, lacunarity, persistence, octaves),
		0,
		Voronoi3D_F2(pos + (4.0 * q) + float3(8.3, 0, 2.8), seed, frequency, lacunarity, persistence, octaves)) * warpStrength;

    return Voronoi3D_F2(pos + (4.0 * r), seed, frequency, lacunarity, persistence, octaves);
}

//F2 minus F1
float DomainWarp1D_Voronoi3D_F2MinF1(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength)
{
    float3 q = float3(
		Voronoi3D_F2MinF1(pos, seed, frequency, lacunarity, persistence, octaves),
		0,
		Voronoi3D_F2MinF1(pos + float3(5.2, 0, 1.3), seed, frequency, lacunarity, persistence, octaves)) * warpStrength;

    return Voronoi3D_F2MinF1(pos + (float3(4.0, 0, 4.0) * q), seed, frequency, lacunarity, persistence, octaves);
}

float DomainWarp2D_Voronoi3D_F2MinF1(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength)
{
    float3 q = float3(
		Voronoi3D_F2MinF1(pos, seed, frequency, lacunarity, persistence, octaves),
		0,
		Voronoi3D_F2MinF1(pos + float3(5.2, 0, 1.3), seed, frequency, lacunarity, persistence, octaves)) * warpStrength;

    float3 r = float3(
		Voronoi3D_F2MinF1(pos + (4.0 * q) + float3(1.7, 0, 9.2), seed, frequency, lacunarity, persistence, octaves),
		0,
		Voronoi3D_F2MinF1(pos + (4.0 * q) + float3(8.3, 0, 2.8), seed, frequency, lacunarity, persistence, octaves)) * warpStrength;

    return Voronoi3D_F2MinF1(pos + (4.0 * r), seed, frequency, lacunarity, persistence, octaves);
}

//Manhattan
float DomainWarp1D_Voronoi3D_Manhattan(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength)
{
    float3 q = float3(
		Voronoi3D_Manhattan(pos, seed, frequency, lacunarity, persistence, octaves),
		0,
		Voronoi3D_Manhattan(pos + float3(5.2, 0, 1.3), seed, frequency, lacunarity, persistence, octaves)) * warpStrength;

    return Voronoi3D_Manhattan(pos + (float3(4.0, 0, 4.0) * q), seed, frequency, lacunarity, persistence, octaves);
}

float DomainWarp2D_Voronoi3D_Manhattan(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength)
{
    float3 q = float3(
		Voronoi3D_Manhattan(pos, seed, frequency, lacunarity, persistence, octaves),
		0,
		Voronoi3D_Manhattan(pos + float3(5.2, 0, 1.3), seed, frequency, lacunarity, persistence, octaves)) * warpStrength;

    float3 r = float3(
		Voronoi3D_Manhattan(pos + (4.0 * q) + float3(1.7, 0, 9.2), seed, frequency, lacunarity, persistence, octaves),
		0,
		Voronoi3D_Manhattan(pos + (4.0 * q) + float3(8.3, 0, 2.8), seed, frequency, lacunarity, persistence, octaves)) * warpStrength;

    return Voronoi3D_Manhattan(pos + (4.0 * r), seed, frequency, lacunarity, persistence, octaves);
}
//=========== VORONOI3D WARP SAMPLES ===========//

