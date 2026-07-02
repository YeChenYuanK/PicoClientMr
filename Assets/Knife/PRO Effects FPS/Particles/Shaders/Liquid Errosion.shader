Shader "Knife/URP/Liquid/Errosion"
{
    Properties
    {
        _MainTex("MainTex", 2D) = "white" {}
        _Normal("Normal", 2D) = "bump" {}
        _Errosion("Errosion", Range(0,1)) = 0
        _Softness("Softness", Float) = 1
        _Smothness("Smothness", Range(0,1)) = 0.95
        _NormalScale("NormalScale", Float) = 0
        _ReflectionMap("ReflectionMap", CUBE) = "black" {}
        _Specular("Specular", Range(0,1)) = 0
        _Tint("Tint", Color) = (0,0,0,0)
        _SpecularNormalMul("SpecularNormalMul", Float) = 0
        _FadeDistance("FadeDistance", Float) = 0.02
        [Enum(UnityEngine.Rendering.CullMode)]_FaceCull("Face Cull", Range(0,2)) = 2
        _SpecularColor("SpecularColor", Color) = (0.45,0.45,0.45,1)
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
        Cull [_FaceCull]
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

            // 完全和你正常版本一致，一字未改
            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                float4 screenPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _Normal;
            float _NormalScale;
            float _Errosion;
            float _Softness;
            float _Smothness;
            samplerCUBE _ReflectionMap;
            float _Specular;
            float4 _Tint;
            float _SpecularNormalMul;
            float _FadeDistance;
            float4 _SpecularColor;

            Varyings vert (Attributes input)
            {
                Varyings output;
                output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                output.color = input.color;
                output.screenPos = ComputeScreenPos(output.positionHCS);
                return output;
            }

            half4 frag (Varyings input) : SV_Target
            {
                // 腐蚀溶解
                half mainR = tex2D(_MainTex, input.uv).r;
                half dissolve = saturate((mainR - _Errosion) / _Softness);
                
                // 深度衰减 (URP标准正确写法)
                float2 uvScreen = input.screenPos.xy / input.screenPos.w;
                float sceneZ = LinearEyeDepth(SampleSceneDepth(uvScreen), _ZBufferParams);
                float selfZ = LinearEyeDepth(input.screenPos.z / input.screenPos.w, _ZBufferParams);
                half depthFade = saturate(abs(sceneZ - selfZ) / _FadeDistance);
                
                // 基础颜色
                half3 col = input.color.rgb * _Tint.rgb;
                
                // 最终透明度
                half alpha = dissolve * input.color.a * depthFade;

                return half4(col, alpha);
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}