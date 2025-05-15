Shader "Custom/WaterRipplePBR"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (0,0.5,1,1)
        _RippleColor ("Ripple Color", Color) = (1,1,1,1)
        _RippleCenter ("Ripple Center (UV)", Vector) = (0.5,0.5,0,0)
        _RippleTime ("Ripple Time", Float) = 0
        _RippleRadius ("Ripple Radius", Float) = 0.2
        _RippleThickness ("Ripple Thickness", Float) = 0.03
        _RippleStrength ("Ripple Strength", Float) = 1

        _MetallicMap ("Metallic Map", 2D) = "black" {}
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Smoothness ("Smoothness", Range(0,1)) = 0.5
        _UseMetallicMap ("Use Metallic Map", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 200

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
            #pragma multi_compile_fragment _ _LIGHT_LAYERS
            #pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
            #pragma multi_compile_fragment _ _REFLECTION_PROBE_BOX_PROJECTION

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 normalWS : TEXCOORD1;
                float2 uv : TEXCOORD0;
                float3 positionWS : TEXCOORD2;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float4 _RippleColor;
                float4 _RippleCenter;
                float _RippleTime;
                float _RippleRadius;
                float _RippleThickness;
                float _RippleStrength;
                float _Metallic;
                float _Smoothness;
                float _UseMetallicMap;
            CBUFFER_END

            TEXTURE2D(_MetallicMap); SAMPLER(sampler_MetallicMap);

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
                o.normalWS = TransformObjectToWorldNormal(v.normalOS);
                o.uv = v.uv;
                o.positionWS = TransformObjectToWorld(v.positionOS.xyz);
                return o;
            }

            void InitializeSurfaceData(Varyings i, out SurfaceData surfaceData)
            {
                float2 uv = i.uv;
                float2 center = _RippleCenter.xy;
                float dist = distance(uv, center);

                // Волна
                float ripplePos = _RippleTime;
                float ring = smoothstep(_RippleThickness, 0.0, abs(dist - ripplePos * _RippleRadius));
                float fade = saturate(1.0 - _RippleTime);
                float alpha = ring * _RippleStrength * fade;

                float3 baseColor = lerp(_BaseColor.rgb, _RippleColor.rgb, alpha);

                // Metallic & Smoothness
                float4 metallicSample = SAMPLE_TEXTURE2D(_MetallicMap, sampler_MetallicMap, uv);
                float metallic = lerp(_Metallic, metallicSample.r, _UseMetallicMap);
                float smoothness = _Smoothness;

                surfaceData = (SurfaceData)0;
                surfaceData.albedo = baseColor;
                surfaceData.metallic = metallic;
                surfaceData.smoothness = smoothness;
                surfaceData.normalTS = float3(0, 0, 1);
                surfaceData.occlusion = 1.0;
                surfaceData.emission = 0;
                surfaceData.alpha = 1.0;
            }

            void InitializeInputData(Varyings i, out InputData inputData)
            {
                inputData = (InputData)0;
                inputData.positionWS = i.positionWS;
                inputData.normalWS = normalize(i.normalWS);
                inputData.viewDirectionWS = normalize(_WorldSpaceCameraPos - i.positionWS);
                inputData.shadowCoord = 0;
                #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
                    inputData.shadowCoord = i.positionCS;
                #endif
                inputData.fogCoord = 0;
                inputData.vertexLighting = 0;
                inputData.bakedGI = 0;
                inputData.normalizedScreenSpaceUV = 0;
                inputData.shadowMask = 1;
            }

            float4 frag (Varyings i) : SV_Target
            {
                SurfaceData surfaceData;
                InitializeSurfaceData(i, surfaceData);

                InputData inputData;
                InitializeInputData(i, inputData);

                float4 color = UniversalFragmentPBR(inputData, surfaceData);
                return color;
            }
            ENDHLSL
        }
    }
}