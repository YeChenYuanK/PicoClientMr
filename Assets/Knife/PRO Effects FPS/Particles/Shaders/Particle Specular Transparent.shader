Shader "Knife/URP/Particle Specular Transparent"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("MainTex", 2D) = "white" {}
        [NoScaleOffset]_NormalMap("NormalMap", 2D) = "bump" {}
        _NormalScale("NormalScale", Float) = 1
        [NoScaleOffset]_Specular("Specular", 2D) = "white" {}
        _Smoothness("Smoothness", Range(0, 1)) = 1
        _SpecularColor("SpecularColor", Color) = (0,0,0,0)
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "Queue"="Transparent+0"
            "IgnoreProjector"="True"
            "RenderPipeline"="UniversalPipeline"
        }
        Cull Back
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Tags { "LightMode"="SRPDefaultUnlit" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // 唯一安全引用
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // 永久铁律：极简结构体（和成功版本完全一致）
            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv         : TEXCOORD0;
                float4 color      : COLOR;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv          : TEXCOORD0;
                float4 color       : COLOR;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                sampler2D _MainTex;
                float4 _MainTex_ST;
                sampler2D _NormalMap;
                float _NormalScale;
                sampler2D _Specular;
                float _Smoothness;
                float4 _SpecularColor;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                output.color = input.color;
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                // 基础纹理采样
                half4 mainTex = tex2D(_MainTex, input.uv);
                half4 col = input.color * _Color * mainTex;

                // ===================== 成功版：手动法线解包（无紫色） =====================
                half3 normalTex = tex2D(_NormalMap, input.uv).rgb;
                half3 normal = (normalTex * 2 - 1) * _NormalScale;
                half normalGloss = normal.r * 0.5 + 0.5;

                // ===================== 高光计算（完整保留） =====================
                half4 specTex = tex2D(_Specular, input.uv);
                half spec = pow(normalGloss, _Smoothness * 100);
                half3 specular = (specTex.rgb + _SpecularColor.rgb) * spec;

                // 最终颜色 + 透明通道（原版1:1）
                half3 finalRGB = col.rgb + specular;
                return half4(finalRGB, col.a);
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}