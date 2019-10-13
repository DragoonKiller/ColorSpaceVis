

// ============================================================================
// 内部使用函数.
// ============================================================================

float util_rd(float n) { return frac(sin(n) * 43758.5453123); }\
float util_rd(float2 n) { return frac(sin(dot(n, float2(12.9898, 4.1414))) * 43758.5453); }

float util_imap(float a, float b, float x) { return (b - a) * x + a; }
float2 util_imap(float2 a, float2 b, float x) { return (b - a) * x + a; }
float3 util_imap(float3 a, float3 b, float x) { return (b - a) * x + a; }
float4 util_imap(float4 a, float4 b, float x) { return (b - a) * x + a; }

float util_mod(float a, float b) { return a - floor(a / b) * b; }
float2 util_mod(float2 a, float b) { return a - floor(a / b.xx) * b; }
float3 util_mod(float3 a, float b) { return a - floor(a / b.xxx) * b; }
float4 util_mod(float4 a, float b) { return a - floor(a / b.xxxx) * b; }
float2 util_mod(float2 a, float2 b) { return a - floor(a / b) * b; }
float3 util_mod(float3 a, float3 b) { return a - floor(a / b) * b; }
float4 util_mod(float4 a, float4 b) { return a - floor(a / b) * b; }
float4 util_taylorInvSqrt(float4 r) { return 1.79284291400159 - 0.85373472095314 * r; }
float util_fade(float t) { return  t * t * t * (t * (t * 6.0 - 15.0) + 10.0); }
float2 util_fade(float2 t) { return  t * t * t * (t * (t * 6.0 - 15.0) + 10.0); }
float3 util_fade(float3 t) { return  t * t * t * (t * (t * 6.0 - 15.0) + 10.0); }
float4 util_fade(float4 t) { return  t * t * t * (t * (t * 6.0 - 15.0) + 10.0); }
float4 util_permute(float4 x) { return util_mod(((x * 34.0) + 1.0) * x, 289.0); }

// ============================================================================
// 通用数学计算
// ============================================================================

// 将 x 从范围 [l, r] 等比映射至范围 [a, b].
float xmap(float x, float l, float r, float a, float b) { return (x - l) / (r - l) * (b - a) + a; }
float2 xmap(float2 x, float l, float r, float a, float b) { return (x - l) / (r - l) * (b - a) + a; }
float3 xmap(float3 x, float l, float r, float a, float b) { return (x - l) / (r - l) * (b - a) + a; }
float4 xmap(float4 x, float l, float r, float a, float b) { return (x - l) / (r - l) * (b - a) + a; }

// 平方函数.
float sqr(float x) { return x * x; }
float2 sqr(float2 x) { return x * x; }
float3 sqr(float3 x) { return x * x; }
float4 sqr(float4 x) { return x * x; }

// 0-1 线性插值.
float imap(float a, float b, float x) { return util_imap(a, b, x); }
float2 imap(float2 a, float2 b, float x) { return util_imap(a, b, x); }
float3 imap(float3 a, float3 b, float x) { return util_imap(a, b, x); }
float4 imap(float4 a, float4 b, float x) { return util_imap(a, b, x); }

// 取正余数.
float mod(float a, float b) { return util_mod(a, b); }
float2 mod(float2 a, float b) { return util_mod(a, b); }
float3 mod(float3 a, float b) { return util_mod(a, b); }
float4 mod(float4 a, float b) { return util_mod(a, b); }
float2 mod(float2 a, float2 b) { return util_mod(a, b); }
float3 mod(float3 a, float3 b) { return util_mod(a, b); }
float4 mod(float4 a, float4 b) { return util_mod(a, b); }

// 角度转二维单位向量.
float2 a2v(float a) { return float2(cos(a), sin(a)); }

// 角度转三维单位向量. 角 a 是偏航角, 范围 [-pi, pi]. 角 b 是俯仰角, 范围 [-0.5 pi, 0.5 pi].
float3 a2v(float a, float b) { return float3(cos(a) * sin(b), sin(a) * sin(b), cos(b)); }


float3 RGB2HSV(float3 c)
{
    float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
	float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
	float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));

	float d = q.x - min(q.w, q.y);
	float e = 1.0e-10;
	return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
}

float3 HSV2RGB(float3 c)
{
      float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
      float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
      return c.z * lerp(K.xxx, saturate(p - K.xxx), c.y);
}

// ============================================================================
// 平滑函数
// ============================================================================

float smoothSin(float x) { return 0.5 * (1 - cos(x * 3.1411592653589793)); }
float2 smoothSin(float2 x) { return 0.5 * (1 - cos(x * 3.1411592653589793)); }
float3 smoothSin(float3 x) { return 0.5 * (1 - cos(x * 3.1411592653589793)); }
float4 smoothSin(float4 x) { return 0.5 * (1 - cos(x * 3.1411592653589793)); }

// ============================================================================
// 噪声
// ============================================================================
// https://gist.github.com/patriciogonzalezvivo/670c22f3966e662d2f83

// 随机数. 0~1.
float rd(float n) { return util_rd(n); }

// 二维随机数. 0~1.
float rd(float2 n) { return util_rd(n); }

// 一维平滑白噪声. 0~1.
float noise(float p)
{
    float fl = floor(p);
    float fc = frac(p);
    return imap(rd(fl), rd(fl + 1.0), fc);
}

