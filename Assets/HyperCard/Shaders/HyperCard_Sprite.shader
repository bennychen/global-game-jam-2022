Shader "HyperCard/Sprite"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		[HideInInspector] _Color ("Tint", Color) = (1,1,1,1)
		[HideInInspector] _Exposure("_Exposure", Float) = 0
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        _StencilComp("Stencil Comparison", Float) = 8
        _Stencil("Stencil ID", Float) = 0
        _StencilOp("Stencil Operation", Float) = 0
        _StencilWriteMask("Stencil Write Mask", Float) = 255
        _StencilReadMask("Stencil Read Mask", Float) = 255
		[HideInInspector] _Zoom("_Zoom", Float) = 1
		[HideInInspector] _DistortionMask("_DistortionMask", 2D) = "white" {}
		[HideInInspector] _SpriteSheetOffset("_MaskOffset", Vector) = (1,1,0,0)
		[HideInInspector] _SpriteSheetScale("_MaskScale", Vector) = (1,1,0,0)
		[HideInInspector] _DistortionFreq("_DistortionFreq", Float) = 0
		[HideInInspector] _DistortionAmp("_DistortionAmp", Float) = 0
		[HideInInspector] _DistortionSpeed("_DistortionSpeed", Float) = 0
		[HideInInspector] _DistortionDir("_DistortionDir", Vector) = (0,0,0,0)
		[HideInInspector] _MoveDir("_MoveDir", Vector) = (0,0,0,0)
		[HideInInspector] _BlackAndWhite("_BlackAndWhite", Int) = 0
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
		}

		Stencil
		{
			Ref[_Stencil]
			Comp Equal
			Pass Keep
			ReadMask [_StencilReadMask]
		}

		Cull Back
		Lighting Off
		ZWrite Off
		Fog { Mode Off }
		Offset -1, -1

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile DUMMY PIXELSNAP_ON
			#include "UnityCG.cginc"

			uniform float4 _TimeEditor;

			uniform fixed4 _Color;
			uniform sampler2D _MainTex;
			uniform float _Zoom;
			uniform float _Exposure;
			uniform sampler2D _DistortionMask;
			uniform float4 _DistortionMask_ST;
			uniform float _DistortionFreq;
			uniform float _DistortionAmp;
			uniform float _DistortionSpeed;
			uniform float2 _DistortionDir;
			uniform float2 _MoveDir;

			uniform int _BlackAndWhite;

			uniform sampler2D _FrameMap;
			uniform int _RestrictToArtwork;

			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 uv0		: TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 uv0		: TEXCOORD0;
			};


			v2f vert(appdata_t i)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(i.vertex);
				OUT.uv0 = i.uv0;
				OUT.color = i.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float4 time = _Time + _TimeEditor;
				float xs = i.uv0.x * _DistortionFreq * _DistortionDir.x;
				float ys = i.uv0.y * _DistortionFreq * _DistortionDir.y;
				float uv = sin(xs + ys + _DistortionSpeed * time.g) * _DistortionAmp / 10;
				float4 c = tex2D(_MainTex, float2(i.uv0.x * _Zoom + uv + _MoveDir.x * time.g, i.uv0.y * _Zoom + uv + _MoveDir.y * time.g)) * i.color * _Exposure;
				float4 maskTex = tex2D(_DistortionMask, TRANSFORM_TEX(i.uv0, _DistortionMask));

				c.a = c.a * i.color.a * maskTex.r;

				if (_BlackAndWhite)
				{
					half c2 = (c.r + c.g + c.b) / 3;
					c = fixed4(c2, c2, c2, c.a);
				};

				return c;
			}

			ENDCG
		}
    }
}