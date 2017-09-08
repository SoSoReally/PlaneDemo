Shader "Unlit/Capsule"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_BackColor("_BackColor",Color) = (0,0,0,1)
		_BackColorStrong("_BackColorStrong",float)=1.1
		_LineColor("LineColor",Color) = (0,0,0,1)
		_LineColorStrong("LineColorStrong",float) = 1.1
		_LineSize("LineSize",Range(0,1)) = 0.1
		_LineCount("LineCount",float) =1
		_Antialias("_Antialias",float)=0.5 
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;

				float4 objpos:TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4	_BackColor;
			fixed _BackColorStrong;
			fixed4 _LineColor;
			fixed _LineColorStrong;
			fixed _LineSize;
			fixed _LineCount;
			fixed _Antialias;
			const fixed2  stander = fixed2(0,1);
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.objpos = v.vertex;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);

			    fixed2 xz=	fixed2(i.objpos.x,i.objpos.z);

				fixed value = 1/_LineCount;

				fixed dotx = dot(xz,fixed2(0,1));

				dotx += 1;

				dotx = dotx/2;

				fixed x = min( dotx%value - value - _LineSize/2,0); 

				fixed y = step(dotx%value - _LineSize/2,0);

				col = lerp(_BackColor,_LineColor,dotx/value);

				return col;
			}
			ENDCG
		}
	}
}
