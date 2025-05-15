Shader "Custom/URP_Hologram"
{
    Properties
    {
        _Color ("Color", Color) = (0,1,1,0.5)
        _MainTex ("Texture", 2D) = "white" {}
        _FresnelPower ("Fresnel Power", Range(1,10)) = 5
        _LineSpeed ("Line Speed", Float) = 2
        _LineDensity ("Line Density", Float) = 20
        _Emission ("Emission", Float) = 2
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldNormal : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float2 uv : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _FresnelPower;
            float _LineSpeed;
            float _LineDensity;
            float _Emission;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = TransformObjectToHClip(v.vertex.xyz);
                o.worldNormal = TransformObjectToWorldNormal(v.normal);
                o.worldPos = TransformObjectToWorld(v.vertex.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
                float fresnel = pow(1.0 - saturate(dot(i.worldNormal, viewDir)), _FresnelPower);
                float scanLine = sin(i.worldPos.y * _LineDensity + _Time.y * _LineSpeed) * 0.5 + 0.5;

                float alpha = _Color.a * (0.5 + 0.5 * fresnel) * (0.7 + 0.3 * scanLine);

                float3 col = _Color.rgb * (1 + fresnel * 0.5 + scanLine * 0.5);

                return half4(col * _Emission, alpha);
            }
            ENDHLSL
        }
    }
}