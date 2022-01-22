#ifndef NOISE_SIMPLEX_FUNC
#define NOISE_SIMPLEX_FUNC

#define NOISE_SIMPLEX_1_DIV_289 0.00346020761245674740484429065744f
 
float mod289(float x) {
    return x - floor(x * NOISE_SIMPLEX_1_DIV_289) * 289.0;
}
 
float2 mod289(float2 x) {
    return x - floor(x * NOISE_SIMPLEX_1_DIV_289) * 289.0;
}
 
float3 mod289(float3 x) {
    return x - floor(x * NOISE_SIMPLEX_1_DIV_289) * 289.0;
}
 
float4 mod289(float4 x) {
    return x - floor(x * NOISE_SIMPLEX_1_DIV_289) * 289.0;
}
 
float permute(float x) {
    return mod289(
        x*x*34.0 + x
    );
}
 
float3 permute(float3 x) {
    return mod289(
        x*x*34.0 + x
    );
}
 
float4 permute(float4 x) {
    return mod289(
        x*x*34.0 + x
    );
}
 
float taylorInvSqrt(float r) {
    return 1.79284291400159 - 0.85373472095314 * r;
}
 
float4 taylorInvSqrt(float4 r) {
    return 1.79284291400159 - 0.85373472095314 * r;
}
 
float4 grad4(float j, float4 ip)
{
    const float4 ones = float4(1.0, 1.0, 1.0, -1.0);
    float4 p, s;
    p.xyz = floor( frac(j * ip.xyz) * 7.0) * ip.z - 1.0;
    p.w = 1.5 - dot( abs(p.xyz), ones.xyz );
 
    s = float4(1 - step(0.0, p));
 
    p.xyz -= sign(p.xyz) * (p.w < 0);
 
    return p;
}
 
float snoise(float2 v)
{
    const float4 C = float4(
        0.211324865405187,
        0.366025403784439,
		-0.577350269189626,
        0.024390243902439
    );

    float2 i = floor( v + dot(v, C.yy) );
    float2 x0 = v - i + dot(i, C.xx);
 
    float4 x12 = x0.xyxy + C.xxzz;
    int2 i1 = (x0.x > x0.y) ? float2(1.0, 0.0) : float2(0.0, 1.0);
    x12.xy -= i1;

    i = mod289(i);
    float3 p = permute(
        permute(
                i.y + float3(0.0, i1.y, 1.0 )
        ) + i.x + float3(0.0, i1.x, 1.0 )
    );
 
    float3 m = max(
        0.5 - float3(
            dot(x0, x0),
            dot(x12.xy, x12.xy),
            dot(x12.zw, x12.zw)
        ),
        0.0
    );
    m = m*m ;
    m = m*m ;
 
    float3 x = 2.0 * frac(p * C.www) - 1.0;
    float3 h = abs(x) - 0.5;
    float3 ox = floor(x + 0.5);
    float3 a0 = x - ox;

    m *= 1.79284291400159 - 0.85373472095314 * ( a0*a0 + h*h );

    float3 g;
    g.x = a0.x * x0.x + h.x * x0.y;
    g.yz = a0.yz * x12.xz + h.yz * x12.yw;
    return 130.0 * dot(m, g);
}
 
