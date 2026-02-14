Shader "Custom/SlopeSnow"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _SnowTex ("Snow Texture", 2D) = "white" {}
        _SnowAmount ("Snow Amount", Range(0,1)) = 1
        _SnowSharpness ("Snow Sharpness", Range(1,8)) = 4
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        sampler2D _MainTex;
        sampler2D _SnowTex;
        float _SnowAmount;
        float _SnowSharpness;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldNormal;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float3 baseColor = tex2D(_MainTex, IN.uv_MainTex).rgb;
            float3 snowColor = tex2D(_SnowTex, IN.uv_MainTex).rgb;

            float snowMask = dot(normalize(IN.worldNormal), float3(0,1,0));
            snowMask = pow(saturate(snowMask), _SnowSharpness);
            snowMask *= _SnowAmount;

            float3 finalColor = lerp(baseColor, snowColor, snowMask);

            o.Albedo = finalColor;
            o.Smoothness = 0.7;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
