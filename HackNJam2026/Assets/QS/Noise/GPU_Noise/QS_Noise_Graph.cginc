
#include "QS_Noise.cginc"



//===========PERLIN2D GRAPH FUNCTIONS ===========//
void Perlin2_float(float2 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, out float noise)
{
    LOOP(P2(pos.x, pos.y, hash, frequency));
}

void Perlin2_half(float2 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, out half noise)
{
    LOOP(P2(pos.x, pos.y, hash, frequency));
}

void Perlin2_turbulence_float(float2 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, out float noise)
{
    LOOP(abs(P2(pos.x, pos.y, hash, frequency)));
}

void Perlin2_turbulence_half(float2 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, out half noise)
{
    LOOP(abs(P2(pos.x, pos.y, hash, frequency)));
}
//===========PERLIN2D GRAPH FUNCTIONS ===========//



//===========PERLIN3D GRAPH FUNCTIONS ===========//
void Perlin3_float(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, out float noise)
{
    LOOP(P3(pos.x, pos.y, pos.z, hash, frequency));
}

void Perlin3_half(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, out half noise)
{
    LOOP(P3(pos.x, pos.y, pos.z, hash, frequency));
}

void Perlin3_turbulence_float(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, out float noise)
{
    LOOP(abs(P3(pos.x, pos.y, pos.z, hash, frequency)));
}

void Perlin3_turbulence_half(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, out half noise)
{
    LOOP(abs(P3(pos.x, pos.y, pos.z, hash, frequency)));
}
//===========PERLIN3D GRAPH FUNCTIONS ===========//



//===========VORONOI2D GRAPH FUNCTIONS ===========//
void Voronoi2_F1_float(float2 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, out float noise)
{
    LOOP(EVALUATE_F1(V2D(pos.x, pos.y, hash, frequency)));
}
void Voronoi2_F2_float(float2 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, out float noise)
{
    LOOP(EVALUATE_F2(V2D(pos.x, pos.y, hash, frequency)));
}
void Voronoi2_F2MinF1_float(float2 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, out float noise)
{
    LOOP(EVALUATE_F2_MIN_F1(V2D(pos.x, pos.y, hash, frequency)));
}
void Voronoi2_Manhattan_float(float2 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, out float noise)
{
    LOOP(EVALUATE_F2_MIN_F1(Manhattan2D(pos.x, pos.y, hash, frequency)));
}

void Voronoi2_F1_half(float2 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, out half noise)
{
    LOOP(EVALUATE_F1(V2D(pos.x, pos.y, hash, frequency)));
}
void Voronoi2_F2_half(float2 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, out half noise)
{
    LOOP(EVALUATE_F2(V2D(pos.x, pos.y, hash, frequency)));
}
void Voronoi2_F2MinF1_half(float2 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, out half noise)
{
    LOOP(EVALUATE_F2_MIN_F1(V2D(pos.x, pos.y, hash, frequency)));
}
void Voronoi2_Manhattan_half(float2 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, out half noise)
{
    LOOP(EVALUATE_F2_MIN_F1(Manhattan2D(pos.x, pos.y, hash, frequency)));
}
//===========VORONOI2D GRAPH FUNCTIONS ===========//




//=========== VORONOI3D GRAPH FUNCTIONS ===========//
void Voronoi3_F1_float(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, out float noise)
{
    LOOP(EVALUATE_F1(V3D(pos.x, pos.y, pos.z, hash, frequency)));
}
void Voronoi3_F2_float(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, out float noise)
{
    LOOP(EVALUATE_F2(V3D(pos.x, pos.y, pos.z, hash, frequency)));
}
void Voronoi3_F2MinF1_float(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, out float noise)
{
    LOOP(EVALUATE_F2_MIN_F1(V3D(pos.x, pos.y, pos.z, hash, frequency)));
}
void Voronoi3_Manhattan_float(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, out float noise)
{
    LOOP(EVALUATE_F2_MIN_F1(Manhattan3D(pos.x, pos.y, pos.z, hash, frequency)));
}

