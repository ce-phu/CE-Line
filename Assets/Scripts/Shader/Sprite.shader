Shader "Unlit/Sprite"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _minX ("minX", float) = -100
        _maxX ("maxX", float) = 100
        _minZ ("minZ", float) = -100
        _maxZ ("maxZ", float) = 100
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "Queue" = "Transparent"

        }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha

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
                float4 worldPos : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _minX;
            float _maxX;
            float _minZ;
            float _maxZ;

            v2f vert( appdata v ) {
                v2f o;
                o.vertex   = UnityObjectToClipPos( v.vertex );
                o.worldPos = mul( UNITY_MATRIX_M, v.vertex );
                o.uv       = TRANSFORM_TEX( v.uv, _MainTex );
                return o;
            }

            fixed4 frag( v2f i ) : SV_Target {
                fixed4 col = tex2D( _MainTex, i.uv );
                if ( i.worldPos.x < _minX ) {
                    return 0, 0, 0, 0;
                }
                if ( i.worldPos.x > _maxX ) {
                    return 0, 0, 0, 0;
                }
                if ( i.worldPos.z < _minZ ) {
                    return 0, 0, 0, 0;
                }
                if ( i.worldPos.z > _maxZ ) {
                    return 0, 0, 0, 0;
                }
                return col;
            }
            ENDCG
        }
    }
}