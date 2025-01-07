Shader "Custom/AutoTilingShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TileSize ("Taille de base de la tuile", Float) = 1.0
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
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _TileSize;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Calcul du tiling automatique basé sur la position mondiale
                float2 worldUV = float2(i.worldPos.x, i.worldPos.z) / _TileSize;
                fixed4 col = tex2D(_MainTex, worldUV);
                return col;
            }
            ENDCG
        }
    }
}