void Voronoi3_F1_half(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, out half noise)
{
    LOOP(EVALUATE_F1(V3D(pos.x, pos.y, pos.z, hash, frequency)));
}
void Voronoi3_F2_half(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, out half noise)
{
    LOOP(EVALUATE_F2(V3D(pos.x, pos.y, pos.z, hash, frequency)));
}
void Voronoi3_F2MinF1_half(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, out half noise)
{
    LOOP(EVALUATE_F2_MIN_F1(V3D(pos.x, pos.y, pos.z, hash, frequency)));
}
void Voronoi3_Manhattan_half(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, out half noise)
{
    LOOP(EVALUATE_F2_MIN_F1(Manhattan3D(pos.x, pos.y, pos.z, hash, frequency)));
}
//=========== VORONOI3D GRAPH FUNCTIONS ===========//




//=========== PERLIN WARP SHADER GRAPH PERLIN ===========//

void DomainWarp1D_Perlin2D_float(float2 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength, out float noise)
{
    noise = DomainWarp1D_Perlin2D(float3(pos.x, 0, pos.y), seed, frequency, lacunarity, persistence, octaves, warpStrength);
}
void DomainWarp1D_Perlin2D_half(float2 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength, out half noise)
{
    noise = DomainWarp1D_Perlin2D(float3(pos.x, 0, pos.y), seed, frequency, lacunarity, persistence, octaves, warpStrength);
}


void DomainWarp2D_Perlin2D_float(float2 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength, out float noise)
{
    noise = DomainWarp2D_Perlin2D(float3(pos.x, 0, pos.y), seed, frequency, lacunarity, persistence, octaves, warpStrength);
}
void DomainWarp2D_Perlin2D_half(float2 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength, out half noise)
{
    noise = DomainWarp2D_Perlin2D(float3(pos.x, 0, pos.y), seed, frequency, lacunarity, persistence, octaves, warpStrength);
}

//=========== PERLIN WARP SHADER GRAPH PERLIN ===========//



//=========== PERLIN WARP SHADER GRAPH PERLIN ===========//
void DomainWarp1D_Perlin3D_float(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength, out float noise)
{
    noise = DomainWarp1D_Perlin3D(pos, seed, frequency, lacunarity, persistence, octaves, warpStrength);
}

void DomainWarp1D_Perlin3D_half(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength, out half noise)
{
    noise = DomainWarp1D_Perlin3D(pos, seed, frequency, lacunarity, persistence, octaves, warpStrength);
}


void DomainWarp2D_Perlin3D_float(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength, out float noise)
{
    noise = DomainWarp2D_Perlin3D(pos, seed, frequency, lacunarity, persistence, octaves, warpStrength);
}

void DomainWarp2D_Perlin3D_half(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength, out half noise)
{
    noise = DomainWarp2D_Perlin3D(pos, seed, frequency, lacunarity, persistence, octaves, warpStrength);
}
//=========== PERLIN WARP SHADER GRAPH PERLIN ===========//





//=========== SHADER GRAPH VORONOI2D WARP ===========//

void DomainWarp1D_Voronoi_2D_F1_float(float2 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength, out float noise)
{
    noise = DomainWarp1D_Voronoi2D_F1(float3(pos.x, 0, pos.y), seed, frequency, lacunarity, persistence, octaves, warpStrength);
}

void DomainWarp1D_Voronoi_2D_F1_half(float2 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength, out half noise)
{
    noise = DomainWarp1D_Voronoi2D_F1(float3(pos.x, 0, pos.y), seed, frequency, lacunarity, persistence, octaves, warpStrength);
}

void DomainWarp2D_Voronoi_2D_F1_float(float2 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength, out float noise)
{
    noise = DomainWarp2D_Voronoi2D_F1(float3(pos.x, 0, pos.y), seed, frequency, lacunarity, persistence, octaves, warpStrength);
}

