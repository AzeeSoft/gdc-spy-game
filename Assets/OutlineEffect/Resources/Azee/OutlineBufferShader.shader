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
		sampler2D _OutlineDepth;
		int _SeeThrough;
		
        sampler2D _CameraDepthTexture;

		struct Input
		{
			float4 position : SV_POSITION;
			float2 uv : TEXCOORD0;
			float4 projPos : TEXCOORD1;
			// float3 worldPos : TEXCOORD2;
			//fixed4 color;
		};

		bool passesZTest(half4 outlineDepth)
		{
			return (outlineDepth.x == 1 && outlineDepth.y == 1 && outlineDepth.z == 1);				
		}

		void vert(inout appdata_full v, out Input o)
		{			
			/* #if defined(PIXELSNAP_ON)
			v.vertex = UnityPixelSnap(v.vertex);
			#endif */

			o.position = UnityObjectToClipPos(v.vertex);
			o.uv = v.texcoord;
			o.projPos = ComputeScreenPos(o.position);
			// o.worldPos = mul(unity_ObjectToWorld, v.vertex);
		}

		void surf(Input IN, inout SurfaceOutput o)
		{
			if (!_SeeThrough)
			{
				half4 outlineDepth = tex2Dproj(_OutlineDepth, UNITY_PROJ_COORD(IN.projPos));

				bool zTestPass = passesZTest(outlineDepth);
				if (!zTestPass)
				{
					discard;
				}
			}

			o.Albedo = 0;
			o.Alpha = 1;
			o.Emission = _Color;
		}

		ENDCG		
	}

	Fallback "Transparent/VertexLit"
}
