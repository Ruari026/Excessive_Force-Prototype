Shader "Custom/GooDisplacementShader"
{
    Properties
    {
		_NoiseTexture("Noise", 2D) = "white" {}
		
		_NormalDisplacement("Normal Displacement Strength", Range(0, 1)) = 0.2
		_OffsetDisplacement("Offset Displacement Strength", Range(0, 1)) = 0.35

        _MainColor ("Main Color", Color) = (1,1,1,1)
		_RimColor ("Rim Color", Color) = (1,1,1,1)

        _Glossiness ("Smoothness", Range(0,1)) = 1.0
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
		Tags { "RenderType" = "Opaque" }
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

		sampler2D _NoiseTexture;
		
		float3 _GooOrigin;
		float3 _GooFloor;
		float _NormalDisplacement;
		float _OffsetDisplacement;

		fixed4 _MainColor;
		fixed4 _RimColor;
		float3 _CameraPos;

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
			float3 newPos = v.vertex.xyz;
			float3 worldPos = mul(unity_ObjectToWorld, float4(newPos.xyz, 1.0)).xyz;

			// Normal Displacement
			float2 right = float2(0, 1);
			right = (normalize(right));
			float2 vPos = float2(v.vertex.x, v.vertex.y);
			vPos = (normalize(vPos));

			float angle = dot(right, vPos);
			float noise = tex2Dlod(_NoiseTexture, float4(angle, 0, 0, 0));
			noise = (noise * 2) / 1;

			float3 normalDisplacement = v.normal;
			normalDisplacement *= _NormalDisplacement * noise;

			// Offset Displacement
			float noiseX = tex2Dlod(_NoiseTexture, float4(0, (angle / 2), 0, 0));
			noiseX = (noiseX * 2) - 1;
			float noiseY = tex2Dlod(_NoiseTexture, float4(0.5, (angle / 2), 0, 0));
			noiseY = (noiseY * 2) - 1;

			float3 offsetDisplacement = float3(noiseX, 0, noiseY);
			if (length(offsetDisplacement) > 1)
			{
				offsetDisplacement = normalize(offsetDisplacement);
			}
			offsetDisplacement *= _OffsetDisplacement;

			// Displacement Greater From Origin
			float maxDist = distance(_GooFloor, _GooOrigin);
			float dist = distance(worldPos, _GooOrigin.xyz);
			float power = (dist / maxDist);
			if (power > 1)
			{
				power = 1;
			}
			power = (power * power * power);

			float3 displacement = (normalDisplacement + offsetDisplacement);
			newPos = newPos + (displacement * power);
			

			// Finalising Position
			o.vertexPosition = mul(unity_ObjectToWorld, float4(newPos.xyz, 1.0)).xyz;
			v.vertex.xyz = newPos;

			// Normal
			o.vertexNormal = mul(unity_ObjectToWorld, float4(v.normal.xyz, 0.0)).xyz;
		}

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			float3 rotatedPos = mul(unity_ObjectToWorld, float4(IN.vertexPosition.xyz, 0.0)).xyz;

			// Rim Shading
			float3 cameraDir = (IN.vertexPosition - _CameraPos);
			cameraDir = normalize(cameraDir);

			float3 normalDir = (IN.vertexNormal);
			normalDir = normalize(normalDir);

			float angle = dot(IN.vertexNormal, cameraDir);
			angle = (angle + 1) / 2;
			angle = (angle * angle);

			float3 newColor = lerp(_MainColor, _RimColor, angle);
			o.Albedo = newColor.xyz;
			o.Alpha = 1.0;

            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
			o.Emission = 0;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
