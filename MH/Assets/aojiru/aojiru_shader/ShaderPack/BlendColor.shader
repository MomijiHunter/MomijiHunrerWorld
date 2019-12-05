//画像に色を混ぜるシェーダ
//_BlendColorが白なら白くできる
Shader "myShader/BlendColor"//spriteRenderの標準シェーダー
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		_BlendColor("BlendColor", Color) = (1,1,1,1)//混ぜる色
		_BlendRate("BlendRate",Range(0,1))=0//０なら変化なし１なら真っ白
		[MaterialToggle] _BlendAlpha("Blend alph", Float) = 0
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
	}

		SubShader
		{
			Tags
			{
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
			}

			Cull Off
			Lighting Off
			ZWrite On
			Blend One OneMinusSrcAlpha

			Pass
			{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile _ PIXELSNAP_ON
				#include "UnityCG.cginc"

				struct appdata_t
				{
					float4 vertex   : POSITION;
					float4 color    : COLOR;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f
				{
					float4 vertex   : SV_POSITION;
					fixed4 color : COLOR;
					float2 texcoord  : TEXCOORD0;
				};

				fixed4 _Color;
				fixed4 _BlendColor;
				fixed _BlendRate;
				fixed _BlendAlpha;

				v2f vert(appdata_t IN)
				{
					v2f OUT;
					OUT.vertex = UnityObjectToClipPos(IN.vertex);
					OUT.texcoord = IN.texcoord;
					OUT.color = IN.color * _Color;
					#ifdef PIXELSNAP_ON
					OUT.vertex = UnityPixelSnap(OUT.vertex);
					#endif

					return OUT;
				}

				sampler2D _MainTex;
				sampler2D _AlphaTex;
				float _AlphaSplitEnabled;

				fixed4 SampleSpriteTexture(float2 uv)
				{
					fixed4 color = tex2D(_MainTex, uv);

	#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
					if (_AlphaSplitEnabled)
						color.a = tex2D(_AlphaTex, uv).r;
	#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

					return color;
				}

				fixed4 frag(v2f IN) : SV_Target
				{
					fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;
					fixed4 targetColor =_BlendColor;
					fixed3 white = fixed3(targetColor.r,targetColor.g,targetColor.b);
					c.rgb=white*_BlendRate+c.rgb*(1-_BlendRate);//白とブレンド
					if (_BlendAlpha == 1) {
						c.a = targetColor.a*_BlendRate +c.a*(1 - _BlendRate);
						c.rgb *= c.a;
					}
					else {
						c.rgb *= c.a;
					}

					return c;
				}
			ENDCG
			}
		}
}
