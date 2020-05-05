Shader "Unlit/Checkerboard"
{
    Properties
    {
        _Color1 ("Color", Color) = (1, 1, 1, 1)
        _Color2 ("Color", Color) = (1, 1, 1, 1)
        _MainTex ("MainTex", 2D) = "white" {}
        _Rows ("Rows", Int) = 6
        _Columns("Columns", Int) = 6
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
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
                float4 vertex : SV_POSITION;
            };

            float4 _Color1;
            float4 _Color2;
            sampler2D _MainTex;
            float _Rows;
            float _Columns;

            half rand2(half2 coords) 
            {
                return frac(sin(dot(coords, half2(12.9898, 78.233))) * 43758.5453);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                const half2x2 rotMx[4] = { half2x2(1, 0, 0, 1), half2x2(0, 1, -1, 0),
                                           half2x2(-1, 0, 0, -1), half2x2(0, -1, 1, 0) };

                float2 uv = i.uv;
                float rowInterval = 1 / _Rows;
                float colInterval = 1 / _Columns;
                uv = float2(uv.x / rowInterval, uv.y / colInterval);
                float rowEven = floor(uv.x % 2);
                float rowOdd = 1 - rowEven;
                float colEven = floor(uv.y % 2);
                float colOdd = 1 - colEven;
                float strength = rowEven * colOdd + rowOdd * colEven;
                int rot = rand2(floor(uv)) * 4;
                uv = mul(rotMx[rot], uv);
                uv = uv / 4 + 0.25 * rot;

                half4 color = tex2D(_MainTex, uv, ddx(uv), ddy(uv));
                return color * (_Color1 * strength + _Color2 * (1 - strength));
            }
            ENDCG
        }
    }
}