void DomainWarp2D_Voronoi_2D_F1_half(float2 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength, out half noise)
{
    noise = DomainWarp2D_Voronoi2D_F1(float3(pos.x, 0, pos.y), seed, frequency, lacunarity, persistence, octaves, warpStrength);
}




void DomainWarp1D_Voronoi_2D_F2_float(float2 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength, out float noise)
{
    noise = DomainWarp1D_Voronoi2D_F2(float3(pos.x, 0, pos.y), seed, frequency, lacunarity, persistence, octaves, warpStrength);
}

void DomainWarp1D_Voronoi_2D_F2_half(float2 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength, out half noise)
{
    noise = DomainWarp1D_Voronoi2D_F2(float3(pos.x, 0, pos.y), seed, frequency, lacunarity, persistence, octaves, warpStrength);
}

void DomainWarp2D_Voronoi_2D_F2_float(float2 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength, out float noise)
{
    noise = DomainWarp2D_Voronoi2D_F2(float3(pos.x, 0, pos.y), seed, frequency, lacunarity, persistence, octaves, warpStrength);
}

void DomainWarp2D_Voronoi_2D_F2_half(float2 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength, out half noise)
{
    noise = DomainWarp2D_Voronoi2D_F2(float3(pos.x, 0, pos.y), seed, frequency, lacunarity, persistence, octaves, warpStrength);
}




void DomainWarp1D_Voronoi_2D_F2MinF1_float(float2 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength, out float noise)
{
    noise = DomainWarp1D_Voronoi2D_F2MinF1(float3(pos.x, 0, pos.y), seed, frequency, lacunarity, persistence, octaves, warpStrength);
}

void DomainWarp1D_Voronoi_2D_F2MinF1_half(float2 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength, out half noise)
{
    noise = DomainWarp1D_Voronoi2D_F2MinF1(float3(pos.x, 0, pos.y), seed, frequency, lacunarity, persistence, octaves, warpStrength);
}

void DomainWarp2D_Voronoi_2D_F2MinF1_float(float2 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength, out float noise)
{
    noise = DomainWarp2D_Voronoi2D_F2MinF1(float3(pos.x, 0, pos.y), seed, frequency, lacunarity, persistence, octaves, warpStrength);
}

void DomainWarp2D_Voronoi_2D_F2MinF1_half(float2 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength, out half noise)
{
    noise = DomainWarp2D_Voronoi2D_F2MinF1(float3(pos.x, 0, pos.y), seed, frequency, lacunarity, persistence, octaves, warpStrength);
}




void DomainWarp1D_Voronoi_2D_Manhattan_float(float2 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength, out float noise)
{
    noise = DomainWarp1D_Voronoi2D_Manhattan(float3(pos.x, 0, pos.y), seed, frequency, lacunarity, persistence, octaves, warpStrength);
}

void DomainWarp1D_Voronoi_2D_Manhattan_half(float2 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength, out half noise)
{
    noise = DomainWarp1D_Voronoi2D_Manhattan(float3(pos.x, 0, pos.y), seed, frequency, lacunarity, persistence, octaves, warpStrength);
}

void DomainWarp2D_Voronoi_2D_Manhattan_float(float2 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength, out float noise)
{
    noise = DomainWarp2D_Voronoi2D_Manhattan(float3(pos.x, 0, pos.y), seed, frequency, lacunarity, persistence, octaves, warpStrength);
}

void DomainWarp2D_Voronoi_2D_Manhattan_half(float2 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength, out half noise)
{
    noise = DomainWarp2D_Voronoi2D_Manhattan(float3(pos.x, 0, pos.y), seed, frequency, lacunarity, persistence, octaves, warpStrength);
}

//=========== SHADER GRAPH VORONOI2D WARP ===========//



//=========== SHADER GRAPH VORONOI3D WARP ===========//

