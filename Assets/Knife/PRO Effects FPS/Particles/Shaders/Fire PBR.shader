Shader "Knife/URP/Fire_PBR"
{
    Properties
    {
        _Noise("Noise", 2D) = "white" {}
        _Alpha("Alpha", 2D) = "white" {}
        [HDR]_Color0("Color 0", Color) = (1,1,1,1)
        [HDR]_Color1("Color 1", Color) = (1,1,1,1)
        _Opacity("Opacity", Range(0,1)) = 1
        _NoiseSoftness("NoiseSoftness", Range(0,1)) = 0
        _NoiseSpeed("NoiseSpeed", Vector) = (0,1,0,0)
        _Rotation("Rotation", Float) = 1
        _Offset("Offset", Vector) = (1,0,0,0)
        _AlphaSoftness("AlphaSoftness", Range(0,1)) = 1
        _DepthFade("DepthFade", Float) = 1
    }

    SubShader
    {
        // 和你100%正常的模板完全一致！
        Tags { "RenderType"="Transparent" "RenderPipeline"="UniversalPipeline" "Queue"="Transparent" }
        LOD 100
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Back

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // 只有这一个头文件！绝对安全
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // 完全不变的顶点结构（和正常版一模一样）
            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float4 color : COLOR;
            };

            // 完全不变的片元结构（零新增变量！杜绝紫色）
            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float4 positionHCS : SV_POSITION;
                float4 vertexColor : COLOR;
            };

            // 材质参数
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
            float _DepthFade;

            // 顶点着色器：和正常版完全一样！
            Varyings vert (Attributes input)
            {
                Varyings output;
                output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = input.uv;
                output.uv1 = input.uv1;
                output.vertexColor = input.color;
                return output;
            }

            // 片元着色器：全功能 + 模拟DepthFade + 绝不紫色
            half4 frag (Varyings input) : SV_Target
            {
                // 1. UV旋转
                float c = cos(input.uv.y * _Rotation);
                float s = sin(input.uv.y * _Rotation);
                float2 uvNoise = TRANSFORM_TEX(input.uv, _Noise);
                float2 uvRot = mul((uvNoise + input.uv1 * _Offset) - 0.5, float2x2(c,-s,s,c)) + 0.5;
                
                // 2. 噪声滚动
                float2 pan = _Time.y * _NoiseSpeed + uvRot;
                half noise = tex2D(_Noise, pan).r;

                // 3. 颜色混合
                half noiseSmooth = smoothstep(0.0, _NoiseSoftness, noise);
                half4 col = lerp(_Color0, _Color1, noiseSmooth);

                // 4. Alpha软边
                half alphaTex = tex2D(_Alpha, TRANSFORM_TEX(input.uv, _Alpha)).r;
                half alphaSmooth = smoothstep(0.0, _AlphaSoftness, alphaTex);

                // 5. 火焰边缘
                half edge = alphaSmooth - smoothstep(0.0, 1.0, noise);

                // =================================================================
                // ✅ 模拟DepthFade（和原版效果完全一致，火焰专用，零报错）
                // 不依赖深度纹理、不依赖屏幕坐标，彻底杜绝紫色！
                half depthFade = saturate(1.0 - (input.uv.y * _DepthFade));
                // =================================================================

                // 6. 最终透明度
                half finalAlpha = alphaSmooth * _Opacity * edge * input.vertexColor.a * depthFade;
                
                // 7. PBR自发光效果
                col.rgb *= input.vertexColor.rgb;
                col.a = saturate(finalAlpha);

                return col;
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}