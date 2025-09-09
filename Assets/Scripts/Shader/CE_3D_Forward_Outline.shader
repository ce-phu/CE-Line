//======================================
// Project ColorBlock
// Copyright (c) 2025 CubicEgg.Ltd 
//======================================
// File     : Forward_3D_Outline.shader
// Author   : Okada Taizo
// Date     : 3rd May, 2025
//======================================

Shader "CE/Forward_3D_Outline"
{

    Properties
    {
        //	OutLine
        _OutLineBaseColor   ( "OutLineBaseColor",       Color               ) = ( 1.0, 1.0, 1.0, 1.0 )
        _OutlineMaxPixel    ( "OutlineMaxPixel",        Range( 0.0, 20.0 )  ) = 5.0
    }



    SubShader
    {

        Tags
        {
            "RenderType"    = "Transparent"
            "Queue"         = "Transparent"
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

            Cull Front
            ZTest Always
            ZWrite ON
            Blend ONE ZERO

            CGPROGRAM
            #pragma vertex vert_outline
            #pragma fragment frag_outline
            #pragma target 3.0

            #pragma multi_compile_fwdbase
            #pragma multi_compile_instancing
            
            #include "CE_Forward_3D_Block.cginc"

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

            Cull Front
            ZTest Always
            ZWrite OFF
            Blend ONE ZERO

            CGPROGRAM
            #pragma vertex vert_outline
            #pragma fragment frag_outline
            #pragma target 3.0

            #pragma multi_compile_fwdbase
            #pragma multi_compile_instancing

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

            Cull Front
            ZTest Always
            ZWrite OFF
            Blend ONE ZERO

            CGPROGRAM
            #pragma vertex vert_outline
            #pragma fragment frag_outline
            #pragma target 3.0

            #pragma multi_compile_fwdbase
            #pragma multi_compile_instancing

            #include "CE_Forward_3D_Block.cginc"

            ENDCG
        }
    }
}