//======================================
// Project My Neighborhood Emiko-san 
// Copyright (c) 2018 CubicEgg.Ltd 
//======================================
// File     : CE_Utility.cginc
// Author   : Okada Taizo
// Date     : 13st November, 2018
//======================================

//----------------------------------------
//
//	include
//
//----------------------------------------

#include "AutoLight.cginc"
#include "UnityCG.cginc"
#include "UnityStandardInput.cginc"



//----------------------------------------
//
//	Define
//
//----------------------------------------

#define	MAX_SEARCHLENGTH	16
#define MAX_BLUR_AMOUNT		6
#define	LUMINANCE			float3( 0.299, 0.587, 0.114 )
#define MAX_LIGHT_COUNT		32
#define SCREEN_DIV_H        2
#define SCREEN_DIV_V        3



//----------------------------------------
//
//	Valuables
//
//----------------------------------------

uniform float _CameraNearClip;
uniform float _CameraFarClip;
uniform float4x4 _ShadowViewProj;
uniform sampler2D _ShadowMap;
uniform float _ShadowBias;
uniform float4 _Light[ MAX_LIGHT_COUNT * 2 ];
uniform float4 _ScreenLightIndex[ SCREEN_DIV_H * SCREEN_DIV_V ];
uniform float4 _ScreenRTLightIndex[ SCREEN_DIV_H * SCREEN_DIV_V ];
uniform float4 _ScreenFogLightIndex[ SCREEN_DIV_H * SCREEN_DIV_V ];
uniform fixed3 _AmbientColor;
uniform float4x4 _InvProjection;
uniform float4x4 _InvViewProjection;



//----------------------------------------
//
//	Input & Output Structure
//
//----------------------------------------

struct fragout_forward {

	float4 Color	: SV_Target0;		//	RT0, ARGB32 format
	float4 DepthTex	: SV_Target1;		//  RT1, Depth texture
};



struct fragout_Deferred {

    float4 gBuffer0	: SV_Target0;		//	RT0, ARGB32 format : Diffuse color(RGB), occlusion(A).
    float4 gBuffer1	: SV_Target1;		//	RT1, ARGB32 format : Specular color(RGB), roughness(A).
    float4 gBuffer2	: SV_Target2;		//	RT2, ARGB2101010 format : World space normal(RGB), unused(A).
    float4 gBuffer3	: SV_Target3;		//	RT3, ARGB2101010(non - HDR) or ARGBHalf(HDR) format : Emission + lighting + lightmaps + reflection probes buffer.
};



struct fragout_Deferred_SRP {

    float4 gBuffer0	: SV_Target0;		//	RT0, ARGB32 format : Albedo_RGB, Occlusion
    float4 gBuffer1	: SV_Target1;		//	RT1, ARGB32 format : WorldSpace_Normal.xy, Specular_Rate&Power
    float4 gBuffer2	: SV_Target2;		//	RT3, ARGB2101010 format : Emission + ambient + reflection.
    float4 gBuffer3	: SV_Target3;		//  Depth texture
};



//----------------------------------------
//
//	Helper Function
//
//----------------------------------------


float4 FloatToColor( float _value ) {
	
	float4 output;

	_value		*= 255.0;
	output.x	= int( _value ) / 255.0;
	_value		-= int( _value );
	_value		*= 255.0;
	output.y	= int( _value ) / 255.0;
	_value		-= int( _value );
	_value		*= 255.0;
	output.z	= int( _value ) / 255.0;
	_value		-= output.z;
	_value		*= int( _value ) / 255.0;
	output.w	= int( _value );

	return output;
}



float ColorToFloat( float4 _value ) {
	
	float output;

	output		= _value.x;
	output		+= _value.y / pow( 255.0, 1.0 );
	output		+= _value.z / pow( 255.0, 2.0 );
	output		+= _value.w / pow( 255.0, 3.0 );

	return output;
}



