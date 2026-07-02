Shader "Knife/URP/Particle Channel Packed Unlit"
{
    Properties
    {
        _Rows("Rows", Float) = 4
        _Columns("Columns", Float) = 4
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("MainTex", 2D) = "white" {}
        [Toggle(_MAINTEXSMOOTHSTEP_ON)] _MainTexSmoothstep("MainTexSmoothstep", Float) = 0
        _MainSoftnessMin("MainSoftnessMin", Range(0,1)) = 0
        _MainSoftnessMax("MainSoftnessMax", Range(0,1)) = 1
        _DepthSoftness("DepthSoftness", Float) = 1
        [Toggle(_ALPHADISSOLVE_ON)] _AlphaDissolve("AlphaDissolve", Float) = 0
        [HDR]_Emission("Emission", Color) = (0,0,0,0)
        [Toggle(_EMISSIONDISSOLVE_ON)] _EmissionDissolve("EmissionDissolve", Float) = 0
        _EmissionTex("EmissionTex", 2D) = "white" {}
        _EmissionSpeed("EmissionSpeed", Vector) = (0,0,0,0)
        _EmissionSoftness1("EmissionSoftness1", Range(0,1)) = 0
        _EmissionSoftness2("EmissionSoftness2", Range(0,1)) = 0
        [Toggle(_FINALALPHASMOOTHSTEP_ON)] _FinalAlphaSmoothstep("FinalAlphaSmoothstep", Float) = 0
        _FinalAlphaSmoothstepMin("FinalAlphaSmoothstepMin", Range(0,1)) = 0
        _FinalAlphaSmoothstepMax("FinalAlphaSmoothstepMax", Range(0,1)) = 1
        [Toggle(_EMISSIONALPHA_ON)] _EmissionAlpha("EmissionAlpha", Float) = 0
        [Toggle(_FINALEMISSIONSMOOTHSTEP_ON)] _FinalEmissionSmoothstep("FinalEmissionSmoothstep", Float) = 0
        _FinalEmissionSmoothstepMin("FinalEmissionSmoothstepMin", Range(0,1)) = 0
        _FinalEmissionSmoothstepMax("FinalEmissionSmoothstepMax", Range(0,1)) = 1
        _EmissionSubValue("EmissionSubValue", Range(0,1)) = 0
        [Toggle(_ALPHAEMISSIONDISSOLVESUB_ON)] _AlphaEmissionDissolveSub("Alpha Emission Dissolve Sub", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "Queue"="Transparent"
            "RenderPipeline"="UniversalPipeline"
            "IgnoreProjector"="True"
        }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Back
        ZWrite Off
        ZTest LEqual
        ColorMask RGBA

        Pass
        {
            Name "Unlit"
            Tags { "LightMode"="SRPDefaultUnlit" }
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            
            #pragma shader_feature _EMISSIONALPHA_ON
            #pragma shader_feature _EMISSIONDISSOLVE_ON
            #pragma shader_feature _ALPHAEMISSIONDISSOLVESUB_ON
            #pragma shader_feature _ALPHADISSOLVE_ON
            #pragma shader_feature _MAINTEXSMOOTHSTEP_ON
            #pragma shader_feature _FINALEMISSIONSMOOTHSTEP_ON
            #pragma shader_feature _FINALALPHASMOOTHSTEP_ON

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float4 color        : COLOR;
                float4 texcoord     : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
                float4 color        : COLOR;
                float4 texcoord     : TEXCOORD0;
                float4 screenPos    : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            CBUFFER_START(UnityPerMaterial)
                float _Rows;
                float _Columns;
                float4 _Color;
                sampler2D _MainTex;
                float4 _MainTex_ST;
                float _MainSoftnessMin;
                float _MainSoftnessMax;
                float _DepthSoftness;
                float4 _Emission;
                sampler2D _EmissionTex;
                float4 _EmissionTex_ST;
                float2 _EmissionSpeed;
                float _EmissionSoftness1;
                float _EmissionSoftness2;
                float _EmissionSubValue;
                float _FinalAlphaSmoothstepMin;
                float _FinalAlphaSmoothstepMax;
                float _FinalEmissionSmoothstepMin;
                float _FinalEmissionSmoothstepMax;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);
                output.color = input.color;
                output.texcoord = input.texcoord;
                output.screenPos = ComputeScreenPos(output.positionHCS);
                
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                float4 uv = input.texcoord;
                float4 vertexColor = input.color;

                // 深度软化
                float2 screenUV = input.screenPos.xy / input.screenPos.w;
                float sceneZ = LinearEyeDepth(SampleSceneDepth(screenUV), _ZBufferParams);
                float selfZ = LinearEyeDepth(input.screenPos.z / input.screenPos.w, _ZBufferParams);
                float depthFade = saturate(abs(sceneZ - selfZ) / _DepthSoftness);

                // 序列帧动画
                float cols = _Columns;
                float rows = _Rows;
                float frame = round(uv.z);
                float totalFrames = cols * rows;
                float colOffset = 1.0 / cols;
                float rowOffset = 1.0 / rows;
                float tileX = fmod(frame, cols);
                float tileY = (rows - 1) - floor(frame / cols);
                float2 flipbookUV = uv.xy * float2(colOffset, rowOffset) + float2(tileX * colOffset, tileY * rowOffset);

                half4 mainTex = tex2D(_MainTex, flipbookUV);
                #ifdef _MAINTEXSMOOTHSTEP_ON
                mainTex = smoothstep(_MainSoftnessMin, _MainSoftnessMax, mainTex);
                #endif

                // 通道混合
                float channelLerp = saturate(frame / totalFrames);
                float alphaSource = lerp(mainTex.r, lerp(mainTex.g, mainTex.b, channelLerp), channelLerp);

                // 透明度计算
                float finalAlpha;
                #ifdef _ALPHADISSOLVE_ON
                finalAlpha = _Color.a * saturate(depthFade * alphaSource - (1 - vertexColor.a));
                #else
                finalAlpha = _Color.a * vertexColor.a * depthFade * alphaSource;
                #endif

                // 自发光
                float2 emissUV = TRANSFORM_TEX(uv.xy, _EmissionTex);
                float2 emissPanner = emissUV + _Time.y * _EmissionSpeed;
                half4 emissTex = smoothstep(_EmissionSoftness1, _EmissionSoftness2, tex2D(_EmissionTex, emissPanner));

                float emissMask = uv.w;
                #ifdef _ALPHAEMISSIONDISSOLVESUB_ON
                emissMask -= finalAlpha * _EmissionSubValue;
                #endif

                half3 emissColor = _Emission.rgb;
                #ifdef _EMISSIONDISSOLVE_ON
                emissColor *= saturate(emissTex.r - emissMask);
                #else
                emissColor *= emissMask;
                #endif

                // 最终平滑
                #ifdef _FINALEMISSIONSMOOTHSTEP_ON
                emissColor *= smoothstep(_FinalEmissionSmoothstepMin, _FinalEmissionSmoothstepMax, finalAlpha);
                #endif
                #ifdef _FINALALPHASMOOTHSTEP_ON
                finalAlpha = smoothstep(_FinalAlphaSmoothstepMin, _FinalAlphaSmoothstepMax, finalAlpha);
                #endif

                #ifdef _EMISSIONALPHA_ON
                emissColor *= finalAlpha;
                #endif

                // 最终颜色
                half3 albedo = _Color.rgb * vertexColor.rgb;
                half3 finalRGB = albedo + emissColor;
                
                return half4(finalRGB, finalAlpha);
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}