Shader "Custom/FlowingColors" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _FlowSpeed ("Flow Speed", Range(0, 10)) = 1
        _Color1 ("Color 1", Color) = (1, 0, 0, 1)
        _Color2 ("Color 2", Color) = (0, 1, 0, 1)
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _NoiseTex;
            float _FlowSpeed;
            fixed4 _Color1;
            fixed4 _Color2;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                float2 uv = i.uv;
                uv.x += _Time.y * _FlowSpeed;
                uv.y -= _Time.y * _FlowSpeed;
                fixed4 col = tex2D(_MainTex, uv);
                fixed4 noise = tex2D(_NoiseTex, uv);
                fixed4 color = lerp(_Color1, _Color2, noise.r);
                col *= color;
                return col;
            }
            ENDCG
        }
    }
}