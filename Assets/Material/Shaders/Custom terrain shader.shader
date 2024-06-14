Shader "Custom/Custom terrain shader"
{
    Properties
    {
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _NormalMap("Normal Map", 2D) = "bump" {}
        _Roughness("Roughness Map", 2D) = "white" {}
        _Displacement("Displacement Map", 2D) = "black" {}
        _AO("Ambient Occlusion Map", 2D) = "white" {}
    }
        SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard

        sampler2D _MainTex;
        sampler2D _NormalMap;
        sampler2D _Roughness;
        sampler2D _Displacement;
        sampler2D _AO;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_NormalMap;
            float2 uv_Roughness;
            float2 uv_Displacement;
            float2 uv_AO;
        };

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo and Normal
            o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
            o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_NormalMap));

            // Roughness
            float roughness = tex2D(_Roughness, IN.uv_Roughness).r;
            o.Metallic = 0.0; // Assuming non-metallic for terrain
            o.Smoothness = 1.0 - roughness; // Inverted for smoothness

            // Displacement (parallax mapping)
            // Assuming Parallax mapping implementation

            // Ambient Occlusion
            o.Occlusion = tex2D(_AO, IN.uv_AO).r;
        }
        ENDCG
    }
        FallBack "Diffuse"
}
