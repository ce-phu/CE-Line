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

CE_SETUP_UNIFORM_RIMLIGHT_ANIM
CE_SETUP_UNIFORM_REFLECTION_ANIM

uniform float4 _HideColor;

uniform float _SpecularColorRate;
uniform float4 _Color2;

uniform float _LightAttenuationThreshold;
uniform float _LightShadeHardness;
uniform float _LightShadeColorRate;
uniform float _LightShadeBottom;
uniform float _LightAmbientRate;

uniform float _OutLineBlendRateColorMap;
uniform float _OutLineBlendRateAmbient;
uniform float4 _OutLineBaseColor;
uniform float _OutlineMaxPixel;

uniform sampler2D _GoldenItemTexture1;
uniform sampler2D _GoldenItemTexture2;
uniform sampler2D _GoldenItemTexture3;

// uniform float _ColorMapBrightness;


//----------------------------------------
//
//	Input & Output Structure
//
//----------------------------------------


UNITY_INSTANCING_BUFFER_START(Props)
    UNITY_DEFINE_INSTANCED_PROP(float, _SpecularHardness)
    UNITY_DEFINE_INSTANCED_PROP(float, _ColorMapBrightness)
    UNITY_DEFINE_INSTANCED_PROP(int, _IndexGoldTexture)
    UNITY_DEFINE_INSTANCED_PROP(int, _Hide)
    UNITY_DEFINE_INSTANCED_PROP(float, _SizeItemX)
    UNITY_DEFINE_INSTANCED_PROP(float, _SizeItemY)
    UNITY_DEFINE_INSTANCED_PROP(float, _OutlineSize)

UNITY_INSTANCING_BUFFER_END(Props)


struct VertexIn
{
    float4 vertex : POSITION;
    float3 normal : NORMAL;
    float2 uv : TEXCOORD0;

    CE_SETUP_V_INPUT_NORMALMAP
    CE_SETUP_V_INPUT_VERTEXCOLOR
    UNITY_VERTEX_INPUT_INSTANCE_ID
};


struct VertexInOutLine
{
    float4 vertex : POSITION;
    float3 normal : NORMAL;
    float4 color : COLOR;
    float2 uv : TEXCOORD0;

    UNITY_VERTEX_INPUT_INSTANCE_ID
};


struct VertexOut
{
    float4 pos : SV_POSITION;
    float3 normal : NORMAL;
    float2 uv : TEXCOORD0;
    SHADOW_COORDS(1)
    float4 worldPos : TEXCOORD2;

    CE_SETUP_V2F_NORMALMAP
    CE_SETUP_V2F_VERTEXCOLOR
    CE_SETUP_V2F_RIMLIGHT_ANIM
    CE_SETUP_V2F_REFLECTION_ANIM

    float3 ambient : TEXCOORD7;
    float4 vertex : TEXCOORD8;
    float3 screenPos : TEXCOORD9;
};


//----------------------------------------
//
//	Vertex Shader
//
//----------------------------------------

VertexOut vert(VertexIn v)
{
    VertexOut o;

    UNITY_SETUP_INSTANCE_ID(v);

    o.worldPos = mul(UNITY_MATRIX_M, v.vertex);
    o.pos = mul(UNITY_MATRIX_V, o.worldPos);
    o.pos = mul(UNITY_MATRIX_P, o.pos);
    o.screenPos = o.pos.xyz / o.pos.w;
    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
    o.vertex = v.vertex;

    o.normal = UnityObjectToWorldNormal(v.normal);

    CE_SETUP_VIEWNORMAL_VS(o.normal);

    CE_SETUP_TANGENTBINORMAL_VS(o.normal);

    o.ambient = ShadeSH9(half4(o.normal, 1)).rgb;

    CE_SETUP_VERTEXCOLOR_VS;

    TRANSFER_SHADOW(o)

    return o;
}


VertexOut vert_outline(VertexInOutLine v)
{
    VertexOut o;

    UNITY_SETUP_INSTANCE_ID(v);

    float outLineSize = v.color.r * UNITY_ACCESS_INSTANCED_PROP(Props, _OutlineSize);

    float4 scaledPos = v.vertex;
    scaledPos.xyz += v.normal * outLineSize * 0.1;
    scaledPos = UnityObjectToClipPos(scaledPos);
    scaledPos /= scaledPos.w;
    o.pos = scaledPos;

    scaledPos.xy *= _ScreenParams.xy / 2;
    float4 originalPos = UnityObjectToClipPos(v.vertex);
    originalPos /= originalPos.w;
    originalPos.xy *= _ScreenParams.xy / 2;

    float2 scaledVec = scaledPos.xy - originalPos.xy;
    float scaledLength = length(scaledVec);
    scaledLength = min(scaledLength, _OutlineMaxPixel);
    scaledVec = normalize(scaledVec) * scaledLength;
    scaledVec.xy /= _ScreenParams.xy / 2;
    originalPos.xy /= _ScreenParams.xy / 2;
    o.pos.xy = originalPos.xy + scaledVec.xy;

    o.screenPos.z = o.pos.z;

    o.normal = UnityObjectToWorldNormal(v.normal);
    o.ambient = ShadeSH9(half4(o.normal, 1)).rgb;

    o.uv = TRANSFORM_TEX(v.uv, _MainTex);

    return o;
}


//----------------------------------------
//
//	Fragment Shader
//
//----------------------------------------