float snoise(float3 v)
{
    const float2 C = float2(
        0.166666666666666667,
        0.333333333333333333
    );
    const float4 D = float4(0.0, 0.5, 1.0, 2.0);
 
    float3 i = floor( v + dot(v, C.yyy) );
    float3 x0 = v - i + dot(i, C.xxx);
    float3 g = step(x0.yzx, x0.xyz);
    float3 l = 1 - g;
    float3 i1 = min(g.xyz, l.zxy);
    float3 i2 = max(g.xyz, l.zxy);
    float3 x1 = x0 - i1 + C.xxx;
    float3 x2 = x0 - i2 + C.yyy;
    float3 x3 = x0 - D.yyy;

    i = mod289(i);
    float4 p = permute(
        permute(
            permute(
                    i.z + float4(0.0, i1.z, i2.z, 1.0 )
            ) + i.y + float4(0.0, i1.y, i2.y, 1.0 )
        )     + i.x + float4(0.0, i1.x, i2.x, 1.0 )
    );
 
    float n_ = 0.142857142857;
    float3 ns = n_ * D.wyz - D.xzx;
 
    float4 j = p - 49.0 * floor(p * ns.z * ns.z);
 
    float4 x_ = floor(j * ns.z);
    float4 y_ = floor(j - 7.0 * x_ );
 
    float4 x = x_ *ns.x + ns.yyyy;
    float4 y = y_ *ns.x + ns.yyyy;
    float4 h = 1.0 - abs(x) - abs(y);
    float4 b0 = float4( x.xy, y.xy );
    float4 b1 = float4( x.zw, y.zw );
    float4 s0 = floor(b0)*2.0 + 1.0;
    float4 s1 = floor(b1)*2.0 + 1.0;
    float4 sh = -step(h, 0.0);
    float4 a0 = b0.xzyw + s0.xzyw*sh.xxyy ;
    float4 a1 = b1.xzyw + s1.xzyw*sh.zzww ;
    float3 p0 = float3(a0.xy,h.x);
    float3 p1 = float3(a0.zw,h.y);
    float3 p2 = float3(a1.xy,h.z);
    float3 p3 = float3(a1.zw,h.w);
 
    float4 norm = taylorInvSqrt(float4(
        dot(p0, p0),
        dot(p1, p1),
        dot(p2, p2),
        dot(p3, p3)
    ));
    p0 *= norm.x;
    p1 *= norm.y;
    p2 *= norm.z;
    p3 *= norm.w;

    float4 m = max(
        0.6 - float4(
            dot(x0, x0),
            dot(x1, x1),
            dot(x2, x2),
            dot(x3, x3)
        ),
        0.0
    );
    m = m * m;
    return 42.0 * dot(
        m*m,
        float4(
            dot(p0, x0),
            dot(p1, x1),
            dot(p2, x2),
            dot(p3, x3)
        )
    );
}
 
float snoise(float4 v)
{
    const float4 C = float4(
        0.138196601125011,
        0.276393202250021,
        0.414589803375032,
		-0.447213595499958
    );

    float4 i = floor(
        v +
        dot(v, 0.309016994374947451)
    );
    float4 x0 = v - i + dot(i, C.xxxx);
 
    float4 i0;
    float3 isX = step( x0.yzw, x0.xxx );
    float3 isYZ = step( x0.zww, x0.yyz );
    i0.x = isX.x + isX.y + isX.z;
    i0.yzw = 1.0 - isX;
    i0.y += isYZ.x + isYZ.y;
    i0.zw += 1.0 - isYZ.xy;
    i0.z += isYZ.z;
    i0.w += 1.0 - isYZ.z;
 
    float4 i3 = saturate(i0);
    float4 i2 = saturate(i0-1.0);
    float4 i1 = saturate(i0-2.0);

    float4 x1 = x0 - i1 + C.xxxx;
    float4 x2 = x0 - i2 + C.yyyy;
    float4 x3 = x0 - i3 + C.zzzz;
    float4 x4 = x0 + C.wwww;

    i = mod289(i);
    float j0 = permute(
        permute(
            permute(
                permute(i.w) + i.z
            ) + i.y
        ) + i.x
    );
    float4 j1 = permute(
        permute(
            permute(
                permute (
                    i.w + float4(i1.w, i2.w, i3.w, 1.0 )
                ) + i.z + float4(i1.z, i2.z, i3.z, 1.0 )
            ) + i.y + float4(i1.y, i2.y, i3.y, 1.0 )
        ) + i.x + float4(i1.x, i2.x, i3.x, 1.0 )
    );
 
    const float4 ip = float4(
        0.003401360544217687075,
        0.020408163265306122449,
        0.142857142857142857143,
        0.0
    );
 
    float4 p0 = grad4(j0, ip);
    float4 p1 = grad4(j1.x, ip);
    float4 p2 = grad4(j1.y, ip);
    float4 p3 = grad4(j1.z, ip);
    float4 p4 = grad4(j1.w, ip);
 
    float4 norm = taylorInvSqrt(float4(
        dot(p0, p0),
        dot(p1, p1),
        dot(p2, p2),
        dot(p3, p3)
    ));
    p0 *= norm.x;
    p1 *= norm.y;
    p2 *= norm.z;
    p3 *= norm.w;
    p4 *= taylorInvSqrt( dot(p4, p4) );

    float3 m0 = max(
        0.6 - float3(
            dot(x0, x0),
            dot(x1, x1),
            dot(x2, x2)
        ),
        0.0
    );
    float2 m1 = max(
        0.6 - float2(
            dot(x3, x3),
            dot(x4, x4)
        ),
        0.0
    );
    m0 = m0 * m0;
    m1 = m1 * m1;
 
    return 49.0 * (
        dot(
            m0*m0,
            float3(
                dot(p0, x0),
                dot(p1, x1),
                dot(p2, x2)
            )
        ) + dot(
            m1*m1,
            float2(
                dot(p3, x3),
                dot(p4, x4)
            )
        )
    );
}
 
