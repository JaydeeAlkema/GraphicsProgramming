#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

//float BlurDistance = 0.002f;

//sampler ColorMapSampler : register( s0 );

Texture2D _MainTex;
sampler2D _MainTexSampler = sampler_state
{
	Texture = <_MainTex>;
};

float4 InvertPS( float2 uv : VPOS) : COLOR
{
	uv = ( uv + 0.5 ) * float2( 1.0 / 1600.0, 1.0 / 900.0 );
	float4 color = tex2D( _MainTexSampler, uv );

	return color;
}

technique Invert
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL InvertPS();
	}
};

float4 ChromaticAberrationPS( float2 uv : VPOS) : COLOR
{
	uv = ( uv + 0.5 ) * float2( 1.0 / 1600.0, 1.0 / 900.0 );

	float strength = 5;
	float3 rgbOffset = 1 + float3( 0.01, 0.005, 0 ) * strength;
	float dist = distance( uv, float2( 0.5, 0.5 ) );
	float2 dir = uv - float2( 0.5, 0.5 );

	// scale rgbOffset & Renormalize
	rgbOffset = normalize( rgbOffset * dist );

	// Calculate uvs for each color channel
	float2 uvR = float2( 0.5, 0.5 ) + rgbOffset.r * dir;
	float2 uvG = float2( 0.5, 0.5 ) + rgbOffset.g * dir;
	float2 uvB = float2( 0.5, 0.5 ) + rgbOffset.b * dir;

	float4 colorR = tex2D( _MainTexSampler, uvR );
	float4 colorG = tex2D( _MainTexSampler, uvG );
	float4 colorB = tex2D( _MainTexSampler, uvB );

	return float4( colorR.r, colorG.g, colorB.b, 1 );
}

technique ChromaticAberration
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL ChromaticAberrationPS();
	}
};

//float4 MotionBlurPS( float2 Tex: TEXCOORD0) : COLOR
//{
//	float4 Color;

//	// Get the texel from ColorMapSampler using a modified texture coordinate. This
//	// gets the texels at the neighbour texels and adds it to Color.
//	Color = tex2D( ColorMapSampler, float2( Tex.x + BlurDistance, Tex.y + BlurDistance ) );
//	Color += tex2D( ColorMapSampler, float2( Tex.x - BlurDistance, Tex.y - BlurDistance ) );
//	Color += tex2D( ColorMapSampler, float2( Tex.x + BlurDistance, Tex.y - BlurDistance ) );
//	Color += tex2D( ColorMapSampler, float2( Tex.x - BlurDistance, Tex.y + BlurDistance ) );
//	// We need to devide the color with the amount of times we added
//	// a color to it, in this case 4, to get the avg. color
//	Color = Color / 4;

//	// returned the blurred color
//	return Color;
//}

//technique MotionBlur
//{
//	pass P0
//	{
//		PixelShader = compile PS_SHADERMODEL MotionBlurPS();
//	}
//};