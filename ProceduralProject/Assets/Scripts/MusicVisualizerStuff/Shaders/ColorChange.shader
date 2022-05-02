Shader "MirandasShaders/ColorChange"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ColorROffset ("ColorROffset", float) = 0
        _ColorGOffset ("ColorGOffset", float) = 0
        _ColorBOffset ("ColorBOffset", float) = 0
        _AlphaOffset ("AlphaOffset", float) = 0
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
            // make fog work
            #pragma multi_compile_fog

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
            float _ColorROffset;
            float _ColorGOffset;
            float _ColorBOffset;
            float _AlphaOffset;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                //fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 col = fixed4(_ColorROffset, _ColorGOffset, _ColorBOffset, _AlphaOffset);
                if (_AlphaOffset == 0) clip(-1);

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
