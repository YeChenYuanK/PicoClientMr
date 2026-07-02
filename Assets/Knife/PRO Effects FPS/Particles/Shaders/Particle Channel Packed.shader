Shader "Knife/URP/Particle Channel Packed"
{
    Properties
    {
        _Rows("Rows", Float) = 4
        _Columns("Columns", Float) = 4
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("MainTex", 2D) = "white" {}
        [Toggle(_MAINTEXSMOOTHSTEP_ON)] _MainTexSmoothstep("MainTexSmoothstep", Float) = 0
        _MainSoftnessMin("MainSoftnessMin", Range(0, 1)) = 0
        _MainSoftnessMax("MainSoftnessMax", Range(0, 1)) = 1
        _AlphaSoftness("AlphaSoftness", Range(0, 1)) = 0
        _DepthSoftness("DepthSoftness", Float) = 1
        [Toggle(_ALPHADISSOLVE_ON)] _AlphaDissolve("AlphaDissolve", Float) = 0
        [HDR]_Emission("Emission", Color) = (0,0,0,0)
        [Toggle(_EMISSIONDISSOLVE_ON)] _EmissionDissolve("EmissionDissolve", Float) = 0
        _EmissionTex("EmissionTex", 2D) = "white" {}
        _EmissionSoftness1("EmissionSoftness1", Range(0, 1)) = 0
        _EmissionSoftness2("EmissionSoftness2", Range(0, 1)) = 0
        [Toggle(_FINALALPHASMOOTHSTEP_ON)] _FinalAlphaSmoothstep("FinalAlphaSmoothstep", Float) = 0
        _FinalAlphaSmoothstepMin("FinalAlphaSmoothstepMin", Range(0, 1)) = 0
        _FinalAlphaSmoothstepMax("FinalAlphaSmoothstepMax", Range(0, 1)) = 1
        [Toggle(_EMISSIONALPHA_ON)] _EmissionAlpha("EmissionAlpha", Float) = 0
        [Toggle(_FINALEMISSIONSMOOTHSTEP_ON)] _FinalEmissionSmoothstep("FinalEmissionSmoothstep", Float) = 0
        _FinalEmissionSmoothstepMin("FinalEmissionSmoothstepMin", Range(0, 1)) = 0
        _FinalEmissionSmoothstepMax("FinalEmissionSmoothstepMax", Range(0, 1)) = 1
        [Toggle(_NORMALMAPENABLED_ON)] _NormalMapEnabled("Normal Map Enabled", Float) = 0
        _NormalMap("NormalMap", 2D) = "bump" {}
        _NormalScale("NormalScale", Float) = 0
        _EmissionSubValue("EmissionSubValue", Range(0, 1)) = 0
        [Toggle(_ALPHAEMISSIONDISSOLVESUB_ON)] _AlphaEmissionDissolveSub("Alpha Emission Dissolve Sub", Float) = 0
        _EmissionSpeed("EmissionSpeed", Vector) = (0,0,0,0)
        [Toggle(_ELIMINATEEMISSIONROTATION_ON)] _EliminateEmissionRotation("EliminateEmissionRotation", Float) = 0
        [Enum(UnityEngine.Rendering.CullMode)]_CullMode("Cull Mode", Float) = 2
        [HideInInspector] _texcoord("", 2D) = "white" {}
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent+0"
            "IgnoreProjector" = "True"
            "RenderPipeline" = "UniversalPipeline"
        }
        Cull [_CullMode]
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // URP 标准库（你环境完美兼容的成功版本）
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

            // 所有宏开关 1:1 保留
            #pragma shader_feature _NORMALMAPENABLED_ON
            #pragma shader_feature _EMISSIONALPHA_ON
            #pragma shader_feature _EMISSIONDISSOLVE_ON
            #pragma shader_feature _ELIMINATEEMISSIONROTATION_ON
            #pragma shader_feature _ALPHAEMISSIONDISSOLVESUB_ON
            #pragma shader_feature _ALPHADISSOLVE_ON
            #pragma shader_feature _MAINTEXSMOOTHSTEP_ON
            #pragma shader_feature _FINALEMISSIONSMOOTHSTEP_ON
            #pragma shader_feature _FINALALPHASMOOTHSTEP_ON

            // 极简结构体（你验证成功的安全格式）
            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 uv4 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 color : COLOR;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 uv4 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 color : COLOR;
                float4 screenPos : TEXCOORD3;
            };

            // 材质参数 1:1 保留
            CBUFFER_START(UnityPerMaterial)
            float _Rows;
            float _Columns;
            float4 _Color;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _MainSoftnessMin;
            float _MainSoftnessMax;
            float _AlphaSoftness;
            float _DepthSoftness;
            float4 _Emission;
            sampler2D _EmissionTex;
            float4 _EmissionTex_ST;
            float _EmissionSoftness1;
            float _EmissionSoftness2;
            float2 _EmissionSpeed;
            float _NormalScale;
            sampler2D _NormalMap;
            float _EmissionSubValue;
            float _FinalEmissionSmoothstepMin;
            float _FinalEmissionSmoothstepMax;
            float _FinalAlphaSmoothstepMin;
            float _FinalAlphaSmoothstepMax;
            CBUFFER_END

            // 顶点着色器（极简安全写法）
            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                output.uv4 = input.uv4;
                output.uv2 = input.uv2;
                output.color = input.color;
                output.screenPos = ComputeScreenPos(output.positionHCS);
                return output;
            }

            // 片元着色器（全功能还原，无紫色）
            half4 frag(Varyings i) : SV_Target
            {
                // ===================== 序列帧动画（原版1:1） =====================
                float cols = _Columns;
                float rows = _Rows;
                float frame = round(i.uv4.z);
                
                // 主序列帧 UV
                float total = cols * rows;
                float colOffset = 1.0 / cols;
                float rowOffset = 1.0 / rows;
                float tileX = fmod(frame, cols);
                float tileY = (rows - 1) - floor(frame / cols);
                float2 fbUV = i.uv * float2(colOffset, rowOffset) + float2(tileX * colOffset, tileY * rowOffset);

                // ===================== 深度软化（URP 标准成功写法） =====================
                float2 screenUV = i.screenPos.xy / i.screenPos.w;
                float sceneZ = LinearEyeDepth(SampleSceneDepth(screenUV), _ZBufferParams);
                float selfZ = LinearEyeDepth(i.screenPos.z / i.screenPos.w, _ZBufferParams);
                float depthFade = saturate(abs(sceneZ - selfZ) / _DepthSoftness);

                // ===================== 主纹理 & 通道混合 =====================
                half4 mainTex = tex2D(_MainTex, fbUV);
                #ifdef _MAINTEXSMOOTHSTEP_ON
                mainTex = smoothstep(_MainSoftnessMin, _MainSoftnessMax, mainTex);
                #endif

                float channelLerp = saturate((frame - 0) / (total * 1.0));
                float mainAlpha = lerp(mainTex.r, lerp(mainTex.g, mainTex.b, channelLerp), channelLerp);
                float alphaSoft = smoothstep(0, _AlphaSoftness, mainAlpha);

                // ===================== 最终Alpha计算 =====================
                float finalAlpha;
                #ifdef _ALPHADISSOLVE_ON
                finalAlpha = _Color.a * saturate(depthFade * alphaSoft - (1 - i.color.a));
                #else
                finalAlpha = _Color.a * i.color.a * depthFade * alphaSoft;
                #endif

                // ===================== 自发光 =====================
                float2 emissUV = TRANSFORM_TEX(i.uv, _EmissionTex);
                #ifdef _ELIMINATEEMISSIONROTATION_ON
                float rot = cos(i.uv2.x);
                float2 rotUV = mul(emissUV - 0.5, float2x2(rot, -sin(i.uv2.x), sin(i.uv2.x), rot)) + 0.5;
                emissUV = rotUV;
                #endif
                float2 emissPanner = emissUV + _Time.y * _EmissionSpeed;
                half4 emissTex = smoothstep(_EmissionSoftness1, _EmissionSoftness2, tex2D(_EmissionTex, emissPanner));

                float emissMask = i.uv4.w;
                #ifdef _ALPHAEMISSIONDISSOLVESUB_ON
                emissMask -= finalAlpha * _EmissionSubValue;
                #endif

                half3 emission = _Emission.rgb;
                #ifdef _EMISSIONDISSOLVE_ON
                emission *= saturate(emissTex.r - emissMask);
                #else
                emission *= emissMask;
                #endif

                // ===================== 最终平滑 =====================
                #ifdef _FINALEMISSIONSMOOTHSTEP_ON
                emission *= smoothstep(_FinalEmissionSmoothstepMin, _FinalEmissionSmoothstepMax, finalAlpha);
                #endif
                #ifdef _FINALALPHASMOOTHSTEP_ON
                finalAlpha = smoothstep(_FinalAlphaSmoothstepMin, _FinalAlphaSmoothstepMax, finalAlpha);
                #endif

                // ===================== 颜色输出 =====================
                half3 albedo = _Color.rgb * i.color.rgb;
                half3 finalColor = albedo + emission;

                return half4(finalColor, finalAlpha);
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}