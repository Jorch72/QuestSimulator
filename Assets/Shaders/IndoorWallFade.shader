Shader "Custom/IndoorWallFade" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_FadeMin ("Fade minimal", Float) = 0
		_FadeMax ("Fade maximal", Float) = 10
		_FadeColor ("Fade Color", Color) = (1,1,1,1)
	}
	SubShader{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
		LOD 100

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
	
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 4.0
	
		half _Glossiness;
		half _Metallic;
		sampler2D _MainTex;
		fixed4 _Color;
		half _FadeMin;
		half _FadeMax;
		fixed4 _FadeColor;
		
		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
			INTERNAL_DATA
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			float smoothness = _Glossiness;
			float metallic = _Metallic;
			float backfaceValue = 1 - clamp((IN.worldPos.y - _FadeMin) / (_FadeMax - _FadeMin), 0 ,1);

			o.Albedo = lerp(c.rgb, _FadeColor, 1 - backfaceValue);

			// Metallic and smoothness come from slider variables
			o.Metallic = metallic * backfaceValue;
			o.Smoothness = smoothness * backfaceValue;
			o.Alpha = c.a - (1 - backfaceValue);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
