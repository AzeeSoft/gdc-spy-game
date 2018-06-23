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

Shader "Hidden/OutlineBufferEffect" {
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
	}

	CGINCLUDE

        #pragma target 3.0

    ENDCG

	SubShader
	{ 
		Tags
		{
			"Queue" = "Geometry"
			"RenderType" = "Opaque"
		}

		ZTest Less

		// Change this stuff in OutlineEffect.cs instead!
		//ZWrite Off
		Blend One One
		Cull [_Culling]
		// Lighting Off

		Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;
			float _OutlineAlphaCutoff;

			sampler2D _CameraDepthTexture;

			struct v2f
			{
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 projPos : TEXCOORD1;
				//fixed4 color;
			};

			v2f vert(appdata_full v)
			{
			   	v2f o;

				/* #if defined(PIXELSNAP_ON)
				v.vertex = UnityPixelSnap(v.vertex);
				#endif */

				o.position = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				o.projPos = ComputeScreenPos(o.position);
				//o.color = v.color;

				return o;
			}

			half4 frag(v2f IN) : Color
			{
				// float sceneDepthSample = tex2D(_CameraDepthTexture, UnityStereoScreenSpaceUVAdjust(IN.uv, _MainTex_ST));
				// float sceneDepthSample = tex2D(_CameraDepthTexture, IN.uv);

				// fixed4 c = tex2D(_MainTex, IN.uv_MainTex);// * IN.color;
				// if (c.a < _OutlineAlphaCutoff) discard;

				float rawZ = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(IN.projPos));
				// float rawZ = tex2D(_CameraDepthTexture, IN.uv_MainTex);;
				float sceneZ = LinearEyeDepth(rawZ);
				// float partZ = IN.eyeDepth;



				// float alpha = c.a * 99999999;

				// o.Albedo = rawZ * alpha;

				// float testDepth = rawZ;
				float testDepth = IN.projPos.z;

				if (IN.projPos.z < testDepth) {
					// discard;
				}


				return _Color;
			}

			ENDCG
		}	
	}

	Fallback "Transparent/VertexLit"
}
