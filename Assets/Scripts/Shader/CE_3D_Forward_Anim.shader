//======================================
// Project Excave4
// Copyright (c) 2022 CubicEgg.Ltd 
//======================================
// File     : CE_3D_Forward_SingleLight_Anim.shader
// Author   : Okada Taizo
// Date     : 3rd Nov, 2022
//======================================

Shader "CE/Forward_3D_Anim"
{

    Properties
    {
        _Color2 ("Color2", Color ) = (1,1,1,1)
        
        [Enum(Opaque,0,AlphaTest,1,Transparent,2)]
        _Mode ( "Rendering Mode", int ) = 0
        [Enum(One,1,DstColor,2,SrcAlpha,5)]
        _BlendSrc ( "BlendSrc", int ) = 5
        [Enum(Zero,0,One,1,OneMinusSrcAlpha,10)]
        _BlendDst ( "BlendDst", int ) = 10
        [Enum(Off,0,Front,1,Back,2)]
        _CullMode ( "Cull Mode", int ) = 2
        [Enum(Less,2,Equal,3,LEqual,4,Greater,5,NotEqual,6,GEqual,7,Always,8)]
        _ZTest ( "Depth Test", int ) = 4
        [Enum(Off,0,On,1)]
        _ZWrite ( "Depth Write", int ) = 0
        _MainTex ( "ColorMap", 2D ) = "white" {}
        [Normal]
        _BumpMap ( "NormalMap", 2D ) = "white" {}
        _OcclusionMap ( "OcclusionMap", 2D ) = "white" {}
        _MetallicGlossMap ( "MetalicMap", 2D ) = "black" {}
        _SpecGlossMap ( "RoughnessMap", 2D ) = "black" {}
        _MatcapTex ( "MatcapTexture", 2D ) = "black" {}
        _EmissionMap ( "EmissionMap", 2D ) = "black" {}
        _EmissionColor ( "EmissionColor", Color ) = ( 1.0, 1.0, 1.0, 1.0 )
        _ColorMapBrightness ( "ColorMapBrightness", Range( 0.0, 1.0 ) ) = 1.0

        //	Light Diffuse
        _LightAttenuationThreshold ("LightAttenuationThreshold", Range( -1.0, 1.0 ) ) = 0.0
        _LightAmbientRate ( "LightAmbientRate", Range( 0.0, 1.0 ) ) = 0.5
        _LightShadeHardness ( "LightShadeHardness", Range( 1.0, 100.0 )) = 10.0
        _LightShadeColorRate ( "LightShadeColorRate", Range( 1.0, 10.0 ) ) = 2.0
        _LightShadeBottom ( "LightShadeBottom", Range( 0.0, 1.0 ) ) = 0.0

        //	Specular
        _Metallic ( "Metallic", Range( 0.0, 1.0 ) ) = 1.0
        _Glossiness ( "Roughness", Range( 0.0, 1.0 ) ) = 1.0
        _SpecularHardness ( "SpecularHardness", Range( 1.0, 100.0 )) = 10.0
        _SpecularColorRate ( "SpecularColorRate", Range( 0.0, 5.0 ) ) = 1.0

        //	RimLight
        _RimLightHardness ( "RimLightHardness", Range( 0.0, 50.0 ) ) = 10.0
        _RimLightSize ( "RimLightSize", Range( 0.0, 10.0 ) ) = 3.0
        _RimLightColorPower ( "RimLightColorPower", Range( 0.0, 5.0 ) ) = 2.0
        _RimLightRate ( "RimLightRate", Range( 0.0, 2.0 ) ) = 1.0

        _ReflectionRate ( "ReflectionRate", Range( 0.0, 5.0 ) ) = 0.5
        _NormalScale ( "NormalScale", Range( 0.0, 5.0 ) ) = 1.0

        //  Hide Item
        _Hide ( "Hide", Int ) = 0
        _HideColor ( "HideColor", Color ) = ( 0.0, 0.0, 0.0, 1.0 )

        //	OutLine
        _OutLineBlendRateColorMap ( "BlendRateColorMap", Range( 0.0, 1.0 ) ) = 0.5
        _OutLineBlendRateAmbient ( "BlendRateAmbient", Range( 0.0, 1.0 ) ) = 0.5
        _OutLineBaseColor ( "OutLineBaseColor", Color ) = ( 0.0, 0.0, 0.0, 0.0 )
        _OutlineSize ( "OutlineSize", Range( 0.0, 1.0 ) ) = 1.0
        _OutlineMaxPixel ( "OutlineMaxPixel", Range( 0.0, 20.0 ) ) = 5.0

        //	Stencil
        [Enum(Disable,0,Never,1,Less,2,Equal,3,LEqual,4,Greater,5,NotEqual,6,GEqual,7,Always,8)]
        _StencilComp ( "Stencil Comp", int ) = 0
        [Enum(Keep,0,Zero,1,Replace,2,IncrSat,3,DecrSat,4,Invert,5,IncrWrap,6,DecrWrap,7)]
        _StencilPass ( "Stencil Pass", int ) = 0
        _StencilRef ( "Stencil Reference", int ) = 0
    }



    SubShader
    {

        Tags
        {

            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }

        Stencil
        {

            Ref[_StencilRef]
            Comp[_StencilComp]
            Pass[_StencilPass]
        }

        LOD 100

        //======================================
        //
        //  CE Scriptable Render Pipeline
        //
        //======================================

        Pass
        {

            Name "FORWARD_ANIM"

            Tags
            {
                "LightMode" = "ForwardBase"
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
            #pragma shader_feature _APPLY_RIMLIGHT
            #pragma shader_feature _ _APPLY_REFLECTION

            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
            //#pragma multi_compile_instancing

            #include "CE_Forward_3D_Anim.cginc"
            ENDCG
        }


        //        Pass
        //        {
        //
        //            Name "FORWARD_GOLDEN"
        //
        //            Tags
        //            {
        //                "LightMode" = "ForwardBase"
        //            }
        //
        //            Offset -1, 0
        //
        //            //			Cull[_CullMode]
        //            //			ZTest[_ZTest]
        //            //			ZWrite[_ZWrite]
        //            Blend One OneMinusSrcAlpha
        //
        //            CGPROGRAM
        //            #pragma vertex vertGolden
        //            #pragma fragment fragInFront
        //            #pragma target 3.0
        //
        //            #pragma multi_compile_fwdbase
        //
        //            #pragma shader_feature _ _APPLY_NORMALMAP
        //            #pragma shader_feature _ _APPLY_EMISSIONMAP
        //            #pragma shader_feature _ _APPLY_OCCLUSIONMAP
        //            #pragma shader_feature _ _APPLY_METALLIC_ROUGHNESSMAP
        //            #pragma shader_feature _ _APPLY_VERTEXCOLOR
        //            #pragma shader_feature _ _APPLY_RIMLIGHT
        //            #pragma shader_feature _ _APPLY_REFLECTION
        //
        //            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
        //            //#pragma multi_compile_instancing
        //
        //            #include "CE_Forward_3D_Anim.cginc"
        //            ENDCG
        //        }


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
            ZWrite On
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



        //======================================
        //
        //  Pass for Unity Editor Scene
        //
        //======================================

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
        /*
        Pass {

            Name "SCENEVIEWER_OUTLINE"
            
            Tags {
                "LightMode" = "SceneViewerAdd"
            }

            Offset 2, 0

            Cull Front
            //Cull OFF
            ZTest LEqual
            ZWrite On
            Blend One Zero

            CGPROGRAM

            #pragma vertex vert_outline
            #pragma fragment frag_outline
            #pragma target 3.0
            
            #pragma multi_compile_fwdbase
            #pragma multi_compile_instancing
            
            #include "CE_Forward_3D_Anim.cginc"

            ENDCG
        }
        */
        //======================================
        //
        //  Pass for Unity Preview window
        //
        //======================================

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
        /*
        Pass {

            Name "PREVIEW_OUTLINE"
            
            Tags {
                "LightMode" = "PreviewAdd"
            }

            Offset 2, 0

            Cull Front
            //Cull OFF
            ZTest LEqual
            ZWrite On
            Blend One Zero

            CGPROGRAM

            #pragma vertex vert_outline
            #pragma fragment frag_outline
            #pragma target 3.0
            
            #pragma multi_compile_fwdbase
            #pragma multi_compile_instancing
            
            #include "CE_Forward_3D_Anim.cginc"

            ENDCG
        }
        */
    }

    CustomEditor "CustomShaderEditor_CE_Forward_3D_Anim"
}