Shader "Unlit/Board"
{
    Properties
    {
        [NoScaleOffset] _MainTex("MainTex", 2D) = "white" {}
        _Row ("Row", Int) = 0
        _Col("Col", Int) = 0
        _SheetRows ("Sheet Rows", Int) = 4
        _SheetCols ("Sheet Cols", Int) = 4
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
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float _Row;
            float _Col;
            float _SheetRows;
            float _SheetCols;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;
                uv.x = (uv.x + _Row) / _SheetRows;
                uv.y = (uv.y + _Col) / _SheetCols;
                half4 col = tex2D(_MainTex, uv);
                return col * i.color;
            }
            ENDCG
        }
    }
}