float GetDistanceFromCamera( sampler2D _depthMap, float2 _uv ) {
    
	float depth 	    = tex2D( _depthMap, _uv ).r;
    float2 screenPos    = _uv * 2.0 + 1.0;
    float4 outputPos    = mul( _InvProjection, float4( screenPos, depth, 1.0 ) );
    
    return outputPos.z / outputPos.w;
}



float ShadowMapFilter( float3 _worldPos ) {

    float retval        = 1.0;

#if _APPLY_SHADOW_MAP

    float4 shadowPos    = mul( _ShadowViewProj, float4( _worldPos, 1.0 ) );
    shadowPos           /= shadowPos.w;
    shadowPos.y         *= -1.0;
	float2 depth        = tex2D( _ShadowMap, shadowPos.xy * 0.5 + 0.5 ).rg;

#if _APPLY_SOFT_SHADOW

    float depth_sq  = depth.x * depth.x;
    float variance  = depth.y - depth_sq;
    float md        = shadowPos.z - depth.x;
    float p         = variance / (variance + ( md * md ) );

    retval      = saturate( max( p, depth.x <= shadowPos.z + _ShadowBias ) );

#else

    retval      = shadowPos.z > depth.x - _ShadowBias;

#endif
#elif _APPLY_PROJECTION_MAP

    float4 shadowPos    = mul( _ShadowViewProj, float4( _worldPos, 1.0 ) );
    shadowPos           /= shadowPos.w;
    shadowPos.y         *= -1.0;
    float depth         = tex2D( _ShadowMap, shadowPos.xy * 0.5 + 0.5 ).r;
    retval              = 1.0 - depth;

#endif

    return retval;
}



int GetScreenPosIndex( float2 _screenPos ) {

    _screenPos      = _screenPos * 0.5 + 0.5;
    _screenPos.x    *= SCREEN_DIV_H;
    _screenPos.y    *= SCREEN_DIV_V;

    int index       = floor(_screenPos.x) + floor(_screenPos.y) * SCREEN_DIV_H;
    index           = clamp( index, 0, SCREEN_DIV_H * SCREEN_DIV_V - 1 );

    return index;
}



float4 GetNearestRTLightIndex( float2 _screenPos ) {
    
    int screenIdx   = GetScreenPosIndex( _screenPos );

    return _ScreenRTLightIndex[ screenIdx ];
}



float4 GetNearestLightIndex( float2 _screenPos ) {
    
    int screenIdx   = GetScreenPosIndex( _screenPos );

    return _ScreenLightIndex[ screenIdx ];
}



float4 GetNearestFogLightIndex( float2 _screenPos ) {
    
    int screenIdx   = GetScreenPosIndex( _screenPos );

    return _ScreenFogLightIndex[ screenIdx ];
}



half4 GetLightMapColor( sampler2D _lightMap, sampler2D _shadowMask, sampler2D _dirLightMap, float2 _uv, half3 _normal ) {

    half4 outCol;

    half3 lmCol             = tex2D( _lightMap, _uv ).rgb;
    outCol.a                = tex2D( _shadowMask, _uv ).r;
    fixed4 bakedDirLight    = tex2D( _dirLightMap, _uv );
    outCol.rgb              = DecodeDirectionalLightmap( lmCol, bakedDirLight, _normal );

    return outCol;
}



//----------------------------------------
//
//	Common
//
//----------------------------------------
#if defined(_APPLY_REFLECTION) || defined(_APPLY_RIMLIGHT)

#define CE_SETUP_VIEWNORMAL_VS( _normal ) \
	    o.viewNormal	= mul( UNITY_MATRIX_V, _normal );

#else

#define CE_SETUP_VIEWNORMAL_VS( _normal )

#endif



//----------------------------------------
//
//	Vertex Color
//
//----------------------------------------

#ifdef _APPLY_VERTEXCOLOR

#define CE_SETUP_V_INPUT_VERTEXCOLOR \
        float4 color			: COLOR;

