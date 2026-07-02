Shader "Knife/URP/Distortion_FinalSafe"
{
    Properties
    {
        _NormalMap("NormalMap", 2D) = "bump" {}
        _NormalMap2("NormalMap2", 2D) = "bump" {}
        _AlphaMask("AlphaMask", 2D) = "white" {}
        _DistortionAmount("DistortionAmount", Float) = 3
        _DistortionAmount2("DistortionAmount2", Float) = 2
        _DistortionSpeed("DistortionSpeed", Vector) = (0.1,0.1,0,0)
        _DistortionSpeed2("DistortionSpeed2", Vector) = (0.05,0.08,0,0)
        _Tiling1("Tiling1", Float) = 1
        _Tiling2("Tiling2", Float) = 1
        [Toggle]_TwoNormals("TwoNormals", Float) = 0
        [Toggle]_Debug("Debug", Float) = 0
        [Toggle]_ScreenSpaceUV("ScreenSpaceUV", Float) = 0
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "RenderPipeline"="UniversalPipeline"
            "Queue"="Transparent"
        }
        LOD 100
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Back

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
                float4 color : COLOR;
            };

            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float4 positionHCS : SV_POSITION;
                float4 screenPos : TEXCOORD1;
                float4 vertexColor : COLOR;
            };

            sampler2D _NormalMap;
            float4 _NormalMap_ST;
            sampler2D _NormalMap2;
            float4 _NormalMap2_ST;
            sampler2D _AlphaMask;
            float4 _AlphaMask_ST;
            
            float _DistortionAmount;
            float _DistortionAmount2;
            float2 _DistortionSpeed;
            float2 _DistortionSpeed2;
            float _Tiling1;
            float _Tiling2;
            float _TwoNormals;
            float _Debug;
            float _ScreenSpaceUV;

            Varyings vert (Attributes input)
            {
                Varyings output;
                output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = input.uv;
                output.screenPos = ComputeScreenPos(output.positionHCS);
                output.vertexColor = input.color;
                return output;
            }

            half4 frag (Varyings input) : SV_Target
            {
                float2 screenUV = input.screenPos.xy / input.screenPos.w;
                float2 uvMask = TRANSFORM_TEX(input.uv, _AlphaMask);
                half4 mask = tex2D(_AlphaMask, uvMask);
                
                float2 uv1 = TRANSFORM_TEX(input.uv, _NormalMap);
                float2 uv2 = TRANSFORM_TEX(input.uv, _NormalMap2);
                
                if (_ScreenSpaceUV > 0.5)
                {
                    uv1 = screenUV * _Tiling1;
                    uv2 = screenUV * _Tiling2;
                }
                else
                {
                    uv1 *= _Tiling1;
                    uv2 *= _Tiling2;
                }

                float2 pan1 = _Time.y * _DistortionSpeed + uv1;
                float2 pan2 = _Time.y * _DistortionSpeed2 + uv2;

                half3 n1 = UnpackNormal(tex2D(_NormalMap, pan1));
                float strength1 = _DistortionAmount * input.vertexColor.a * mask.r * 0.01;
                n1 *= strength1;

                half3 n2 = half3(0,0,0);
                if (_TwoNormals > 0.5)
                {
                    n2 = UnpackNormal(tex2D(_NormalMap2, pan2));
                    float strength2 = _DistortionAmount2 * input.vertexColor.a * mask.r * 0.01;
                    n2 *= strength2;
                }

                half3 finalNormal = normalize(n1 + n2);
                float2 distortUV = screenUV + finalNormal.xy;

                half4 col;
                if (_Debug > 0.5)
                {
                    col = mask;
                }
                else
                {
                    col = tex2D(_AlphaMask, distortUV);
                }

                col.a = mask.a * input.vertexColor.a;
                return col;
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}