// Description : Array and textureless GLSL 2D/3D/4D simplex
//               noise functions.
//      Author : Ian McEwan, Ashima Arts.
//  Maintainer : ijm
//     Lastmod : 20110822 (ijm)
//     License : Copyright (C) 2011 Ashima Arts. All rights reserved.
//               Distributed under the MIT License. See LICENSE file.
//               https://github.com/ashima/webgl-noise
//
//
//           The text from LICENSE file:
//
//
// Copyright (C) 2011 by Ashima Arts (Simplex noise)
// Copyright (C) 2011 by Stefan Gustavson (Classic noise)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
#endif

// Adapted from:
// https://github.com/BrianSharpe/GPU-Noise-Lib/blob/master/gpu_noise_lib.glsl
//	(C) Brian Sharpe
float4 FAST32_hash_2D_Cell(float2 gridcell)
{
	float2 OFFSET = float2(26.0, 161.0);
	float DOMAIN = 71.0;
	float4 SOMELARGEFLOATS = float4(951.135664, 642.949883, 803.202459, 986.973274);
	float2 P = gridcell - floor(gridcell * (1.0 / DOMAIN)) * DOMAIN;
	P += OFFSET.xy;
	P *= P;
	return frac((P.x * P.y) * (1.0 / SOMELARGEFLOATS.xyzw));
}

float4 FAST32_2_hash_2D(float2 gridcell)
{
	float2 OFFSET = float2(403.839172, 377.242706);
	float DOMAIN = 69.0;
	float SOMELARGEFLOAT = 32745.708984;
	float2 SCALE = float2(2.009842, 1.372549);

	float4 P = float4(gridcell.xy, gridcell.xy + 1.0);
	P = P - floor(P * (1.0 / DOMAIN)) * DOMAIN;
	P = (P * SCALE.xyxy) + OFFSET.xyxy;
	P *= P;
	return frac(P.xzxz * P.yyww * (1.0 / SOMELARGEFLOAT));
}

float Falloff_Xsq_C2(float xsq) { xsq = 1.0 - xsq; return xsq * xsq*xsq; }

float PolkaDot2D(float2 P, float radius_low, float radius_high)
{
	float2 Pi = floor(P);
	float2 Pf = P - Pi;
	float4 hash = FAST32_2_hash_2D(Pi); //  FAST32_hash_2D_Cell(Pi);
	float RADIUS = max(0.0, radius_low + hash.z * (radius_high - radius_low));
	float VALUE = RADIUS / max(radius_high, radius_low);
	RADIUS = 2.0 / RADIUS;
	Pf *= RADIUS;
	Pf -= (RADIUS - 1.0);
	Pf += hash.xy * (RADIUS - 2.0);
	return Falloff_Xsq_C2(min(dot(Pf, Pf), 1.0)) * VALUE;
}
