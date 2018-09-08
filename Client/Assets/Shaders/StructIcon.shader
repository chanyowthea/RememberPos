Shader "Sandbox/Icon/Struct" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_EmissionColor ("Emission Color", Color) = (0,0,0,0)
	}
	SubShader {
		Tags { "RenderType"="Opaque" "Queue" ="Geometry"}
		ColorMask RGBA
		LOD 200
		ZWrite On
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		fixed4 _EmissionColor;

		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			//o.Emission = _EmissionColor;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