#define CE_SETUP_V2F_VERTEXCOLOR \
        float4 color			: TEXCOORD5;

#define CE_SETUP_VERTEXCOLOR_VS \
        o.color         = v.color;

#define CE_APPLY_VERTEXCOLOR( outCol ) \
        outCol          *= i.color;

#else

#define CE_SETUP_V_INPUT_VERTEXCOLOR
#define CE_SETUP_V2F_VERTEXCOLOR
#define CE_SETUP_VERTEXCOLOR_VS
#define CE_APPLY_VERTEXCOLOR( outCol )

#endif



//----------------------------------------
//
//	Normal Map
//
//----------------------------------------

#ifdef _APPLY_NORMALMAP

#define CE_SETUP_V_INPUT_NORMALMAP \
        float4 tangent			: TANGENT;

#define CE_SETUP_V2F_NORMALMAP \
        float3 tangent			: TEXCOORD3; \
        float3 bitangent		: TEXCOORD4;

#define CE_SETUP_TANGENTBINORMAL_VS( _normal ) \
        o.tangent   = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz ); \
        o.bitangent = normalize( cross( _normal, o.tangent ) * v.tangent.w );

#define CE_GET_BUMPMAP_NORMAL( _in_normal, _out_normal ) \
	    float3 _out_normal          = normalize( _in_normal ); \
        float3 tangent              = normalize( i.tangent ); \
        float3 bitangent            = normalize( i.bitangent ); \
        float3x3 tangentTransform   = float3x3( tangent, bitangent, _out_normal ); \
        _out_normal                 = UnpackNormal( tex2D( _BumpMap, i.uv ) ); \
        _out_normal                 = normalize( mul( _out_normal, tangentTransform ) );

#else

#define CE_SETUP_V_INPUT_NORMALMAP
#define CE_SETUP_V2F_NORMALMAP
#define CE_SETUP_TANGENTBINORMAL_VS( _normal )
#define CE_GET_BUMPMAP_NORMAL( _in_normal, _out_normal ) \
        float3 _out_normal = normalize(_in_normal);

#endif




//----------------------------------------
//
//	Metallic Roughness Map
//
//----------------------------------------

#ifdef _APPLY_METALLIC_ROUGHNESSMAP

#define CE_GET_METALLICROUGHNESS( _metallic, _roughness ) \
	    float _metallic     = tex2D( _MetallicGlossMap, i.uv ).r; \
	    float _roughness    = tex2D( _SpecGlossMap, i.uv ).r;

#else

#define CE_GET_METALLICROUGHNESS( _metallic, _roughness ) \
	    float _metallic     = _Metallic; \
        float _roughness    = _Glossiness;
        
#endif



//----------------------------------------
//
//	Occlusion Map
//
//----------------------------------------

#ifdef _APPLY_OCCLUSIONMAP

#define CE_GET_OCCLUSION( _occlusion ) \
	    _occlusion  = min( tex2D( _OcclusionMap, i.uv ).r, _occlusion );

#else

#define CE_GET_OCCLUSION( _occlusion )

#endif



//----------------------------------------
//
//	Emmision Map
//
//----------------------------------------

#ifdef _APPLY_EMISSIONMAP

#define CE_GET_EMISSION( _uv, _emission ) \
	    _emission   += tex2D( _EmissionMap, _uv ).rgb * _EmissionColor.rgb;

#else

#define CE_GET_EMISSION( _uv, _emission )

#endif



//----------------------------------------
//
//	Light Map
//
//----------------------------------------

#ifdef _APPLY_LIGHTMAP

#define CE_SETUP_V2F_LIGHTMAP_UV \
        float2 lmuv             : TEXCOORD6;

#define CE_SETUP_PROPS_LIGHTMAP \
        UNITY_DEFINE_INSTANCED_PROP( float4, _LightMapST )

uniform sampler2D _LightMap;
uniform sampler2D _ShadowMask;

