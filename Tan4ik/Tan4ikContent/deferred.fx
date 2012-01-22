struct Light
{
	float3 position;
	float3 color;
	float invRadius;
};

texture normaltexture;

int numberOfLights;
Light lights[3];

float3 ambientColor;

float screenWidth;
float screenHeight;

sampler ColorMap : register(s0);

sampler NormalMap : samplerState
{
	Texture = normaltexture;
	MinFilter = Linear;
	MagFilter = Linear;
	AddressU = Clamp;
	AddressV = Clamp;
}; 

float3 CalculateLight(Light light, float3 normal,
					float3 pixelPosition)
{
	float3 direction = light.position - pixelPosition;
	float atten = length(direction);

	direction /= atten;
	float amount = max(dot(normal, direction), 0);

	atten *= light.invRadius;
	float modifer = max((1 - atten), 0);
	return light.color * modifer * amount;
}

float4 DeferredNormalPS(float2 texCoords : TEXCOORD0) : COLOR
{
	float4 base = tex2D(ColorMap, texCoords);
	float3 normal = normalize(tex2D(NormalMap, texCoords) * 2.0f - 1.0f);

	//return float4(normal, 1);

	float3 pixelPosition = float3(screenWidth * texCoords.x,
							screenHeight * texCoords.y,0);

	float3 finalColor = 0;

	for (int i=0;i<numberOfLights;i++)
	{
		finalColor += CalculateLight(lights[i], normal, pixelPosition);
	}

	return float4((ambientColor + finalColor) * base.rgb, base.a);
}

technique Deferred
{
    pass Pass0
    {
        PixelShader = compile ps_2_0 DeferredNormalPS();
    }
}