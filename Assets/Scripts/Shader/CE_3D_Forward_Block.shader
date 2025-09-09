//======================================
// Project ColorBlock
// Copyright (c) 2025 CubicEgg.Ltd 
//======================================
// File     : Forward_3D_Block.shader
// Author   : Okada Taizo
// Date     : 3rd May, 2025
//======================================

Shader "CE/Forward_3D_Block"
{

    Properties
    {
        _Color          ( "Color",          Color       ) = ( 1.0, 1.0, 1.0, 1.0 )
        _EmissionColor  ( "EmissionColor",  Color       ) = ( 0.0, 0.0, 0.0, 0.0 )
        
        _minX ("minX", float) = -100
        _maxX ("maxX", float) = 100
        _minZ ("minZ", float) = -100
        _maxZ ("maxZ", float) = 100
        
        //	Light Diffuse
        _LightAttenuationThreshold  ( "LightAttenuationThreshold",  Range( -1.0, 1.0 )  ) =  0.0
        _LightShadeBottom           ( "LightShadeBottom",           Range( 0.0, 1.0 )   ) =  0.0

        //	Reclection
        _MatcapTex      ( "MatcapTexture",                          2D                  ) = "black" {}
        _ReflectionRate("ReflectionRate", Range(0.0, 5.0)) = 0.5
        _NormalScale("NormalScale", Range(0.0, 5.0)) = 1.0
        
        //	Specular
        _Metallic ( "Metallic", Range( 0.0, 1.0 ) ) = 1.0
        _Glossiness ( "Roughness", Range( 0.0, 1.0 ) ) = 1.0
        _SpecularHardness ( "SpecularHardness", Range( 1.0, 100.0 )) = 10.0
        _SpecularColorRate ( "SpecularColorRate", Range( 0.0, 5.0 ) ) = 1.0

        //	OutLine
        _OutLineBaseColor   ( "OutLineBaseColor",       Color               ) = ( 1.0, 1.0, 1.0, 1.0 )
        _OutlineMaxPixel    ( "OutlineMaxPixel",        Range( 0.0, 20.0 )  ) = 5.0
    }



    SubShader
    {

        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }

        LOD 100

        //======================================
        //
        //  Unity BuiltIn Pipeline
        //
        //======================================

        Pass
        {

            Name "FORWARD_ANIM"

            Tags
            {
                "LightMode" = "ForwardBase"
            }

            Cull Back
            ZTest LEqual
            ZWrite ON
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #pragma multi_compile_fwdbase
            #pragma multi_compile_instancing
            #pragma shader_feature _APPLY_REFLECTION
            
            #include "CE_Forward_3D_Block.cginc"

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

            Cull Back
            ZTest LEqual
            ZWrite ON
            Blend ONE ZERO

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #pragma multi_compile_fwdbase
            #pragma multi_compile_instancing
            #pragma shader_feature _APPLY_REFLECTION

            #include "CE_Forward_3D_Block.cginc"

            ENDCG
        }

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

            Cull Back
            ZTest LEqual
            ZWrite ON
            Blend ONE ZERO

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #pragma multi_compile_fwdbase
            #pragma multi_compile_instancing
            #pragma shader_feature _APPLY_REFLECTION

            #include "CE_Forward_3D_Block.cginc"

            ENDCG
        }
    }

    CustomEditor "CustomShaderEditor_CE_Forward_3D_Block"
}