#define CE_GET_LIGHTMAP_UV( _in_uv, _out_lmuv ) \
        float4 lightMapST   = UNITY_ACCESS_INSTANCED_PROP( Props, _LightMapST ); \
        _out_lmuv           = _in_uv.xy * lightMapST.xy + lightMapST.zw;

#define CE_GET_LIGHTMAP_COLOR( _uv, _normal, _outCol, _occlusion ) \
        _outCol             *= tex2D( _LightMap, _uv ).rgb; \
        _occlusion          = min( tex2D( _ShadowMask, _uv ).r, _occlusion );

#else

#define CE_SETUP_V2F_LIGHTMAP_UV
#define CE_SETUP_PROPS_LIGHTMAP
#define CE_GET_LIGHTMAP_UV( _in_uv, _out_lmuv )
#define CE_GET_LIGHTMAP_COLOR( _uv, _normal, outCol, occlusion ) \
        outCol  = float3( 0, 0, 0 );

#endif



//----------------------------------------
//
//	Reflection
//
//----------------------------------------

#ifdef _APPLY_REFLECTION

#define CE_SETUP_UNIFORM_REFLECTION_ANIM \
        uniform sampler2D _MatcapTex; \
        uniform float _ReflectionRate; \
        uniform float _NormalScale;

#ifndef _APPLY_RIMLIGHT

#define CE_SETUP_V2F_REFLECTION_ANIM \
	    float3 viewNormal		: TEXCOORD6;

#else

#define CE_SETUP_V2F_REFLECTION_ANIM

#endif

#define CE_GET_REFLECTION_COLOR_ANIM( _outCol ) \
        float2 uv       = clamp( ( i.viewNormal.xy * _NormalScale ) * 0.5 + 0.5, 0, 1 ); \
	    float3 matCap   = tex2D( _MatcapTex, uv ).rgb; \
        _outCol 		+= matCap * _ReflectionRate;

#else

#define CE_SETUP_UNIFORM_REFLECTION_ANIM
#define CE_SETUP_V2F_REFLECTION_ANIM
#define CE_GET_REFLECTION_COLOR_ANIM( outCol )

#endif



//----------------------------------------
//
//	Rim Light
//
//----------------------------------------

#ifdef _APPLY_RIMLIGHT

#define CE_SETUP_UNIFORM_RIMLIGHT \
        uniform float4 _RimLightColor; \
        uniform float _RimLightPower;

#define CE_SETUP_UNIFORM_RIMLIGHT_ANIM \
        uniform float _RimLightHardness; \
        uniform float _RimLightSize; \
        uniform float _RimLightColorPower; \
        uniform float _RimLightRate; \

#define CE_SETUP_V2F_RIMLIGHT_ANIM \
	    float3 viewNormal		: TEXCOORD6;

#define CE_APPLY_RIMLIGHT( _viewDirection, _normal, _outCol ) \
	    float3 reflectDirRim	= reflect( _viewDirection, _normal ); \
        float rimLightPower     = pow( max( dot( reflectDirRim, _viewDirection ), 0.0001 ), 1 + _RimLightPower ); \
        _outCol                 += _RimLightColor.xyz * rimLightPower;

#define CE_APPLY_RIMLIGHT_ANIM( _viewNormal, _attenuation, _outCol ) \
        float rimLightPower     = 1.0 - clamp( dot( _viewNormal, float3( 0.0, 0.0, 1.0 ) ) * _RimLightHardness - _RimLightSize, 0, 1 ); \
        _outCol                 += pow( _outCol, _RimLightColorPower ) * _attenuation * rimLightPower * _RimLightRate;

#else

#define CE_SETUP_UNIFORM_RIMLIGHT
#define CE_SETUP_UNIFORM_RIMLIGHT_ANIM
#define CE_SETUP_V2F_RIMLIGHT_ANIM
#define CE_APPLY_RIMLIGHT( _viewDirection, _normal, _outCol )
#define CE_APPLY_RIMLIGHT_ANIM( _viewNormal, _attenuation, _outCol )

#endif