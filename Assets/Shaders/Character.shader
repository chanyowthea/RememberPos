Shader "Sandbox/Character" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_MaxLuminance ("Max Luminance", float) = 1 
		_Cutoff ("Cut Off", float) = 0.5
	}
	SubShader {
		Tags { "RenderType"="Opaque" "Queue"="Geometry-1"}

		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert fullforwardshadows// alphatest:_Cutoff


		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		float4 _Color;
		float _MaxLuminance;

		struct Input {
			float2 uv_MainTex;
		};

		half3 ClampLuminance(half3 c, float maxLuminance)
		{
			float luminance = (c.r * 0.3f) + (c.g * 0.59f) + (c.b * 0.11f);

			return min(luminance, maxLuminance) / luminance * c;
		}

		half4 LightingWrapLambert (SurfaceOutput s, half3 lightDir, half atten) {
	        half NdotL = dot (s.Normal, lightDir);
	        half diff = NdotL * 0.5 + 0.5;
	        half4 c;	       	
	        c.rgb = ClampLuminance(s.Albedo * _LightColor0.rgb * diff, _MaxLuminance) *atten;
	        c.a = s.Alpha;
	        return c;
    	}

		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Emission = c.rgb * 0.1f;
			// Metallic and smoothness come from slider variables
			o.Alpha = c.a;
		}

		ENDCG
	}
	FallBack "Diffuse"
}
