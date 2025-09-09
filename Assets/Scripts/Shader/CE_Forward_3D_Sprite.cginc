//======================================
// Project R-TypeDemo
// Copyright (c) 2020 CubicEgg.Ltd 
//======================================
// File     : CE_Forward_3D.cginc
// Author   : Okada Taizo
// Date     : 24st February, 2020
//======================================

//----------------------------------------
//
//	include
//
//----------------------------------------

#define UNITY_SHOULD_SAMPLE_SH	1

#include "CE_Utility.cginc"


//----------------------------------------
//
//	Uniform Variables
//
//----------------------------------------

uniform float _LightAttenuationThreshold;
uniform float _LightShadeBottom;

uniform float _SpecularColorRate;

uniform float _OutlineMaxPixel;
uniform float4 _OutLineBaseColor;

//----------------------------------------
//
//	Input & Output Structure
//
//----------------------------------------

UNITY_INSTANCING_BUFFER_START( Props )

    UNITY_DEFINE_INSTANCED_PROP( float, _OutlineSize )
    UNITY_DEFINE_INSTANCED_PROP( float, _minX )
    UNITY_DEFINE_INSTANCED_PROP( float, _maxX )
    UNITY_DEFINE_INSTANCED_PROP( float, _minZ )
    UNITY_DEFINE_INSTANCED_PROP( float, _maxZ )
    UNITY_DEFINE_INSTANCED_PROP( float4, _MainTexture )

UNITY_INSTANCING_BUFFER_END( Props )



struct VertexIn
{
    float4 vertex   : POSITION;
    float4 color    : COLOR;
    float3 normal   : NORMAL;

    UNITY_VERTEX_INPUT_INSTANCE_ID
};



struct VertexOut
{
    float4 pos          : SV_POSITION;
    float3 normal       : NORMAL;
    float4 worldPos     : TEXCOORD0;
    float3 ambient      : TEXCOORD1;
    SHADOW_COORDS( 2 )
};



struct VertexOutOutLine
{
    float4 pos          : SV_POSITION;
};


//----------------------------------------
//
//	Vertex Shader
//
//----------------------------------------

VertexOut vert( VertexIn v )
{
    VertexOut o;

    UNITY_SETUP_INSTANCE_ID( v );

    o.worldPos      = mul( UNITY_MATRIX_M, v.vertex );
    o.pos           = mul( UNITY_MATRIX_V, o.worldPos );
    o.pos           = mul( UNITY_MATRIX_P, o.pos );

    o.normal        = UnityObjectToWorldNormal( v.normal );
    o.ambient       = ShadeSH9( half4( o.normal, 1 ) ).rgb;

    TRANSFER_SHADOW( o )

    return o;
}



VertexOutOutLine vert_outline( VertexIn v ) {
    
    VertexOutOutLine o;

    UNITY_SETUP_INSTANCE_ID( v );

    //float outLineSize   = UNITY_ACCESS_INSTANCED_PROP( Props, _OutlineSize );
    float outLineSize   = 3;
    
    float4 scaledPos    = v.vertex;
    scaledPos.xyz       += v.normal * outLineSize * 0.1 * v.color.r;
    scaledPos           = UnityObjectToClipPos( scaledPos );
    scaledPos           /= scaledPos.w;
    o.pos               = scaledPos;

    scaledPos.xy        *= _ScreenParams.xy / 2;
    float4 originalPos  = UnityObjectToClipPos( v.vertex );
    originalPos         /= originalPos.w;
    originalPos.xy      *= _ScreenParams.xy / 2;

    float2 scaledVec    = scaledPos.xy - originalPos.xy;
    float scaledLength  = length( scaledVec );
    scaledLength        = min( scaledLength, _OutlineMaxPixel );
    scaledVec           = normalize( scaledVec ) * scaledLength;
    scaledVec.xy        /= _ScreenParams.xy / 2;
    originalPos.xy      /= _ScreenParams.xy / 2;
    o.pos.xy            = originalPos.xy + scaledVec.xy;

    return o;
}



//----------------------------------------
//
//	Fragment Shader
//
//----------------------------------------

fixed4 frag(VertexOut i) : SV_Target0
{
    float4 outCol   = float4( 0.0, 0.0, 0.0, 0.0 );
    
    //  Shadow
    fixed shadow    = SHADOW_ATTENUATION( i );

    float3 lightDir = normalize( _WorldSpaceLightPos0.xyz );
    float3 lightCol = _LightColor0;
    
    //  Diffuse
    float dotNL         = dot( i.normal, lightDir ) + _LightAttenuationThreshold;
    float lightAtten    = saturate( min( dotNL, shadow ) );
    lightAtten          = clamp( lightAtten, _LightShadeBottom, 1.0 );
    //outCol.rgb          = ( lightCol * lightAtten + i.ambient );

    CE_GET_METALLICROUGHNESS( metallic, roughness );

    //	Culc specular
    float3 viewDirection    = normalize( _WorldSpaceCameraPos.xyz - i.worldPos.xyz );

    float3 reflectDir       = reflect( -lightDir, i.normal );
    float specularDensity   = max( dot( reflectDir, viewDirection ), 0.0 );
    
    specularDensity         = pow( specularDensity, 0.001 + roughness * 100.0 ) * shadow;
    outCol                  += lerp(_LightColor0, _Color, _SpecularColorRate ) * specularDensity * metallic;

    float minX = UNITY_ACCESS_INSTANCED_PROP(Props, _minX);
    float maxX = UNITY_ACCESS_INSTANCED_PROP(Props, _maxX);
    float minZ = UNITY_ACCESS_INSTANCED_PROP(Props, _minZ);
    float maxZ = UNITY_ACCESS_INSTANCED_PROP(Props, _maxZ);
    float4 tex = UNITY_ACCESS_INSTANCED_PROP(Props, _MainTexture);
    texCol		= tex2D( _MainTexture, i.uv );
    if(i.worldPos.x < minX) {
        return 0,0,0,0;
    }
    if(i.worldPos.x > maxX) {
        return 0,0,0,0;
    }
    if(i.worldPos.z < minZ) {
        return 0,0,0,0;
    }
    if(i.worldPos.z > maxZ) {
        return 0,0,0,0;
    }
    
    return texCol;
}



fixed4 frag_outline ( VertexOutOutLine i ) : SV_Target0 {

    return _OutLineBaseColor;
}
