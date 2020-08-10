// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/GooPuddleShader"
{
	Properties
	{
		_MainColor("Main Color", Color) = (1,1,1,1)
		_Emission("Emission", float) = 0.0

		_RimColor("Rim Color", Color) = (1,1,1,1)

		_Pulse ("Pulse", Range(0, 10)) = 0.0
		_Offset("Offset", Range(0,10)) = 0.0
		_MaxHeight ("Max Height", Range(0, 10)) = 0.0

		_NoiseTexture("Noise Texture", 2D) = "white" {}
		_NoiseMultiplier("Noise Multiplier", float) = 1.0
		

        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
		#pragma vertex vert
		#pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        struct Input
        {
			float3 vertexPosition;
			float3 vertexNormal;
        };
		
		fixed4 _MainColor;
		float _Emission;

		fixed4 _RimColor;
		float3 _CameraPos;

		int _Pulse;
		float _Offset;
		float _MaxHeight;

		sampler2D _NoiseTexture;
		float _NoiseMultiplier;

        half _Glossiness;
        half _Metallic;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			float3 endPos = v.vertex.xyz;

			// Radial Pulse
			float2 center = float2(0.5, 0.5);
			float2 targetPoint = v.texcoord;

			float dist = distance(center, targetPoint);
			float maxDist = distance(float2(0.0, 0.0), center);
			dist = (dist / 0.5);

			float radians = (dist * 3.14);
			radians = ((radians * _Pulse) - _Offset);

			float amount = sin(radians) + 1;
			amount = amount / 2;

			// Vertex Offset
			float pulseEffect = (dist / maxDist);
			pulseEffect = ((pulseEffect - 2) * -1);
			float3 pulseAddition = (float3(0, 0, 1) * (amount * _MaxHeight) * pulseEffect);
			
			// Adding Noise
			float noise = tex2Dlod(_NoiseTexture, v.texcoord);
			float3 noiseAddition = (float3(0, 0, 1) * (noise * _NoiseMultiplier));

			// Finalising Position
			endPos = (endPos + pulseAddition + noiseAddition);

			o.vertexPosition = mul(unity_ObjectToWorld, float4(endPos.xyz, 1.0)).xyz;
			v.vertex.xyz = endPos;

			// Deriving New Normal
			float2 baseNormalDirection = float2(0, 1);
			float rotationAmount = cos(radians);
				 
			// Rotating Based on Radial Pulse Height
			float2 rotatedRadialNormal = float2(0, 0);
			rotatedRadialNormal.x = ((cos(rotationAmount) * baseNormalDirection.x) - (sin(rotationAmount) * baseNormalDirection.y));
			rotatedRadialNormal.y = ((sin(rotationAmount) * baseNormalDirection.x) + (cos(rotationAmount) * baseNormalDirection.y));

			// Rotating Based On Vertex Position Relative to Center
			float2 directionRight = float2(1.0, 0.0);
			float2 directionUp = float2(0.0, 1.0);
			float2 directionVertex = float2((targetPoint.x - center.x), (targetPoint.y - center.y));

			float2 rotatedVertexNormal = float2(0.0, rotatedRadialNormal.x);

			float rotationAmountX = (dot(directionRight, directionVertex)) / (length(directionRight) * length(directionVertex));
			rotationAmountX = atan(rotationAmountX);
			rotatedVertexNormal.x = ((cos(rotationAmountX + 3.14) * 0.0) - (sin(rotationAmountX + 3.14) * rotatedRadialNormal.x));

			float rotationAmountY = (dot(directionUp, directionVertex)) / (length(directionUp) * length(directionVertex));
			rotationAmountY = atan(rotationAmountY);
			rotatedVertexNormal.y = ((sin(rotationAmountY - (3.14 / 2)) * 0.0) + (cos(rotationAmountY - (3.14 / 2)) * rotatedRadialNormal.x));


			// Calculating Final
			float3 finalNormal = float3(rotatedVertexNormal.x, rotatedVertexNormal.y, rotatedRadialNormal.y);
			finalNormal = normalize(finalNormal);
			o.vertexNormal = finalNormal;
		}

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			// Basic Rim Shader Coloring
			float3 cameraDir = (IN.vertexPosition - _CameraPos);
			cameraDir = normalize(cameraDir);

			float3 normalDir = (IN.vertexNormal);
			normalDir = normalize(normalDir);

			float angle = dot(IN.vertexNormal, cameraDir);
			angle = (angle + 1) / 2;

			float3 newColor = lerp(_RimColor, _MainColor, angle);
			o.Albedo = newColor.xyz;
			o.Alpha = 1.0;

			// Normal Calculated in the Vertex Shader
 			o.Normal = IN.vertexNormal;

            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
