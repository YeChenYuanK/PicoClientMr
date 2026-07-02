Shader "Knife/URP/MuzzleFlash"
{
    Properties
    {
        _Noise("Noise", 2D) = "white" {}
        _Noise1("Noise1", 2D) = "white" {}
        _Alpha("Alpha", 2D) = "white" {}
        [HDR]_Color0("Color 0", Color) = (1,1,1,1)
        [HDR]_Color1("Color 1", Color) = (1,1,1,1)
        _Opacity("Opacity", Range(0,1)) = 1
        _NoiseSoftness1("NoiseSoftness1", Range(0,1)) = 0
        _NoiseSoftness2("NoiseSoftness2", Range(0,1)) = 0
        _NoiseSpeed1("NoiseSpeed1", Vector) = (0,1,0,0)
        _NoiseSpeed("NoiseSpeed", Vector) = (0,1,0,0)
        _DepthFade("DepthFade", Float) = 0
        _AlphaSoftness("AlphaSoftness", Range(0,1)) = 1
        [Normal]_Distortion("Distortion", 2D) = "bump" {}
        _DistortionAmount("DistortionAmount", Range(0,1)) = 0
        _DistortionDiff("DistortionDiff", Float) = 0
        _DistortionSpeed1("DistortionSpeed1", Vector) = (0,0,0,0)
        _DistortionSpeed2("DistortionSpeed2", Vector) = (0,0,0,0)
        _CenterFadeSize("CenterFadeSize", Range(-1,1)) = 0
        _CenterNoiseFadeSize("CenterNoiseFadeSize", Range(-1,1)) = 0
        _CenterNoiseFadeSoftness("CenterNoiseFadeSoftness", Range(0,1)) = 0
        _CenterFadeSoftness("CenterFadeSoftness", Range(0,1)) = 0
        _DissolveSoftness("DissolveSoftness", Range(0,1)) = 0
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderPipeline"="UniversalPipeline"
        }
        LOD 0
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Back
        ColorMask RGBA
        ZWrite Off
        ZTest LEqual
        Offset 0, 0

        Pass
        {
            Name "Unlit"
            Tags { "LightMode"="SRPDefaultUnlit" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing

            // 仅引用安全库（永久成功配置）
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

            // 铁律：极简结构体（无任何多余字段）
            struct Attributes
            {
                float4 positionOS   : POSITION;
                float4 texcoord     : TEXCOORD0;
                float4 color        : COLOR;
            };

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
                float4 texcoord     : TEXCOORD0;
                float4 screenPos    : TEXCOORD1;
                float4 color        : COLOR;
            };

            CBUFFER_START(UnityPerMaterial)
                sampler2D _Noise;
                float4 _Noise_ST;
                float2 _NoiseSpeed;
                sampler2D _Noise1;
                float4 _Noise1_ST;
                float2 _NoiseSpeed1;
                sampler2D _Alpha;
                float4 _Alpha_ST;
                sampler2D _Distortion;
                float4 _Distortion_ST;
                float4 _Color0;
                float4 _Color1;
                float _Opacity;
                float _NoiseSoftness1;
                float _NoiseSoftness2;
                float _DepthFade;
                float _AlphaSoftness;
                float _DistortionAmount;
                float _DistortionDiff;
                float2 _DistortionSpeed1;
                float2 _DistortionSpeed2;
                float _CenterFadeSize;
                float _CenterFadeSoftness;
                float _CenterNoiseFadeSize;
                float _CenterNoiseFadeSoftness;
                float _DissolveSoftness;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);
                output.texcoord = input.texcoord;
                output.screenPos = ComputeScreenPos(output.positionHCS);
                output.color = input.color;
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                float2 uv = input.texcoord.xy;
                float4 vertexColor = input.color;

                // 噪声纹理动画
                float2 uvNoise1 = TRANSFORM_TEX(uv, _Noise1);
                float2 panner80 = _Time.y * _NoiseSpeed1 + uvNoise1;
                float2 uvNoise = TRANSFORM_TEX(uv, _Noise);
                float2 panner24 = _Time.y * _NoiseSpeed + uvNoise;

                // 中心噪声衰减
                float2 uvCenter1 = uv - 0.5;
                float centerNoiseLen = length(uvCenter1 * 2);
                float centerNoiseFade = smoothstep(_CenterNoiseFadeSize, _CenterNoiseFadeSize + _CenterNoiseFadeSoftness, centerNoiseLen);
                float noiseLerp = lerp(0.0, (tex2D(_Noise1, panner80).r + tex2D(_Noise, panner24).r) * 0.5, centerNoiseFade);
                float noiseSmooth = smoothstep(_NoiseSoftness1, _NoiseSoftness2, noiseLerp);
                half3 finalColor = lerp(_Color0.rgb, _Color1.rgb, noiseSmooth);

                // 中心衰减
                float2 uvCenter2 = uv - 0.5;
                float centerLen = length(uvCenter2 * 2);
                float centerFade = smoothstep(_CenterFadeSize, _CenterFadeSize + _CenterFadeSoftness, centerLen);
                float distortionAmount = _DistortionAmount * centerFade;

                // 扭曲扰动（手动法线解包，永久不紫色）
                float2 uvDistort = TRANSFORM_TEX(uv, _Distortion);
                float2 panner107 = _Time.y * _DistortionSpeed1 + uvDistort;
                float2 panner108 = _Time.y * _DistortionSpeed2 + (uvDistort * _DistortionDiff);
                
                half3 distortTex1 = tex2D(_Distortion, panner107).rgb;
                half3 distort1 = (distortTex1 * 2 - 1) * distortionAmount;
                half3 distortTex2 = tex2D(_Distortion, panner108).rgb;
                half3 distort2 = (distortTex2 * 2 - 1) * distortionAmount;
                float2 distortionOffset = distort1.xy + distort2.xy;

                // Alpha 纹理采样
                float2 uvAlpha = TRANSFORM_TEX(uv, _Alpha) + distortionOffset;
                float alphaSmooth = smoothstep(0.0, _AlphaSoftness, tex2D(_Alpha, uvAlpha).r);

                // 深度淡出
                float2 screenUV = input.screenPos.xy / input.screenPos.w;
                float sceneZ = LinearEyeDepth(SampleSceneDepth(screenUV), _ZBufferParams);
                float selfZ = LinearEyeDepth(input.screenPos.z / input.screenPos.w, _ZBufferParams);
                float depthFade = saturate(abs(sceneZ - selfZ) / _DepthFade);

                // 透明度计算
                float alphaMask = saturate(alphaSmooth - noiseSmooth);
                float alpha = alphaSmooth * _Opacity * depthFade * alphaMask * vertexColor.a;

                // 溶解效果
                float2 uvDissolve = uv - 0.5;
                float dissolveBase = 1.0 - (length(uvDissolve.xy) * 2.0);
                float dissolveHide = smoothstep(0.0, _DissolveSoftness, dissolveBase + input.texcoord.w);
                float dissolveShow = smoothstep(0.0, _DissolveSoftness, dissolveBase + input.texcoord.z);
                float finalDissolve = saturate(dissolveHide + (1.0 - dissolveShow));
                alpha = saturate(alpha - finalDissolve);

                // 最终颜色
                finalColor *= _Color0.rgb * vertexColor.rgb;
                return half4(finalColor, alpha);
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}