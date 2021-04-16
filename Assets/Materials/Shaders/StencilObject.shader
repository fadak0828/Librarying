Shader "Stencils/StencilObject" {
	Properties {
		[IntRange] _StencilMask("Stencil Mask", Range(0, 255)) = 0

		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
        [Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp ("StencilComp", Int) = 3
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
        Stencil
        {
            Ref 1
            //Comp notEqua    //이 경우는, 영역을 벗어나야 렌더링이 됩니다. 
            Comp [_StencilComp]   //스탠실 영역과 겹치면 나오게 됩니다. 
            Pass keep
        }
		CGPROGRAM

		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 2.0 target, to get nicer looking lighting
		#pragma target 2.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) {

			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;

			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