void DomainWarp1D_Voronoi_3D_F1_float(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength, out float noise)
{
    noise = DomainWarp1D_Voronoi3D_F1(pos, seed, frequency, lacunarity, persistence, octaves, warpStrength);
}
void DomainWarp1D_Voronoi_3D_F1_half(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength, out half noise)
{
    noise = DomainWarp1D_Voronoi3D_F1(pos, seed, frequency, lacunarity, persistence, octaves, warpStrength);
}
void DomainWarp2D_Voronoi_3D_F1_float(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength, out float noise)
{
    noise = DomainWarp2D_Voronoi3D_F1(pos, seed, frequency, lacunarity, persistence, octaves, warpStrength);
}
void DomainWarp2D_Voronoi_3D_F1_half(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength, out half noise)
{
    noise = DomainWarp2D_Voronoi3D_F1(pos, seed, frequency, lacunarity, persistence, octaves, warpStrength);
}




void DomainWarp1D_Voronoi_3D_F2_float(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength, out float noise)
{
    noise = DomainWarp1D_Voronoi3D_F2(pos, seed, frequency, lacunarity, persistence, octaves, warpStrength);
}
void DomainWarp1D_Voronoi_3D_F2_half(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength, out half noise)
{
    noise = DomainWarp1D_Voronoi3D_F2(pos, seed, frequency, lacunarity, persistence, octaves, warpStrength);
}
void DomainWarp2D_Voronoi_3D_F2_float(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength, out float noise)
{
    noise = DomainWarp2D_Voronoi3D_F2(pos, seed, frequency, lacunarity, persistence, octaves, warpStrength);
}
void DomainWarp2D_Voronoi_3D_F2_half(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength, out half noise)
{
    noise = DomainWarp2D_Voronoi3D_F2(pos, seed, frequency, lacunarity, persistence, octaves, warpStrength);
}




void DomainWarp1D_Voronoi_3D_F2MinF1_float(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength, out float noise)
{
    noise = DomainWarp1D_Voronoi3D_F2MinF1(pos, seed, frequency, lacunarity, persistence, octaves, warpStrength);
}
void DomainWarp1D_Voronoi_3D_F2MinF1_half(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength, out half noise)
{
    noise = DomainWarp1D_Voronoi3D_F2MinF1(pos, seed, frequency, lacunarity, persistence, octaves, warpStrength);
}
void DomainWarp2D_Voronoi_3D_F2MinF1_float(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength, out float noise)
{
    noise = DomainWarp2D_Voronoi3D_F2MinF1(pos, seed, frequency, lacunarity, persistence, octaves, warpStrength);
}
void DomainWarp2D_Voronoi_3D_F2MinF1_half(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength, out half noise)
{
    noise = DomainWarp2D_Voronoi3D_F2MinF1(pos, seed, frequency, lacunarity, persistence, octaves, warpStrength);
}




void DomainWarp1D_Voronoi_3D_Manhattan_float(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength, out float noise)
{
    noise = DomainWarp1D_Voronoi3D_Manhattan(pos, seed, frequency, lacunarity, persistence, octaves, warpStrength);
}
void DomainWarp1D_Voronoi_3D_Manhattan_half(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength, out half noise)
{
    noise = DomainWarp1D_Voronoi3D_Manhattan(pos, seed, frequency, lacunarity, persistence, octaves, warpStrength);
}
void DomainWarp2D_Voronoi_3D_Manhattan_float(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength, out float noise)
{
    noise = DomainWarp2D_Voronoi3D_Manhattan(pos, seed, frequency, lacunarity, persistence, octaves, warpStrength);
}
void DomainWarp2D_Voronoi_3D_Manhattan_half(float3 pos, int seed, float frequency, float lacunarity, float persistence, uint octaves, float warpStrength, out half noise)
{
    noise = DomainWarp2D_Voronoi3D_Manhattan(pos, seed, frequency, lacunarity, persistence, octaves, warpStrength);
}


//=========== SHADER GRAPH VORONOI3D WARP ===========//



