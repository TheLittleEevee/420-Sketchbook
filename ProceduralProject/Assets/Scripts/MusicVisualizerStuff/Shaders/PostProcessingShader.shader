Shader "MirandasShaders/PostProcessingShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("Noise", 2D) = "black" {}
        _Offset ("Offset", Float) = 0
        _Amp ("Amp", Float) = 0
        _Distort ("Distort", Float) = 0
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            sampler2D _NoiseTex;
            float _Offset;
            float _Amp;
            float _Distort;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv + (tex2D(_NoiseTex, i.uv + _Offset)) * .01 * ((_Amp + 1) * _Distort));

                // Alternate between inverted and not
                
                //col.rgb = _Amp - col.rgb;
                float time = (sin(_Time.y) + 1) / 2;
                col.rgb = time - col.rgb;
                //col.rgb = (_Amp * sin(_Time.y)) - col.rgb;
                 
                //if (col.r < 0 || col.g < 0 || col.b < 0) col.rgb *= -1;
                if (col.r < 0) col.r *= -1;
                if (col.g < 0) col.g *= -1;
                if (col.b < 0) col.b *= -1;
                //if (col.r > 1 || col.g > 1 || col.b > 1) col.rgb -= 1;
                //if (col.r > 1) col.r -= 1;
                //if (col.b > 1) col.b -= 1;
                //if (col.g > 1) col.g -= 1;

                return col;
            }
            ENDCG
        }
    }
}
