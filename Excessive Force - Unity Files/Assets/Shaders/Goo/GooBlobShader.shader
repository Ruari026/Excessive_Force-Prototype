Shader "Custom/GooBlobShader"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)

		_RimColor("Rim Color", Color) = (1,1,1,1)

		_EffectRange("Effect Range", Range(0, 10)) = 0
        _NoiseTexture ("Noise", 2D) = "white" {}
		_NoisePower("Power", Range(0, 50)) = 0
		_Strength("Strength", Range(0, 10)) = 0

        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
		Tags { "RenderType" = "Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        struct Input
        {
			float3 vertexPosition;
			float3 vertexNormal;
        };

		fixed4 _Color;
		float _EffectRange;

		fixed4 _RimColor;
		float3 _CameraPos;

		sampler2D _NoiseTexture;
		float _NoisePower;
		float _Strength;

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

			if (newPos.y > -_EffectRange && newPos.y < _EffectRange)
			{
				float noise = tex2Dlod(_NoiseTexture, float4((newPos.z / _NoisePower), (newPos.x / _NoisePower), 0, 0));
				noise = (noise * 2) - 1;

				float3 displacement = (float3(0, 1, 0));
				displacement *= (noise * _Strength);

				newPos = (newPos + displacement);
			}

			// Finalising Position
			o.vertexPosition = mul(unity_ObjectToWorld, float4(newPos.xyz, 1.0)).xyz;
			v.vertex.xyz = newPos;

			// Normal
			o.vertexNormal = mul(unity_ObjectToWorld, float4(v.normal.xyz, 0.0)).xyz;
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

			float4 newColor = lerp(_Color, _RimColor, angle);
			o.Albedo = newColor.xyz;
			o.Alpha = newColor.w;

			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
