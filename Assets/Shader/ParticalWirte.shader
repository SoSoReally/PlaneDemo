Shader "Plane/ParticalWirte"
{
	Properties
	{
		_TintColor ("Color", Color) = (1,1,1,1)
		_LineWith("LineWith",Float)= 0
		_LineColor("LineColor",Color)=(0,0,0,0)
	}
	SubShader
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
		LOD 100

		Pass
		{	
		    Blend SrcAlpha One
			ColorMask RGB
			Cull Off Lighting Off ZWrite Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
	
	
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
	
				float2 uv : TEXCOORD0;

				fixed4 color :Color;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
		    float _LineWith;
			fixed4 _LineColor;
			fixed4 _TintColor;
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.color = v.color;
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
			
			    half withx =step(_LineWith,i.uv.x);
				half withy = step(_LineWith,i.uv.y);
				half withxx = step(i.uv.x,1-_LineWith);
				half withyy = step(i.uv.y,1-_LineWith);
				fixed4 final = lerp(_LineColor*1.2,_TintColor*i.color,withx*withxx*withy*withyy );
			
				return final;
			}
			ENDCG
		}
	}
}
