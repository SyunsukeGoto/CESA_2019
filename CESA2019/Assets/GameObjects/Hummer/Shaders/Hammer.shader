//
// Hammer.shader
// Actor: Tamamura Shuuki
// 

// ハンマー用シェーダー
Shader "Custom/Hammer"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white" {}
		_SourceTex("Source texture", 2D) = "white"{}
		_Threshold("Threshold", Float) = 0
		_Intensity("Intensity", Range(0, 10)) = 0
	}
		SubShader
	{
		CGINCLUDE
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

		struct appdata
	{
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
		float3 normal : NORMAL;
	};

	struct v2f
	{
		float2 uv : TEXCOORD0;
		float4 vertex : SV_POSITION;
		float3 normal : NORMAL;
		float3 viewDir : TEXCOORD1;
		float3 lightDir : TEXCOORD2;
	};

	sampler2D _MainTex;				// メインで使用
	sampler2D _SourceTex;			// 元画像（最後の合成用）
	float4 _MainTex_TexelSize;		// テクセルサイズ
	float _Threshold;				// ブルームに適用したい色の閾値（敷地より大きい色のみブルームの対象となる）
	float _Intensity;				// 光の強度

									// ぼかしに使用 ---------------------------------------------
									// メインテクスチャのRGBのみをサンプリングする
	float3 sampleMain(float2 uv)
	{
		return tex2D(_MainTex, uv).rgb;
	}

	// 対角線上の4点からサンプリングした色の平均値を返す
	float3 sampleBox(float2 uv, float delta)
	{
		float4 offset = _MainTex_TexelSize.xyxy * float2(-delta, delta).xyxy;
		float3 sum = sampleMain(uv + offset.xy) + sampleMain(uv + offset.zy) + sampleMain(uv + offset.xw) + sampleMain(uv + offset.zw);
		return sum / 4;
	}
	// ----------------------------------------------------------

	// 明度の取得
	float getBrightness(float4 color)
	{
		return max(color.r, max(color.g, color.b));
	}

	// 頂点シェーダを各パス共通で使用
	v2f vert(appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = v.uv;
		o.normal = UnityObjectToWorldNormal(v.normal);
		o.viewDir = WorldSpaceViewDir(v.vertex);
		o.lightDir = WorldSpaceLightDir(v.vertex);
		return o;
	}
	ENDCG

	Tags
	{
		"RenderType" = "Opaque"
	}

		// ブルームを適用するピクセル抽出用
		Pass
	{
	CGPROGRAM
	float4 frag(v2f i) : SV_Target
	{
		float4 col = 1;
		col.rgb = sampleBox(i.uv, 1);
		float brightness = getBrightness(col);

		// 明度が閾値（_Threshold）より大きいピクセルだけをブルームの対象にする
		float contribution = max(0, brightness - _Threshold);
		contribution /= max(brightness, 0.00001);

		return col;
	}
	ENDCG
	}

		// ダウンサンプリング用
		// ダウンサンプリング時には1ピクセル分ずらした対角線上の4点からサンプリング
		Pass
		{
			//Blend One One

			CGPROGRAM
			float4 frag(v2f i) : SV_Target
			{
				float4 col = 1;
				col.rgb = sampleBox(i.uv, 1.0);

				return col;
			}
			ENDCG
		}

		// アップサンプリング用
		// アップサンプリング時には0.5ピクセル分ずらした対角線上の4点からサンプリング
		Pass
		{
			Blend One One

			CGPROGRAM
			float4 frag(v2f i) : SV_Target
			{
				float4 col = 1;
				col.rgb = sampleBox(i.uv, 0.5);
				return col;
			}
			ENDCG
		}

		// 元画像との合成用
		Pass
		{
			CGPROGRAM
			float4 frag(v2f i) : SV_Target
			{
				float4 col = tex2D(_SourceTex, i.uv);
				col.rgb += sampleBox(i.uv, 0.5) * _Intensity;
				return col;
			}
			ENDCG
		}

		// ハンマーに適用させるエミッション
		Pass
		{
			//Cull Front
			//Cull Front
			Blend One One

			CGPROGRAM
			float4 frag(v2f i) : SV_Target
			{
				float4 col = tex2D(_SourceTex, i.uv);
				col.rgb += sampleBox(i.uv, 0.5) * _Intensity;
				float4 rimColor = float4(1, 1, 1, 1);
				float rim = 1 - saturate(abs(dot(i.normal, i.viewDir)));
				float4 emission = (rimColor * rim) * _Intensity;
				col.rgb *= emission.rgb;
				return col;
			}
			ENDCG
		}

		// ハンマーに頂点ライティングを適用
		//Pass
		//{
		//	//Cull Off
		//	//ZWrite Off
		//	//Cull Front
		//	//Blend SrcAlpha OneMinusSrcAlpha

		//	CGPROGRAM
		//	float4 frag(v2f i) : SV_Target
		//	{
		//		float4 col = tex2D(_SourceTex, i.uv);
		//		float light = dot(i.normal, i.lightDir);
		//		col.rgb *= light;
		//		return col;
		//	}
		//	ENDCG
		//}
	}
}
