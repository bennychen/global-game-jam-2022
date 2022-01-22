Shader "HyperCard/Side"
{
	Properties
	{
		// not used but required by SpriteRenderers
		[HideInInspector] _MainTex("_MainTex", 2D) = "black" {}

		// general settings
		[HideInInspector] _Stencil("_Stencil", Int) = 0
		[HideInInspector] _Seed("_Seed", Float) = 1
		[HideInInspector] _CardOpacity("_CardOpacity", Float) = 1
		// frame
		[HideInInspector] _FrameMap("_FrameMap", 2D) = "black" {}
		[HideInInspector] _FrameDiffuse("_FrameDiffuse", 2D) = "black" {}
		[HideInInspector] _FrameDiffuseColor("_FrameDiffuseColor", Color) = (0,0,0,0)
		[HideInInspector] _Artwork("_Artwork", 2D) = "black" {}
		// outline
		[HideInInspector] _IsOutlineEnabled("_IsOutlineEnabled", Int) = 0
		[HideInInspector] _OutlineWidth("_OutlineWidth", Float) = 1
		[HideInInspector] _OutlineHeight("_OutlineHeight", Float) = 1
		[HideInInspector] _OutlineSmoothness("_OutlineSmoothness", Float) = 1
		[HideInInspector] _OutlineSmoothSpeed("_OutlineSmoothSpeed", Float) = 1
		[HideInInspector] _OutlineTrimOffset("_OutlineTrimOffset", Float) = 1
		[HideInInspector] _OutlinePosOffset("_OutlinePosOffset", Vector) = (0,0,0,0)
		[HideInInspector] _OutlineStartColor("_OutlineStartColor", Color) = (0, 0, 0, 0)
		[HideInInspector] _OutlineEndColor("_OutlineEndColor", Color) = (0, 0, 0, 0)
		[HideInInspector] _OutlineEndColorDistance("_OutlineEndColorDistance", Float) = 1
		[HideInInspector] _OutlineColorExposure("_OutlineColorExposure", Float) = 1
		[HideInInspector] _OutlineNoiseFrequency("_OutlineNoiseFrequency", Float) = 1
		[HideInInspector] _OutlineNoiseSpeed("_OutlineNoiseSpeed", Float) = 1
		[HideInInspector] _OutlineNoiseMult("_OutlineNoiseMult", Float) = 1
		[HideInInspector] _OutlineNoiseOffset("_OutlineNoiseOffset", Float) = 1
		[HideInInspector] _OutlineNoiseThreshold("_OutlineNoiseThreshold", Float) = 1
		[HideInInspector] _OutlineNoiseDistance("_OutlineNoiseDistance", Float) = 1
		[HideInInspector] _OutlineNoiseVerticalAjust("_OutlineNoiseVerticalAjust", Float) = 1
		[HideInInspector] _CanvasMode("_CanvasMode", Float) = 0
		[HideInInspector] _CanvasOffsetX("_CanvasOffsetX", Float) = 0
		[HideInInspector] _CanvasOffsetY("_CanvasOffsetY", Float) = 0
		// dissolve
		[HideInInspector] _IsDissolveEnabled("_IsDissolveEnabled", Int) = 0
		[HideInInspector] _BurnNoiseFrequency("_BurnNoiseFrequency", Float) = 0
		[HideInInspector] _BurningAmount("_BurningAmount", Float) = 1
		[HideInInspector] _BurningRotateSpeed("_BurningRotateSpeed", Float) = 0
		[HideInInspector] _BurningOutline("_BurningOutline", Float) = 0
		[HideInInspector] _BurnStartColor("_BurnStartColor", Color) = (0,0,0,0)
		[HideInInspector] _BurnEndColor("_BurnEndColor", Color) = (0,0,0,0)
		[HideInInspector] _BurningOutline("_BurnExposure", Float) = 0
		[HideInInspector] _BurningRotateSpeed("_BurnAlphaCut", Float) = 0
		// distortion
		[HideInInspector] _Dist0_Enabled("_IsDistortionEnabled", Int) = 0
		[HideInInspector] _DistortionMap("_DistortionMap", 2D) = "black" {}
		[HideInInspector] _DistortionRedFrequency("_DistortionRedFrequency", Float) = 5
		[HideInInspector] _DistortionRedAmplitude("_DistortionRedAmplitude", Float) = 1
		[HideInInspector] _DistortionRedSpeed("_DistortionRedSpeed", Float) = 1
		[HideInInspector] _DistortionRedDirection("_DistortionRedDirection", Vector) = (0,0,0,0)
		[HideInInspector] _DistortionGreenFrequency("_DistortionGreenFrequency", Float) = 5
		[HideInInspector] _DistortionGreenAmplitude("_DistortionGreenAmplitude", Float) = 1
		[HideInInspector] _DistortionGreenSpeed("_DistortionGreenSpeed", Float) = 1
		[HideInInspector] _DistortionGreenDirection("_DistortionGreenDirection", Vector) = (0,0,0,0)
		[HideInInspector] _DistortionBlueFrequency("_DistortionBlueFrequency", Float) = 5
		[HideInInspector] _DistortionBlueAmplitude("_DistortionBlueAmplitude", Float) = 1
		[HideInInspector] _DistortionBlueSpeed("_DistortionBlueSpeed", Float) = 1
		[HideInInspector] _DistortionBlueDirection("_DistortionBlueDirection", Vector) = (0,0,0,0)
		// sprite sheet
		[HideInInspector] _IsSpriteSheetEnabled("_IsSpriteSheetEnabled", Int) = 0
		[HideInInspector] _SpriteSheetTexture("_SpriteSheetTexture", 2D) = "black" {}
		[HideInInspector] _SpriteSheetOffset("_SpriteSheetOffset", Vector) = (1,1,0,0)
		[HideInInspector] _SpriteSheetScale("_SpriteSheetScale", Vector) = (1,1,0,0)
		[HideInInspector] _SpriteSheetIndex("_SpriteSheetIndex", Int) = 0
		[HideInInspector] _SpriteSheetCols("_SpriteSheetCols", Float) = 1
		[HideInInspector] _SpriteSheetRows("_SpriteSheetRows", Float) = 1
		[HideInInspector] _SpriteSheetRemoveBlack("_SpriteSheetRemoveBlack", Int) = 0
		// glitter fx
		[HideInInspector] _IsGlitterFxEnabled("_IsGlitterFxEnabled", Int) = 0
		[HideInInspector] _GlitterUseArtworkCoords("_GlitterUseArtworkCoords", Int) = 0
		[HideInInspector] _GlitterColor("_GlitterColor", Color) = (1,1,1,1)
		[HideInInspector] _GlitterPower("_GlitterPower", Float) = 1
		[HideInInspector] _GlitterContrast("_GlitterContrast", Float) = 1
		[HideInInspector] _GlitterSpeed("_GlitterSpeed", Float) = 1
		[HideInInspector] _GlitterSize("_GlitterSize", Float) = 1
		[HideInInspector] _GlitterMask("_GlitterMask", 2D) = "black" {}
		[HideInInspector] _GlitterBackTex("_GlitterBackTex", 2D) = "white" {}
		[HideInInspector] _GlitterBackPower("_GlitterBackPower", Float) = 1
		[HideInInspector] _GlitterBackContrast("_GlitterBackContrast", Float) = 1
		[HideInInspector] _GlitterLight("_GlitterLight", Float) = 1
		[HideInInspector] _GlitterLightColor("_GlitterLightColor", Color) = (0,0,0,0)
		[HideInInspector] _GlitterLightRadius("_GlitterLightRadius", Float) = 1
		[HideInInspector] _GlitterSpeed("_GlitterOpacity", Float) = 0.5
		// color
		[HideInInspector] _SideColor("_SideColor", Color) = (1,1,1,1)
		// filters
		[HideInInspector] _BlackAndWhite("_BlackAndWhite", Int) = 0
	}

	SubShader
	{
		Tags 
		{
			"Queue" = "Transparent"
			// "RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"IgnoreProjector" = "True"
			"ForceNoShadowCasting" = "True"
			"DisableBatching" = "True"
		}

		Pass
		{
			Stencil {
				Ref [_Stencil]
				Comp NotEqual
				Pass Keep
			}

			Name "Outline"
		
			Cull Back
			Lighting Off
			Fog { Mode Off }
			ZWrite Off
			ZTest Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag

			#include "HyperCard_NoiseLib.cginc"
			#include "UnityCG.cginc"

			uniform float4 _TimeEditor;

			uniform float _Seed;
			uniform float _CardOpacity;

			uniform int _IsOutlineEnabled;

			uniform float _OutlineNoiseFrequency;
			uniform float _OutlineNoiseSpeed;
			uniform float _OutlineNoiseMult;
			uniform float _OutlineNoiseDistance;
			uniform float _OutlineNoiseOffset;
			uniform float _OutlineNoiseThreshold;
			uniform float _OutlineNoiseVerticalAjust;
			uniform float _OutlineWidth;
			uniform float _OutlineHeight;
			uniform float _OutlineSmoothness;
			uniform float _OutlineSmoothSpeed;
			uniform float _OutlineColorExposure;
			uniform float _OutlineTrimOffset;
			uniform float4 _OutlineStartColor;
			uniform float4 _OutlineEndColor;
			uniform float _OutlineEndColorDistance;
			uniform float2 _OutlinePosOffset;
			
			uniform float _CanvasMode;
			uniform float _CanvasOffsetX;
			uniform float _CanvasOffsetY;

			struct VertexInput 
			{
				float4 vertex	: POSITION;
				float2 uv0		: TEXCOORD0;
			};

			struct VertexOutput 
			{
				float4 vertex	: SV_POSITION;
				float2 uv0		: TEXCOORD0;
			};

			VertexOutput vert(VertexInput v) 
			{
				VertexOutput o = (VertexOutput) 0;
				o.uv0 = v.uv0;
		
		        if(!_CanvasMode)
		        {    
					float4 w = v.vertex;
					w.x += normalize(v.vertex).x * _OutlineWidth + _OutlinePosOffset.x;
					w.y += normalize(v.vertex).y * _OutlineHeight + _OutlinePosOffset.y;

		            o.vertex = UnityObjectToClipPos(w - float4(0, 0, -0.0001, 0));
                }
                else
                {
                    v.vertex.x *= _OutlineWidth * 10;
					v.vertex.y *= _OutlineHeight * 10;
                    v.vertex.x += _CanvasOffsetX;
                    v.vertex.y += _CanvasOffsetY;
                    v.vertex.z -= 0.0001;
                    				 
				    o.vertex = UnityObjectToClipPos(v.vertex);			 			
                }

				return o;
			}

			float4 frag(VertexOutput i) : SV_TARGET
			{
				float4 time = _Time + _TimeEditor;

				float4 col = float4(1, 1, 1, 1.0f);

				if (_IsOutlineEnabled == 1) 
				{				
					if(i.uv0.y < 0.01) discard;

					float smc = 1 - smoothstep(0.0, _OutlineEndColorDistance, i.uv0.x) * (1 - smoothstep(1 - _OutlineEndColorDistance, 1.0, i.uv0.x)) * smoothstep(0.0, _OutlineEndColorDistance, i.uv0.y) * (1 - smoothstep(1 - _OutlineEndColorDistance, 1.0, i.uv0.y));

					col.xyz = lerp(_OutlineStartColor.xyz, _OutlineEndColor.xyz, smc) * (_OutlineColorExposure * pow(_CardOpacity, 6));

					float trim = 1 - smoothstep(0.0, _OutlineTrimOffset, i.uv0.x) * (1 - smoothstep(1 - _OutlineTrimOffset, 1.0, i.uv0.x)) * smoothstep(0.0, _OutlineTrimOffset, i.uv0.y) * (1 - smoothstep(1 - _OutlineTrimOffset, 1.0, i.uv0.y));

					col.a = lerp(0, 1, trim);

					float sm = 1 - smoothstep(0.0, _OutlineSmoothness, i.uv0.x) * (1 - smoothstep(1 - _OutlineSmoothness, 1.0, i.uv0.x)) * smoothstep(0.0, _OutlineSmoothness, i.uv0.y) * (1 - smoothstep(1 - _OutlineSmoothness, 1.0, i.uv0.y));
					
					col.a = max(0, col.a - sm * _OutlineSmoothSpeed);

					float2 c = i.uv0 * _OutlineNoiseFrequency;

					c.y += _Time.y * _OutlineNoiseSpeed + _Seed;

					float ns = snoise(c + i.uv0) * _OutlineNoiseMult + _OutlineNoiseOffset;

					ns = lerp(0, _OutlineNoiseThreshold, ns);
					ns *= pow(i.uv0.y, _OutlineNoiseVerticalAjust);

					float noise = 1 - smoothstep(0.0, _OutlineNoiseDistance, i.uv0.x) * (1 - smoothstep(1 - _OutlineNoiseDistance, 1.0, i.uv0.x)) * smoothstep(0.0, _OutlineNoiseDistance, i.uv0.y) * (1 - smoothstep(1 - _OutlineNoiseDistance, 1.0, i.uv0.y));
					
					col *= lerp(1, ns, noise);
				}
				else 
				{
					discard;
				}

				col.a *= _CardOpacity;

				return col;
			}

			ENDCG
		}

		Pass
		{
		    Stencil {
                Ref [_Stencil]
                Comp Always
				Pass Replace
            }

			Name "Side"

			Cull Back
			Lighting Off
			Fog { Mode Off }
			ZWrite Off
			ZTest Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM

			#pragma target 3.0
			#pragma vertex vert 
			#pragma fragment frag

			#include "HyperCard_NoiseLib.cginc"
			#include "HyperCard_Utils.cginc"
			#include "UnityCG.cginc"

			uniform float4 _TimeEditor;

			// general
			uniform int _Seed;
			uniform float _CardOpacity;

			// frame
			uniform sampler2D _FrameMap;
			uniform sampler2D _FrameDiffuse;
			uniform float4 _FrameDiffuseColor;

			uniform sampler2D _Artwork;
			uniform float4 _Artwork_ST;

			// dissolve
			uniform int _IsDissolveEnabled;
			uniform float _BurnNoiseFrequency;
			uniform float _BurningAmount;
			uniform float _BurningRotateSpeed;
			uniform float _BurningOutline;
			uniform float4 _BurnStartColor;
			uniform float4 _BurnEndColor;
			uniform float _BurnAlphaCut;
			uniform float _BurnExposure;

			// distortion
			uniform int _IsDistortionEnabled;
			uniform sampler2D _DistortionMap;

			uniform float _DistortionRedFrequency;
			uniform float _DistortionRedAmplitude;
			uniform float _DistortionRedSpeed;
			uniform float4 _DistortionRedDirection;

			uniform float _DistortionGreenFrequency;
			uniform float _DistortionGreenAmplitude;
			uniform float _DistortionGreenSpeed;
			uniform float4 _DistortionGreenDirection;

			uniform float _DistortionBlueFrequency;
			uniform float _DistortionBlueAmplitude;
			uniform float _DistortionBlueSpeed;
			uniform float4 _DistortionBlueDirection;

			// holo/cubemap
			uniform int _IsHoloFxEnabled;
			uniform int _HoloUseArtworkCoords;
			uniform sampler2D _HoloMask;
			uniform sampler2D _HoloMap;
			uniform float2 _HoloMapScale;
			uniform samplerCUBE _HoloCube;
			uniform float4 _HoloCubeColor;
			uniform float _HoloCubeContrast;
			uniform float _HoloCubeRotation;
			uniform float _HoloPower;
			uniform float _HoloAlpha;
			uniform float3 _HoloBBoxMin;
			uniform float3 _HoloBBoxMax;
			uniform float3 _HoloEnviCubeMapPos;
			uniform int _HoloDebug;

			// glitter fx
			uniform int _IsGlitterFxEnabled;
			uniform int _GlitterUseArtworkCoords;
			uniform float4 _GlitterColor;
			uniform float _GlitterPower;
			uniform float _GlitterContrast;
			uniform float _GlitterSpeed;
			uniform float _GlitterSize;
			uniform sampler2D _GlitterMask;
			uniform float4 _GlitterMask_ST;
			uniform sampler2D _GlitterBackTex;
			uniform float4 _GlitterBackTex_ST;
			uniform float _GlitterBackPower;
			uniform float _GlitterBackContrast;
			uniform float _GlitterLight;
			uniform float4 _GlitterLightColor;
			uniform float _GlitterLightRadius;
			uniform float _GlitterOpacity;

			// color
			uniform float4 _SideColor;

			// filters
			uniform int _BlackAndWhite;

			struct appdata_t
			{
				float4 vertex			: POSITION;
				float2 uv0				: TEXCOORD0;
				float3 normal			: NORMAL;
				float4 tangent			: TANGENT;
			};

			struct v2f
			{
				float4 vertex			: SV_POSITION;
				float2 uv0				: TEXCOORD0;
				float3 vertexInWorld	: TEXCOORD1;
				float3 viewDirInWorld	: TEXCOORD2;
				float3 normalInWorld	: TEXCOORD3;
				float3 tangentDir		: TEXCOORD4;
				float3 bitangentDir		: TEXCOORD5;

			};

			v2f vert(appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv0 = v.uv0;
	
				float4 vertexWorld = mul(unity_ObjectToWorld, v.vertex);
				float4 normalWorld = mul(float4(v.normal, 0.0), unity_WorldToObject);

				o.vertexInWorld = vertexWorld.xyz;
				o.viewDirInWorld = vertexWorld.xyz - _WorldSpaceCameraPos;
				o.normalInWorld = normalWorld.xyz;
				o.tangentDir = normalize(mul(unity_ObjectToWorld, float4(v.tangent.xyz, 0.0)).xyz);
				o.bitangentDir = normalize(cross(o.normalInWorld, o.tangentDir) * v.tangent.w);
				UnityObjectToWorldNormal(v.normal);

				return o;
			}

			float3 LocalCorrect(float3 origVec, float3 bboxMin, float3 bboxMax, float3 vertexPos, float3 cubemapPos)
			{
				float3 invOrigVec = float3(1.0,1.0,1.0)/origVec;
				float3 intersecAtMaxPlane = (bboxMax - vertexPos) * invOrigVec;
				float3 intersecAtMinPlane = (bboxMin - vertexPos) * invOrigVec;
				float3 largestIntersec = max(intersecAtMaxPlane, intersecAtMinPlane);
				float Distance = min(min(largestIntersec.x, largestIntersec.y), largestIntersec.z);
				float3 IntersectPositionWS = vertexPos + origVec * Distance;
				float3 localCorrectedVec = IntersectPositionWS - cubemapPos;

				return localCorrectedVec;
			}

			float4 RotateAroundYInDegrees (float4 vertex, float degrees)
			{
				float alpha = degrees * UNITY_PI / 180.0;
				float sina, cosa;
				sincos(alpha, sina, cosa);
				float2x2 m = float2x2(cosa, -sina, sina, cosa);
				return float4(mul(m, vertex.xz), vertex.yw).xzyw;
			}

			float4 frag(v2f i) : SV_TARGET
			{
				float4 time = _Time + _TimeEditor;

				// frame
				float4 frameMap = tex2D(_FrameMap, i.uv0);
				float4 frameDiffuse = tex2D(_FrameDiffuse, i.uv0);

				if (frameMap.a < 0.01) discard;

				frameDiffuse *= _FrameDiffuseColor;

				// splat
				float2 artworkUV = TRANSFORM_TEX(i.uv0, _Artwork);

				float4 artwork = float4(0,0,0,0);

				// distortion
				if (_IsDistortionEnabled)
				{
					float4 distortionMap = tex2D(_DistortionMap, artworkUV);

					// red
					float2 distortionRed = float2(artworkUV.x * _DistortionRedFrequency * _DistortionRedDirection.x, artworkUV.y * _DistortionRedFrequency * _DistortionRedDirection.y);
					float distortionRedCoord = sin(distortionRed.x + distortionRed.y + _DistortionRedSpeed * time.g) * distortionMap.r * _DistortionRedAmplitude / 10;

					// green
					float2 distortionGreen = float2(artworkUV.x * _DistortionGreenFrequency * _DistortionGreenDirection.x, artworkUV.y * _DistortionGreenFrequency * _DistortionGreenDirection.y);
					float distortionGreenCoord = sin(distortionGreen.x + distortionGreen.y + _DistortionGreenSpeed * time.g) * distortionMap.g * _DistortionGreenAmplitude / 10;

					// blue
					float2 distortionBlue = float2(artworkUV.x * _DistortionBlueFrequency * _DistortionBlueDirection.x, artworkUV.y * _DistortionBlueFrequency * _DistortionBlueDirection.y);
					float distortionBlueCoord = sin(distortionBlue.x + distortionBlue.y + _DistortionBlueSpeed * time.g) * distortionMap.b * _DistortionBlueAmplitude / 10;

					artwork = tex2D(_Artwork, float2(artworkUV.x + distortionRedCoord + distortionGreenCoord + distortionBlueCoord, artworkUV.y + distortionRedCoord + distortionGreenCoord + distortionBlueCoord));
				}
				else 
				{
					artwork = tex2D(_Artwork, artworkUV);
				}

				// holo/cubemap
				if (_IsHoloFxEnabled)
				{
					float2 uv = _HoloUseArtworkCoords ? artworkUV : i.uv0;

					float3 viewDirWS = normalize(i.viewDirInWorld);
					float3 normalWS = normalize(i.normalInWorld);
					float3 reflDirWS = reflect(viewDirWS, normalWS);

					float3 localCorrReflDirWS = LocalCorrect(reflDirWS, _HoloBBoxMin, _HoloBBoxMax, i.vertexInWorld, _HoloEnviCubeMapPos);
					float4 holoTexture = tex2D(_HoloMap, uv * _HoloMapScale);

					holoTexture *= _HoloPower;

					float2 holo_o = holoTexture.rg + float4(localCorrReflDirWS, 0).rg;
					float3 holo_uv = localCorrReflDirWS + float3(holo_o.r, holo_o.g, 0);
					float4 holo_uvrot = RotateAroundYInDegrees(float4(holo_uv, 0), _HoloCubeRotation);
					float4 reflColor = texCUBE(_HoloCube, holo_uvrot.xyz);

					if (_HoloCubeColor.r < 1 || _HoloCubeColor.g < 1 || _HoloCubeColor.b < 1)
					{
						reflColor.rgb = (reflColor.r + reflColor.g + reflColor.b) / 3;
						reflColor *= _HoloCubeColor;
					}

					if (_HoloDebug)
					{
						if (_HoloUseArtworkCoords)
						{
							artwork = reflColor;
						}
						else
						{
							frameDiffuse = reflColor;
						}					
					}
					else
					{
						float4 holoMask = tex2D(_HoloMask, uv);

						if (_HoloUseArtworkCoords)
						{						
							float3 holoCol = lerp(artwork.rgb, AdjustContrast(reflColor.rgb, _HoloCubeContrast), _HoloAlpha);
							artwork.rgb = lerp(artwork.rgb, holoCol, holoMask.r);
						}
						else
						{
							frameDiffuse = lerp(frameDiffuse, artwork, frameMap.g);
							float3 holoCol = lerp(frameDiffuse.rgb, AdjustContrast(reflColor.rgb, _HoloCubeContrast), _HoloAlpha);
							frameDiffuse.rgb = lerp(frameDiffuse.rgb, holoCol, holoMask.r);
						}
					}
				}

				// glitter fx
				if (_IsGlitterFxEnabled)
				{
					float2 uv = _GlitterUseArtworkCoords ? artworkUV : i.uv0;

					float3 normalDirection = normalize(i.normalInWorld);
					float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.vertexInWorld.xyz);
					float3x3 tangentTransform = float3x3(i.tangentDir, i.bitangentDir, normalDirection);
					float NdotE = max(0, dot(normalDirection, viewDirection));

					float2 factor = 0.08 * mul(viewDirection, tangentTransform).xy * 1 / _GlitterSize;
					factor.x *= _GlitterSpeed;

					float distanceAlpha = 1; // saturate(1 - (distance(i.vertexInWorld.xyz, _WorldSpaceCameraPos.xyz) / 10));
					float scale = 1 / (_GlitterSize) * 5;

					float2 glints1_uv = scale * (factor + uv);
					float4 glints1_color = float4(PolkaDot2D(glints1_uv, 0, 1), PolkaDot2D(glints1_uv, 0, 1), PolkaDot2D(glints1_uv, 0, 1), 1);

					glints1_uv = _GlitterSpeed * mul(glints1_uv - float2(0.5, 0.5), float2x2(0.5, -0.5, 0.5, 0.5)) + float2(0.5, 0.5) + 1;
					float4 glintsx_color = float4(PolkaDot2D(-glints1_uv, 0, 0.4), PolkaDot2D(-glints1_uv, 0, 0.4), PolkaDot2D(-glints1_uv, 0, 0.4), 1);
					glints1_color += glintsx_color;
					glints1_color *= _GlitterPower * _GlitterColor * 10 * distanceAlpha;

					float2 glints2_uv = scale * (mul((-1 * factor + uv).xy - float2(0.5, 0.5), float2x2(-1, 0, 0, -1)) + float2(0.5, 0.5));
					float4 glints2_color = float4(PolkaDot2D(glints2_uv, 0, 1), PolkaDot2D(glints2_uv, 0, 1), PolkaDot2D(glints2_uv, 0, 1), 1);

					float4 glitter_mask_color = tex2D(_GlitterMask, TRANSFORM_TEX(uv, _GlitterMask));
					float3 final_glints = lerp(0, glints1_color.rgb, min(glints2_color.rgb, glitter_mask_color.rgb));
					final_glints = AdjustContrast(final_glints, _GlitterContrast);

					float4 back_color = tex2D(_GlitterBackTex, TRANSFORM_TEX(uv, _GlitterBackTex));
					back_color.rgb = AdjustContrast(back_color.rgb, _GlitterBackContrast) * _GlitterBackContrast;
					back_color.rgb = lerp(0, back_color, glitter_mask_color.rgb) * _GlitterBackPower;

					float3 specular = pow(max(0, dot(viewDirection, normalDirection)), 10 / _GlitterLightRadius);
					specular *= back_color.rgb * _GlitterLightColor.rgb * _GlitterLight;
					specular = lerp(0, specular, glitter_mask_color.rgb);

					float3 glitter_color = lerp((final_glints + back_color), (final_glints + back_color) * 10, specular) * (NdotE - 0.25);
					glitter_color += specular;

					if (_GlitterUseArtworkCoords)
					{
						artwork.rgb = lerp(artwork.rgb, lerp(artwork.rgb, glitter_color, _GlitterOpacity), glitter_mask_color.r);
					}
					else
					{
						frameDiffuse.rgb = lerp(frameDiffuse.rgb, lerp(frameDiffuse.rgb, glitter_color, _GlitterOpacity), glitter_mask_color.r);
					}
				}

				frameDiffuse = lerp(frameDiffuse, artwork, frameMap.g);

				// dissolve
				if (_IsDissolveEnabled)
				{
					float cosburn = cos(0.5 * time.g * _BurningRotateSpeed);
					float sinburn = sin(0.5 * time.g * _BurningRotateSpeed);
					float2 pivotburn = float2(0.5, 0.5);
					float2 burn_uv = (mul(i.uv0 - pivotburn, float2x2(cosburn, -sinburn, sinburn, cosburn)) + pivotburn);

					float burnMap = snoise(burn_uv * _BurnNoiseFrequency + _Seed) * 0.5 + 0.5;
					float smo = saturate((_BurningAmount * (1 + _BurningOutline) - burnMap) / _BurningOutline);
					float smb = smo - smoothstep(0.0, smo, i.uv0.x) * (1 - smoothstep(1 - smo, 1.0, i.uv0.x)) * smoothstep(0.0, smo, i.uv0.y) * (1 - smoothstep(1 - smo, 1.0, i.uv0.y));
					float burnBorder = lerp(0, 1, 1 - smb);
					float alphaBurn = min(frameMap.a, burnBorder);

					frameDiffuse = float4(lerp(frameDiffuse.rgb, frameDiffuse.rgb * (lerp(_BurnStartColor, _BurnEndColor, smo) * _BurnExposure * burnBorder), smo), alphaBurn);

					if (alphaBurn < _BurnAlphaCut)
					{
						clip(-1);
					}
				}

				frameDiffuse *= _SideColor;

				// filters
				if (_BlackAndWhite) 
				{
					half c = (frameDiffuse.r + frameDiffuse.g + frameDiffuse.b) / 3;
					frameDiffuse = fixed4(c, c, c, frameDiffuse.a);
				};

				return frameDiffuse * _CardOpacity;
			}
			
			ENDCG
		}


		Pass
		{
			Stencil {
				Ref [_Stencil]
				Comp Equal
				Pass Keep
			}

			Name "SpriteSheet"
			ZWrite Off
			ZTest Off
			Cull Back
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert 
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform float _CardOpacity;
			uniform sampler2D _FrameMap;

			uniform int _IsSpriteSheetEnabled;
			uniform sampler2D _SpriteSheetTexture;
			uniform float4 _SpriteSheetTexture_ST;
			uniform float2 _SpriteSheetOffset;
			uniform float2 _SpriteSheetScale;

			uniform float _SpriteSheetIndex;
			uniform float _SpriteSheetCols;
			uniform float _SpriteSheetRows;
			uniform float4 _SpriteSheetColor;
			uniform int _SpriteSheetRemoveBlack;

			uniform int _BlackAndWhite;

			struct appdata_t
			{
				float4 vertex	: POSITION;
				float2 uv1		: TEXCOORD1;
			};

			struct v2f
			{
				float4 vertex	: SV_POSITION;
				float2 uv1		: TEXCOORD1;
			};

			v2f vert(appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv1 = (v.uv1 - _SpriteSheetOffset) * _SpriteSheetScale;

				return o;
			}

			float4 frag(v2f i) : SV_TARGET
			{
				if (_IsSpriteSheetEnabled == 1)
				{
					float ss_row = floor((_SpriteSheetIndex / _SpriteSheetCols));
					float2 ss_v = float2((_SpriteSheetIndex - (_SpriteSheetCols * ss_row)), ss_row).gr;
					float3 ss_t = ((float3(ss_v.g, (abs((1.0 - _SpriteSheetRows)).rr - ss_v)) + float3(i.uv1, 0.0)) * float3((float2(1, 1) / float2(_SpriteSheetCols, _SpriteSheetRows)), 0.0));

					float4 ss_col = tex2D(_SpriteSheetTexture, TRANSFORM_TEX(ss_t, _SpriteSheetTexture));

					if (_SpriteSheetRemoveBlack == 1)
					{
						ss_col = float4(ss_col.rgb, (ss_col.r + ss_col.g + ss_col.b) / 3.0);
					}

					if (((i.uv1.r / _SpriteSheetScale.r) > (1 / _SpriteSheetScale.r)) || ((i.uv1.r / _SpriteSheetScale.r) < 0))
					{
						discard;
					}

					if (((i.uv1.g / _SpriteSheetScale.g) > (1 / _SpriteSheetScale.g)) || ((i.uv1.g / _SpriteSheetScale.g) < 0))
					{
						discard;
					}

					if (_BlackAndWhite == 1) 
					{
						ss_col = (ss_col.r + ss_col.g + ss_col.b) / 3;
					}

					ss_col *= _SpriteSheetColor;

					float2 cardMap = tex2D(_FrameMap, (i.uv1 / _SpriteSheetScale) + _SpriteSheetOffset);

					ss_col.a *= cardMap.g;
					ss_col.a *= _CardOpacity;

					if (ss_col.a < 0.1) discard;

					return ss_col;
				}

				return 0;
			}

			ENDCG
		}


		Pass 
		{
			ColorMask 0

			Stencil	{
				Ref	[_Stencil]
				Comp Equal
				Pass IncrSat
			}

			Cull Back
			ZWrite Off
			ZTest Off
			Offset -1, -1

			CGPROGRAM
			#pragma target 3.0
			#pragma vertex vert 
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform sampler2D _FrameMap;

			struct appdata_t
			{
				float4 vertex			: POSITION;
				float2 uv0				: TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex	: SV_POSITION;
				float2 uv0		: TEXCOORD0;
			};

			v2f vert(appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv0 = v.uv0;

				return o;
			}

			float4 frag(v2f i) : SV_TARGET
			{

				float4 frameMap = tex2D(_FrameMap, i.uv0);

				if (frameMap.g < 0.1) discard;

				return float4(1, 1, 0, 1);


				return frameMap;
			}

			ENDCG
		}
	}
}