Shader "A Rat King/SelfIllumTransparentZwrite" {
	Properties {
		_Color ("Main Color", Color) = (0.5, 0.5, 0.5, 0.5)
		_Emission ("Emission Factor", float) = 0.2 // (0.5, 0.5, 0.5, 0.5)
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		//Tags {"Queue"="Geometry" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 200
		// Lighting On
		
		Pass {
		Zwrite on
		ColorMask 0
		}
        
		CGPROGRAM
		#pragma surface surf Lambert alpha
		
		sampler2D _MainTex;
		float4 _Color;
		float _Emission;
		
		struct Input {
			float2 uv_MainTex;
		};
		
		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Emission = c * _Emission;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Transparent/Diffuse"
}
