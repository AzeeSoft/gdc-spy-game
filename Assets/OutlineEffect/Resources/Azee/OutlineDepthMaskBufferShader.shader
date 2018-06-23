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

Shader "Hidden/OutlineDepthBuffer" {
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
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

		ZWrite Off
		Blend One Zero
		// Cull [_Culling]
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
			int _SeeThrough;

			sampler2D _CameraDepthTexture;
			float4 _CameraDepthTexture_ST;
			
			sampler2D _CameraNormalsTexture;
			float4 _CameraNormalsTexture_ST;

			struct v2f
			{
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 projPos : TEXCOORD1;
				float depth: TEXCOORD2;
				//fixed4 color;
			};

			v2f vert(appdata_full v)
			{
			   	v2f o;
				   
				o.position = UnityObjectToClipPos(v.vertex);
    			o.uv = TRANSFORM_TEX(v.texcoord, _CameraNormalsTexture);
				o.projPos = ComputeScreenPos(o.position);
				o.depth = COMPUTE_DEPTH_01;
				//o.color = v.color;

				return o;
			}

			half4 frag(v2f IN) : Color
			{
				bool zTestPass = true;
				
				if (!_SeeThrough)
				{
					// float rawZ = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(IN.projPos));
					float closestLinearDepth = Linear01Depth(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(IN.projPos)).r);
					// float testDepth = IN.projPos.z;

					if (IN.depth - closestLinearDepth > 0.000001) 
					{
						zTestPass = false;
					}		
				}

				return (zTestPass) ? half4(1, 1, 1, 1) : half4(0, 0, 0, 0);
			}

			ENDCG
		}	
	}

	Fallback "Transparent/VertexLit"
}
