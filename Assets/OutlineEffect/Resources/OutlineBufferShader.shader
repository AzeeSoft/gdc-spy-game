/*
//  Copyright (c) 2015 José Guerreiro. All rights reserved.
//
//  MIT license, see http://www.opensource.org/licenses/mit-license.php
//  
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
//  
//  The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
//  
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//  THE SOFTWARE.
*/

Shader "Hidden/OutlineBufferEffectOld" {
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
	}

	SubShader
	{ 
		Tags
		{
			"Queue" = "Geometry"
			"RenderType" = "Opaque"
		}

		ZTest Less

		// Change this stuff in OutlineEffect.cs instead!
		ZWrite Off
		//Blend One OneMinusSrcAlpha
		Cull [_Culling]
		Lighting Off
			
		CGPROGRAM

		#pragma surface surf Lambert vertex:vert 
		#pragma multi_compile _ PIXELSNAP_ON
		#include "UnityCG.cginc"

		sampler2D _MainTex;
		float4 _MainTex_ST;
		fixed4 _Color;
		float _OutlineAlphaCutoff;
		
        sampler2D _CameraDepthTexture;

		struct Input
		{
			float4 position : SV_POSITION;
			float2 uv : TEXCOORD0;
			float4 projPos : TEXCOORD1;
			float3 worldPos : TEXCOORD2;
			//fixed4 color;
		};

		void vert(inout appdata_full v, out Input o)
		{			
			/* #if defined(PIXELSNAP_ON)
			v.vertex = UnityPixelSnap(v.vertex);
			#endif */

			// UNITY_INITIALIZE_OUTPUT(Input, o);
			o.position = UnityObjectToClipPos(v.vertex);
			o.uv = v.texcoord;
			o.projPos = ComputeScreenPos(o.position);
			o.worldPos = mul(unity_ObjectToWorld, v.vertex);
            // COMPUTE_EYEDEPTH(o.eyeDepth);
			//o.color = v.color;
		}

		void surf(Input IN, inout SurfaceOutput o)
		{
			// float sceneDepthSample = tex2D(_CameraDepthTexture, UnityStereoScreenSpaceUVAdjust(IN.uv, 1));
			// float sceneDepthSample = tex2D(_CameraDepthTexture, IN.uv);

			// fixed4 c = tex2D(_MainTex, IN.uv_MainTex);// * IN.color;
			// if (c.a < _OutlineAlphaCutoff) discard;

			float rawZ = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(IN.projPos));
			// float rawZ = tex2D(_CameraDepthTexture, IN.uv_MainTex);;
            float sceneZ = LinearEyeDepth(rawZ);
            // float partZ = IN.eyeDepth;



			// float alpha = c.a * 99999999;

			// o.Albedo = rawZ * alpha;

			float testDepth = rawZ;
			// float testDepth = IN.projPos.z;

			if (IN.projPos.z < testDepth) {
				discard;
			}


			o.Albedo = 0;
			// o.Albedo.w = IN.projPos.w;
			// o.Albedo = float3(rawZ, rawZ, rawZ);
			// o.Albedo = float3(sceneZ, sceneZ, sceneZ);
			// o.Albedo = float3(partZ, partZ, partZ);
			o.Alpha = 1;
			o.Emission = _Color;
		}

		ENDCG		
	}

	Fallback "Transparent/VertexLit"
}
