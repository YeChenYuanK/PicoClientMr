Shader "Knife/URP/Fire"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Noise("Noise", 2D) = "white" {}
        _Alpha("Alpha", 2D) = "white" {}
        [HDR]_Color0("Color 0", Color) = (1,1,1,1)
        [HDR]_Color1("Color 1", Color) = (1,1,1,1)
        _Opacity("Opacity", Range( 0 , 1)) = 1
        _NoiseSoftness("NoiseSoftness", Range( 0 , 1)) = 0
        _NoiseSpeed("NoiseSpeed", Vector) = (0,1,0,0)
        _Rotation("Rotation", Float) = 1
        _Offset("Offset", Vector) = (1,0,0,0)
        _AlphaSoftness("AlphaSoftness", Range( 0 , 1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "RenderPipeline"="UniversalPipeline" "Queue"="Transparent" }
        LOD 100
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float4 color : COLOR;
            };

            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float4 positionHCS : SV_POSITION;
                float4 vertexColor : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _Noise;
            float4 _Noise_ST;
            sampler2D _Alpha;
            float4 _Alpha_ST;
            float4 _Color0;
            float4 _Color1;
            float _Opacity;
            float _NoiseSoftness;
            float2 _NoiseSpeed;
            float _Rotation;
            float2 _Offset;
            float _AlphaSoftness;

            Varyings vert (Attributes input)
            {
                Varyings output;
                output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                output.uv1 = input.uv1;
                output.vertexColor = input.color;
                return output;
            }

            half4 frag (Varyings input) : SV_Target
            {
                // Ðý×ªUV
                float c = cos(input.uv.y * _Rotation);
                float s = sin(input.uv.y * _Rotation);
                float2 uvNoise = TRANSFORM_TEX(input.uv, _Noise);
                float2 uvRot = mul((uvNoise + input.uv1 * _Offset) - 0.5, float2x2(c,-s,s,c)) + 0.5;
                
                // ÔëÉù¹ö¶¯
                float2 pan = _Time.y * _NoiseSpeed + uvRot;
                half noise = tex2D(_Noise, pan).r;
                
                // ÑÕÉ«»ìºÏ
                half noiseSmooth = smoothstep(0.0, _NoiseSoftness, noise);
                half4 col = lerp(_Color0, _Color1, noiseSmooth);
                
                // AlphaÕÚÕÖ
                half alphaTex = tex2D(_Alpha, TRANSFORM_TEX(input.uv, _Alpha)).r;
                half alphaSmooth = smoothstep(0.0, _AlphaSoftness, alphaTex);
                
                // »ðÑæ±ßÔµ
                half edge = alphaSmooth - smoothstep(0.0, 1.0, noise);
                half finalAlpha = alphaSmooth * _Opacity * edge * input.vertexColor.a;
                
                col.a = finalAlpha;
                col.rgb *= input.vertexColor.rgb;

                return col;
            }
            ENDHLSL
        }
    }
}