// 二维平滑白噪声. 0~1.
float noise(float2 p)
{
    float2 ip = floor(p);
    float2 u = frac(p);
    u = u * u * (3.0 - 2.0 * u);
    float res = imap(
        imap(rd(ip), rd(ip + float2(1.0,0.0)), u.x),
        imap(rd(ip + float2(0.0,1.0)), rd(ip + float2(1.0, 1.0)), u.x), u.y
    );
    return res * res;
}


// 三维平滑白噪声. 0~1.
float noise(float3 p)
{
    float3 a = floor(p);
    float3 d = p - a;
    d = d * d * (3.0 - 2.0 * d);
    float4 b = a.xxyy + float4(0.0, 1.0, 0.0, 1.0);
    float4 k1 = util_permute(b.xyxy);
    float4 k2 = util_permute(k1.xyxy + b.zzww);
    float4 c = k2 + a.zzzz;
    float4 k3 = util_permute(c);
    float4 k4 = util_permute(c + 1.0);
    float4 o1 = frac(k3 * (1.0 / 41.0));
    float4 o2 = frac(k4 * (1.0 / 41.0));
    float4 o3 = o2 * d.z + o1 * (1.0 - d.z);
    float2 o4 = o3.yw * d.x + o3.xz * (1.0 - d.x);
    return o4.y * d.y + o4.x * (1.0 - d.y);
}


// 三维柏林噪声. 0~1.
float perlinNoise(float3 P, float freq)
{
    float unit = 1.0 / freq;
    P = P / unit;
    float3 Pi0 = floor(P);
    float3 Pi1 = Pi0 + float3(1, 1, 1);
    Pi0 = mod(Pi0, 289.0);
    Pi1 = mod(Pi1, 289.0);
    float3 Pf0 = frac(P);
    float3 Pf1 = Pf0 - float3(1, 1, 1);
    float4 ix = float4(Pi0.x, Pi1.x, Pi0.x, Pi1.x);
    float4 iy = float4(Pi0.yy, Pi1.yy);
    float4 iz0 = Pi0.zzzz;
    float4 iz1 = Pi1.zzzz;
    float4 ixy = util_permute(util_permute(ix) + iy);
    float4 ixy0 = util_permute(ixy + iz0);
    float4 ixy1 = util_permute(ixy + iz1);
    float4 gx0 = ixy0 / 7.0;
    float4 gy0 = frac(floor(gx0) / 7.0) - 0.5;
    gx0 = frac(gx0);
    float4 gz0 = float4(0.5, 0.5, 0.5, 0.5) - abs(gx0) - abs(gy0);
    float4 sz0 = step(gz0, float4(0, 0, 0, 0));
    gx0 -= sz0 * (step(0.0, gx0) - 0.5);
    gy0 -= sz0 * (step(0.0, gy0) - 0.5);
    float4 gx1 = ixy1 / 7.0;
    float4 gy1 = frac(floor(gx1) / 7.0) - 0.5;
    gx1 = frac(gx1);
    float4 gz1 = float4(0.5, 0.5, 0.5, 0.5) - abs(gx1) - abs(gy1);
    float4 sz1 = step(gz1, float4(0, 0, 0, 0));
    gx1 -= sz1 * (step(0.0, gx1) - 0.5);
    gy1 -= sz1 * (step(0.0, gy1) - 0.5);
    float3 g000 = float3(gx0.x, gy0.x, gz0.x);
    float3 g100 = float3(gx0.y, gy0.y, gz0.y);
    float3 g010 = float3(gx0.z, gy0.z, gz0.z);
    float3 g110 = float3(gx0.w, gy0.w, gz0.w);
    float3 g001 = float3(gx1.x, gy1.x, gz1.x);
    float3 g101 = float3(gx1.y, gy1.y, gz1.y);
    float3 g011 = float3(gx1.z, gy1.z, gz1.z);
    float3 g111 = float3(gx1.w, gy1.w, gz1.w);
    float4 norm0 = util_taylorInvSqrt(float4(dot(g000, g000), dot(g010, g010), dot(g100, g100), dot(g110, g110)));
    g000 *= norm0.x;
    g010 *= norm0.y;
    g100 *= norm0.z;
    g110 *= norm0.w;
    float4 norm1 = util_taylorInvSqrt(float4(dot(g001, g001), dot(g011, g011), dot(g101, g101), dot(g111, g111)));
    g001 *= norm1.x;
    g011 *= norm1.y;
    g101 *= norm1.z;
    g111 *= norm1.w;
    float n000 = dot(g000, Pf0);
    float n100 = dot(g100, float3(Pf1.x, Pf0.yz));
    float n010 = dot(g010, float3(Pf0.x, Pf1.y, Pf0.z));
    float n110 = dot(g110, float3(Pf1.xy, Pf0.z));
    float n001 = dot(g001, float3(Pf0.xy, Pf1.z));
    float n101 = dot(g101, float3(Pf1.x, Pf0.y, Pf1.z));
    float n011 = dot(g011, float3(Pf0.x, Pf1.yz));
    float n111 = dot(g111, Pf1);
    float3 util_fade_xyz = util_fade(Pf0);
    float4 n_z = imap(float4(n000, n100, n010, n110), float4(n001, n101, n011, n111), util_fade_xyz.z);
    float2 n_yz = imap(n_z.xy, n_z.zw, util_fade_xyz.y);
    float n_xyz = imap(n_yz.x, n_yz.y, util_fade_xyz.x); 
    return 0.5 + n_xyz;
}

// 二维柏林噪声.
float perlinNoise(float2 a, float freq) { return perlinNoise(float3(a.xy, 12123.113), freq); }

// 一维柏林噪声.
float perlinNoise(float a, float freq) { return perlinNoise(float3(a, 3399.123441, 773.113), freq); }
