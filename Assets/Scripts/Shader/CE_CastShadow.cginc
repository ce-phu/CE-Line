//======================================
// Project My Neighborhood Emiko-san 
// Copyright (c) 2018 CubicEgg.Ltd 
//======================================
// File     : CE_CastShadow.cginc
// Author   : Okada Taizo
// Date     : 21st September, 2018
//======================================

//----------------------------------------
//
//	Input & Output Structure
//
//----------------------------------------


struct VertexInput {

	float4 vertex			: POSITION;

	UNITY_VERTEX_INPUT_INSTANCE_ID
};



struct VertexOutput {

	V2F_SHADOW_CASTER;
};



//----------------------------------------
//
//	Vertex Shader
//
//----------------------------------------



VertexOutput vert ( VertexInput v ) {

	VertexOutput o	= (VertexOutput)0;

	UNITY_SETUP_INSTANCE_ID( v );
	
    o.pos			= UnityObjectToClipPos( v.vertex );

	TRANSFER_SHADOW_CASTER( o )

	return o;
}



//----------------------------------------
//
//	Fragment Shader
//
//----------------------------------------

float4 frag( VertexOutput i ) : SV_Target {

	SHADOW_CASTER_FRAGMENT( i )
}
