Shader "Unlit/Fire"
{
    Properties
    {
        _NoiseScale ("Noise Scale", Float) = 1
        _TimeScale ("Time Scale", Float) = 1
        _GradientScale ("Gradient Scale", Float) = 1
        _Color1 ("Color 1", Color) = (1,1,1,1)
        _Color2 ("Color 2", Color) = (0.5,0.5,0.5,0.5)
        _Color3 ("Color 3", Color) = (0,0,0,0)
        _Color4 ("Color 3", Color) = (0,0,0,0)
        _Color5 ("Color 3", Color) = (0,0,0,0)
        _Stepper1 ("Stepper 1", Float) = 0.2
        _Stepper2 ("Stepper 2", Float) = 0.4
        _Stepper3 ("Stepper 3", Float) = 0.6
        _Stepper4 ("Stepper 4", Float) = 0.8

        _DissolveAmount ("Dissolve Amount", Range(0, 1)) = 0
        _DissolveNoise ("Dissolve Noise", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        ZWrite Off
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #define RANDOM1 12.9898
            #define RANDOM2 78.233
            #define RANDOM3 43758.54531213
            
            #include "UnityCG.cginc"

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

            float4 _Color1, _Color2, _Color3, _Color4, _Color5;
            float _NoiseScale, _TimeScale, _Stepper1, _Stepper2, _Stepper3, _Stepper4, _GradientScale, _DissolveAmount, _DissolveNoise;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float random(float2 st)
            {
                float randomVal = frac(sin(dot(st.xy, float2(RANDOM1, RANDOM2))) * RANDOM3);
                return  randomVal;
            }
            
            float simple_Noise(float2 st)
            {
                float2 i = floor(st);
                float2 f = frac(st);

                float a = random(i);
                float b = random(i + float2(1.0, 0.0));
                float c = random(i + float2(0.0, 1.0));
                float d = random(i + float2(1.0, 1.0));

                float2 u = f * f * (3.0 - 2.0 * f);
                return lerp(lerp(a, b, u.x), lerp(c, d, u.x), u.y);
            }

            float4 frag (v2f i) : SV_Target
            {
                float noise = simple_Noise(float2(i.uv.x * _NoiseScale, i.uv.y * _NoiseScale + _Time.y * _TimeScale));

                float gradientY = i.uv.y * _GradientScale;
                float step1 = step(noise, gradientY);
                float step2 = step(noise, gradientY - _Stepper1);
                float step3 = step(noise, gradientY - _Stepper2);
                float step4 = step(noise, gradientY - _Stepper3);
                float step5 = step(noise, gradientY - _Stepper4);

                
                //clipping
                float dissolveNoise = simple_Noise(i.uv * _DissolveNoise);
                float dissolve = dissolveNoise - _DissolveAmount;
                clip(dissolve);
                
                float4 lerped = lerp(_Color1, _Color2, step1 - step2);
                float4 lerped2 = lerp(lerped, _Color3, step2 - step3);
                float4 lerped3 = lerp(lerped2, _Color4, step3 - step4);
                float4 lerped4 = lerp(lerped3, _Color5, step4 - step5);
                
                return lerped4;
            }
            ENDCG
        }
    }
    Fallback "Diffuse"
}
