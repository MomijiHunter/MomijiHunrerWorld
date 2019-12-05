//徐々に透明になるシェーダー
Shader "myShader/GradientAlpha"
{
    Properties
    {
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		_PosX("PoxX",Range(0,1)) = 1//ここより大きい範囲は透明になる
		_Width("Width",Range(0,1)) = 1//半透明になる範囲
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
    }
    SubShader
    { Tags
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
			fixed _PosX;
			fixed _Width;

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

#if ETC1_EXTERNAL_ALPHA
				// get the color from an external texture (usecase: Alpha support for ETC1 on android)
				//color.a = tex2D(_AlphaTex, uv).r;
#endif //ETC1_EXTERNAL_ALPHA

				return color;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;
				clip(c.a - 0.5);//半透明をカット

				//補正後のPOS_X
				//0の時-width、１の時1になるように変換
				float pos_use = (_PosX*((1 + _Width) / 1))-_Width; 
				float param = _ScreenParams.x;
				float edge0 = (pos_use + _Width) * param;//透明終了点
				float edge1 = (pos_use) * param;//透明開始点

				float now = IN.texcoord.x * _ScreenParams.x;//現在位置

				if (now < edge1) {//通常描画範囲

				}
				else if (edge1 < now&&now < edge0) {//透明補完範囲
					c.a = (1 - smoothstep(0, 1, (now - edge1) / (edge0 - edge1)))*IN.color.a;
				}
				else if (edge0 < now) {//透明範囲
					c.a = 0;
				}
				c.rgb *= c.a;

				return c;
			}
            ENDCG
        }
    }
}
