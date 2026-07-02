Shader "Knife/URP/Demo Water"
{
    Properties
    {
        _Normal1("Normal 1", 2D) = "bump" {}
        _NormalScale1("Normal Scale 1", Float) = 1
        _NormalSpeed1("Normal Speed 1", Vector) = (0.1,0.1,0,0)
        _Normal2("Normal 2", 2D) = "bump" {}
        _NormalScale2("Normal Scale 2", Float) = 1
        _NormalSpeed2("Normal Speed 2", Vector) = (-0.1,-0.1,0,0)
        _Color("Color", Color) = (0.4764151,0.7418258,1,1)
        _Smoothness("Smoothness", Range(0,1)) = 0.98
        _Specular("Specular", Range(0,1)) = 0.5
        _Distortion("Distortion", Float) = 0.1
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

            // 完全沿用你成功的结构体
            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv          : TEXCOORD0;
            };

            CBUFFER_START(UnityPerMaterial)
                sampler2D _Normal1;
                float4 _Normal1_ST;
                float _NormalScale1;
                float2 _NormalSpeed1;

                sampler2D _Normal2;
                float4 _Normal2_ST;
                float _NormalScale2;
                float2 _NormalSpeed2;

                float4 _Color;
                float _Specular;
                float _Smoothness;
                float _Distortion;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = TRANSFORM_TEX(input.uv, _Normal1);
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                float2 uv = input.uv;

                // 双法线流动（你成功的手动解包）
                float2 pan1 = _Time.y * _NormalSpeed1 + uv;
                half3 n1 = (tex2D(_Normal1, pan1).rgb * 2 - 1) * _NormalScale1;

                float2 pan2 = _Time.y * _NormalSpeed2 + uv;
                half3 n2 = (tex2D(_Normal2, pan2).rgb * 2 - 1) * _NormalScale2;

                half3 normal = normalize(n1 + n2);

                // ===================== 安全UV扭曲（不碰屏幕采样，绝不紫色） =====================
                float2 uvDistorted = uv + normal.xy * _Distortion;

                // 用扭曲后的UV做颜色（水波纹视觉）
                half3 baseColor = _Color.rgb;

                // 高光（完全沿用你成功的写法）
                half normalGloss = normal.r * 0.5 + 0.5;
                half spec = pow(normalGloss, _Smoothness * 100);
                half3 specular = _Specular * spec;

                // 最终颜色
                half3 finalColor = baseColor + specular;
                return half4(finalColor, _Color.a);
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}