fixed4 frag(VertexOut i) : SV_Target0
{
    float4 outCol = float4(0.0, 0.0, 0.0, 0.0);
    float4 texCol = tex2D(_MainTex, i.uv);

    // texCol.rgb			*= _ColorMapBrightness;

    CE_APPLY_VERTEXCOLOR(texCol);

    float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz);

    CE_GET_BUMPMAP_NORMAL(i.normal, normal);

    float3 ambient = i.ambient;

    float attenuation = 1.0;

    CE_GET_OCCLUSION(attenuation);
    fixed shadow = SHADOW_ATTENUATION(i);
    attenuation = min(shadow.r, attenuation);

    CE_GET_METALLICROUGHNESS(metallic, roughness);

    //  Get Light Indices
    float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
    float3 lightCol = _LightColor0;

    //  Diffuse
    float4 diffuseCol = float4(0.0, 0.0, 0.0, 0.0);

    float dotNL = dot(normal, lightDir) + _LightAttenuationThreshold;
    float lightAtten = saturate(min(dotNL, attenuation)) * _LightShadeHardness;
    lightAtten = clamp(lightAtten, _LightShadeBottom, 1.0);
    diffuseCol.rgb = lightCol * lightAtten;
    diffuseCol.a = max(diffuseCol.a, lightAtten);

    float3 shadeColor = pow(texCol.rgb, _LightShadeColorRate) * ambient * _LightAmbientRate;
    float3 lightColor = texCol.rgb * (diffuseCol + ambient * _LightAmbientRate);
    outCol.rgb = lerp(shadeColor, lightColor, diffuseCol.a);
    outCol.a = texCol.a;

    CE_GET_EMISSION(i.uv, outCol.rgb)

    //	Culc specular
    float3 specularCol = float3(0, 0, 0);

    float3 reflectDir = reflect(-lightDir, normal);
    float specularDensity = max(dot(reflectDir, viewDirection), 0.0);
    specularDensity = pow(specularDensity, 0.001 + roughness * 50.0);
    specularDensity = clamp(specularDensity * attenuation * UNITY_ACCESS_INSTANCED_PROP(Props, _SpecularHardness), 0,
                            1);
    specularCol += pow(texCol.rgb, _SpecularColorRate) * specularDensity * metallic * lightAtten;

    CE_GET_REFLECTION_COLOR_ANIM(outCol.rgb);

    outCol.rgb += specularCol;

    CE_APPLY_RIMLIGHT_ANIM(i.viewNormal, attenuation, outCol.rgb);

    
    return float4(outCol.rgb * _Color2.rgb * UNITY_ACCESS_INSTANCED_PROP(Props, _ColorMapBrightness), outCol.a);
}


// fixed4 fragInFront( VertexOut i ) : SV_Target0 {
//
// 	float4 texCol		= float4( 0, 0, 0, 1 );
// 	
// 	if ( UNITY_ACCESS_INSTANCED_PROP( Props, _Hide ) == 1 )
// 	{
// 		texCol = _HideColor;
// 	}
// 	else if ( UNITY_ACCESS_INSTANCED_PROP( Props, _IndexGoldTexture ) >= 0 && UNITY_ACCESS_INSTANCED_PROP( Props, _IndexGoldTexture ) < 3 )
// 	{
// 		// 0.4
// 		float xUV = i.vertex.x / UNITY_ACCESS_INSTANCED_PROP( Props, _SizeItemX ) + 0.5;
// 		float yUV = i.vertex.y / UNITY_ACCESS_INSTANCED_PROP( Props, _SizeItemY );
// 		if ( UNITY_ACCESS_INSTANCED_PROP( Props, _IndexGoldTexture ) == 0)
// 		{
// 			texCol = tex2D( _GoldenItemTexture1, float2( xUV, yUV ) );
// 		}
// 		else if ( UNITY_ACCESS_INSTANCED_PROP( Props, _IndexGoldTexture ) == 1 )
// 		{
// 			texCol = tex2D( _GoldenItemTexture2, float2( xUV, yUV ) );
// 		}
// 		else if ( UNITY_ACCESS_INSTANCED_PROP( Props, _IndexGoldTexture ) == 2 )
// 		{
// 			texCol = tex2D( _GoldenItemTexture3, float2( xUV, yUV ) );
// 		}
// 		
// 		texCol= texCol*texCol.a;
// 	}
// 	else {
// 		texCol		= tex2D( _MainTex, i.uv );
// 	}
// 				
// 	CE_APPLY_VERTEXCOLOR( texCol );
//
// 	if ( UNITY_ACCESS_INSTANCED_PROP( Props, _Hide ) == 1 || ( UNITY_ACCESS_INSTANCED_PROP( Props, _IndexGoldTexture ) >= 0 && UNITY_ACCESS_INSTANCED_PROP( Props, _IndexGoldTexture ) < 3 ) )
// 	{
// 		return float4( texCol.rgb * UNITY_ACCESS_INSTANCED_PROP( Props, _ColorMapBrightness ), texCol.a );
// 	}
//
// 	return float4( 0.0, 0.0, 0.0, 0.0 );
// }


fixed4 frag_outline(VertexOut i) : SV_Target0
{
    float3 outCol = lerp(_OutLineBaseColor.rgb, i.ambient, _OutLineBlendRateAmbient);
    float3 texCol = tex2D(_MainTex, i.uv).rgb;
    outCol = lerp(outCol, texCol, _OutLineBlendRateColorMap);

    return fixed4(outCol, 1.0);
}
