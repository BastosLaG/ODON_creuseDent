Shader "Custom/FlipTexture"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {} // La texture principale
        _FlipInterval ("Flip Interval", Float) = 2.0 // Intervalle pour flip (taille du tile en UV)
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

            sampler2D _MainTex;
            float _FlipInterval;

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

                // On modifie ici les coordonnées UV pour appliquer le flip
                float2 uvMod = v.uv / _FlipInterval; // Diviser par l'intervalle
                float flipCondition = floor(uvMod.x) % 2.0; // Alterner sur 2
                if (flipCondition == 1.0)
                {
                    uvMod.x = ceil(uvMod.x) - uvMod.x; // Inverser les coordonnées UV
                }
                o.uv = uvMod * _FlipInterval; // Revenir à l'échelle initiale

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return tex2D(_MainTex, i.uv); // Retourne la texture avec le flip appliqué
            }
            ENDCG
        }
    }
}
