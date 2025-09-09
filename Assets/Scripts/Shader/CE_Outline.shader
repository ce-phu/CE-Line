Shader "Unlit/CE_Outline"
{
    Properties
    {
        _MainTex ( "ColorMap", 2D ) = "white" {}
        _Color ("Color", Color ) = (1,1,1,1)
        //	OutLine
        _OutLineBlendRateColorMap ( "BlendRateColorMap", Range( 0.0, 1.0 ) ) = 0.5
        _OutLineBlendRateAmbient ( "BlendRateAmbient", Range( 0.0, 1.0 ) ) = 0.5
        _OutLineBaseColor ( "OutLineBaseColor", Color ) = ( 0.0, 0.0, 0.0, 0.0 )
        _OutlineSize ( "OutlineSize", Range( 0.0, 1.0 ) ) = 1.0
        _OutlineMaxPixel ( "OutlineMaxPixel", Range( 0.0, 20.0 ) ) = 5.0
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
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
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
                fixed4 col = tex2D(_MainTex, i.uv);
                return col * _Color;
            }
            ENDCG
        }
        
        Pass
        {

            Name "FORWARD_OUTLINE"

            Tags
            {
                "LightMode" = "ForwardBase"
            }

            Offset 2, 0

            Cull Front
            ZTest LEqual
            ZWrite Off
            Blend One Zero

            CGPROGRAM
            #pragma vertex vert_outline
            #pragma fragment frag_outline
            #pragma target 3.0

            #pragma shader_feature _APPLY_VERTEXCOLOR
            #pragma multi_compile_fwdbase
            #pragma multi_compile_instancing

            #include "CE_Forward_3D_Anim.cginc"
            ENDCG
        }
        
        Pass
        {

            Name "CastShadow"

            Tags
            {
                "LightMode" = "ShadowCaster"
            }

            Offset 1, 1

            Cull[_CullMode]
            Zwrite On
            ZTest LEqual

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #pragma multi_compile _APPLY_FADEOUT
            #pragma multi_compile _ALPHA_TEST
            #pragma multi_compile _ALPHA_TEST_GREATER
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "CE_CastShadow.cginc"
            ENDCG
        }
        
        Pass
        {

            Name "SCENEVIEWER"

            Tags
            {
                "LightMode" = "SceneViewer"
            }

            Cull[_CullMode]
            ZTest[_ZTest]
            ZWrite[_ZWrite]
            Blend[_BlendSrc][_BlendDst]

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #pragma multi_compile_fwdbase

            #pragma shader_feature _ _APPLY_NORMALMAP
            #pragma shader_feature _ _APPLY_EMISSIONMAP
            #pragma shader_feature _ _APPLY_OCCLUSIONMAP
            #pragma shader_feature _ _APPLY_METALLIC_ROUGHNESSMAP
            #pragma shader_feature _ _APPLY_VERTEXCOLOR
            #pragma shader_feature _ _APPLY_RIMLIGHT
            #pragma shader_feature _ _APPLY_REFLECTION

            #pragma multi_compile_instancing

            #include "CE_Forward_3D_Anim.cginc"
            ENDCG
        }
        
        Pass
        {

            Name "PREVIEW"

            Tags
            {
                "LightMode" = "Preview"
            }

            Cull[_CullMode]
            Blend One Zero
            ZTest LEqual
            Zwrite On

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #pragma multi_compile_fwdbase

            #pragma shader_feature _ _APPLY_NORMALMAP
            #pragma shader_feature _ _EMISSION
            #pragma shader_feature _ _APPLY_OCCLUSIONMAP
            #pragma shader_feature _ _APPLY_GLOTH_ROUGHNESSMAP
            #pragma shader_feature _ _APPLY_RIMLIGHT
            #pragma shader_feature _ _APPLY_LIGHTINGRATE
            #pragma shader_feature _ _APPLY_ALPHAFADE
            #pragma shader_feature _ _ALPHA_TEST
            #pragma shader_feature _ _APPLY_LIGHTMAP

            #pragma multi_compile_instancing

            #include "CE_Forward_3D_Anim.cginc"
            ENDCG
        }
    }
}
