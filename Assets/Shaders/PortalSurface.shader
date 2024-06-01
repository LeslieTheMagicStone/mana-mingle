Shader "Unlit/PortalSurface"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DistortionTex("Distort Texture", 2D) = "black" {}
        _Speed ("Speed", Float) = 1.0
        _Intensity("Intensity", Float) = 1.0
        _Color("Color Multiplier", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _DistortionTex;
            float4 _DistortionTex_ST;

            float _Speed;
            float _Intensity;
            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the Distortion
                float2 distortionUV = TRANSFORM_TEX(i.uv, _DistortionTex);
                distortionUV.x = distortionUV.x + _Time.x * _Speed;
                fixed4 distortion = tex2D(_DistortionTex, distortionUV);

                // Sample the Texture
                float2 mainUV = TRANSFORM_TEX(i.uv, _MainTex);
                fixed4 col = tex2D(_MainTex, mainUV + 0.01f * _Intensity * distortion.xy);

                return col * _Color;
            }
            ENDCG
        }
    }
}
