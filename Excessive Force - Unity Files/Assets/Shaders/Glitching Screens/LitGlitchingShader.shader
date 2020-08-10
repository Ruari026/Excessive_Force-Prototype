Shader "Custom/Lit/GlitchingEffect"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

		_NoiseTex("Noise Texture", 2D) = "white" {}
		_TrashTex("Trash Texture", 2D) = "white" {}
		_Intensity("Intensity", float) = 0.0

		_HexMask("Hex Mask Texture", 2D) = "white" {}
    }

    SubShader
    {
		Tags {"Queue" = "Transparent" "RenderType" = "Transparent" }
		LOD 200

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

        
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows alpha:fade

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

		sampler2D _NoiseTex;
		sampler2D _TrashTex;
		float _Intensity;

		sampler2D _HexMask;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			// Get the color of the generated glitch texture
			float4 glitch = tex2D(_NoiseTex, IN.uv_MainTex);
			

			float thresh = 1.001 - _Intensity * 1.001;
			float w_d = step(thresh, pow(glitch.z, 2.5)); // displacement glitch
			float w_f = step(thresh, pow(glitch.w, 2.5)); // frame glitch
			float w_c = step(thresh, pow(glitch.z, 3.5)); // color glitch

			// Displacement
			float2 uv = frac(IN.uv_MainTex + glitch.xy * (0.1 * _Intensity * glitch.z));

			float4 source = tex2D(_MainTex, uv);
			source = source * _Color;

			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;

			// Shuffle color components.
			float3 color = lerp(source, tex2D(_TrashTex, uv), w_f).rgb;
			float3 neg = saturate(glitch.grb + (1 - dot(color, 1)) * 0.5);
			color = lerp(source, neg, w_c);


			// Finishing Up
			// apply fog
			UNITY_APPLY_FOG(i.fogCoord, color);
            o.Albedo = float4(color, source.a);

            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = source.a * tex2D(_HexMask, IN.uv_MainTex).a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
