Shader "Hidden/ColorCircle"
{
    Properties
    {
        _Radius ("Radius", float) = 0.8
        _FadeRadius ("FadeRadius", float) = 0.02
    }
    
    SubShader
    {
        Tags { "Queue" = "Transparent" }
        
        Cull Off
        ZWrite Off
        ZTest Less
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Utils.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float _Radius;
            float _FadeRadius;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            float4 frag(v2f i) : SV_Target
            {
                float2 normPos = float2(i.uv.x * 2 - 1, i.uv.y * 2 - 1);
                
                float radius = length(normPos);
                float alpha = 1.0;
                if(radius < _Radius - _FadeRadius) alpha = 0;
                else if(radius < _Radius) alpha = (radius - (_Radius - _FadeRadius)) / _FadeRadius;
                else if(radius < 1.0 - _FadeRadius) alpha = 1;
                else if(radius <= 1.0) alpha = 1 - (radius - (1.0 - _FadeRadius)) / _FadeRadius;
                else alpha = 0;
                
                float angle = atan2(normPos.y, normPos.x);
                angle = angle / 3.141592653589793 / 2 + 0.5;
                 
                return float4(HSV2RGB(float3(angle, 1, 1)), alpha);
            }
            ENDCG
        }
    }
}
