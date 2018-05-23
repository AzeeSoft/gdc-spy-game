// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/XRayShader" 
{
	Properties 
	{
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader 
	{
		Tags { "Queue"="Transparent" }

		ZTest Always
		ZWrite Off
		Blend One One

		Pass 
		{

			CGPROGRAM
			#pragma vertex vert 
			#pragma fragment frag

			#include "UnityCG.cginc"
			
			struct appdata 
			{
				float4 vertex: POSITION;
			};

			struct v2f 
			{
				float4 vertex: SV_POSITION;
				float depth : DEPTH;
			};

			v2f vert (appdata v) 
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.depth = -mul(UNITY_MATRIX_MV, v.vertex).z * _ProjectionParams.w;
				return o;
			}

			half4 _XRayVisionColor;
			
			fixed4 frag (v2f i) : SV_TARGET 
			{
				return _XRayVisionColor;
			}

			ENDCG
		}
	}
	FallBack "Diffuse"
}
