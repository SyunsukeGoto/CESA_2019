Shader "Custom/EmissionShader"
{
    Properties
    {
        /*_Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0*/
		_MainColor("Main Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_EmissionColor ("Emission Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_EmissionTex ("Emission Texture", 2D) = "white" {}
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		//#pragma surface surf Standard fullforwardshadows
		#pragma surface surf Lambert 
		#pragma shader_feature _EMISSION

		// Use shader model 3.0 target, to get nicer looking lighting
		//#pragma target 3.0

		float _Emission00FN;
		half4 _MainColor;
		half4 _EmissionColor;
        sampler2D _MainTex;
		sampler2D _EmissionTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        /*half _Glossiness;
        half _Metallic;
        fixed4 _Color;*/

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        //UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        //UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {
            // Albedo comes from a texture tinted by color
            //fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            //o.Albedo = c.rgb;
            //// Metallic and smoothness come from slider variables
            //o.Metallic = _Metallic;
            //o.Smoothness = _Glossiness;
            //o.Alpha = c.a;

			half4 c = tex2D(_MainTex, IN.uv_MainTex) * _MainColor;
			float t = ((2 * _SinTime.w * _CosTime.w) + 1.0) * 0.5;
			//float e = tex2D(_EmissionTex, IN.uv_MainTex).a * t;
			float e = tex2D(_EmissionTex, IN.uv_MainTex).a * _Emission00FN;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			o.Emission = _EmissionColor * e;
			//o.Emission = _Emission00FN;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
