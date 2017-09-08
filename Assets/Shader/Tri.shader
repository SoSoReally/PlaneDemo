Shader "Plane/Tri"
{
	Properties
	{
		_Length("Length" ,float) = 1
		_Radiu("_Radiu" ,float) =0.7
		_BackColor("_BackColor",Color) = (0,0,0,1)
		_BackColorStrong("_BackColorStrong",float)=1.1
		_Color("_Color",Color) = (1,1,1,1)
		_ColorStrong("_ColorStrong",float)=1.1
		_with("_with",float)=0.1
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
				float4 vertex : SV_POSITION;
				float4 obj:TEXCOORD2;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Length;
			float _Radiu;
			float log22;
			fixed4 _Color;
			fixed4 _BackColor;
			fixed _ColorStrong;
			fixed _BackColorStrong;
			fixed _with;
			fixed _Antialias;
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				o.obj = v.vertex;

				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				log22 = 0.86602540378;
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);				

				_Radiu*=_Radiu;

				fixed ania = pow(_Length* log22- i.obj.x,2) + pow(_Length/2-i.obj.y,2) - _Radiu;
				
				fixed a = smoothstep( _Antialias,0, ania);

				fixed aniaa = _Radiu-_with-pow(_Length*log22- i.obj.x,2) - pow(_Length/2-i.obj.y,2);

				fixed aa = smoothstep(_Antialias,0,aniaa);

				a= aa*a;

				fixed anib=pow(-_Length*log22- i.obj.x,2) + pow(_Length/2-i.obj.y,2) - _Radiu;

				fixed b =	 smoothstep( _Antialias,0, anib);
				
				fixed anibb = _Radiu-_with -pow(-_Length*log22- i.obj.x,2) - pow(_Length/2-i.obj.y,2);

				fixed bb = smoothstep( _Antialias,0, anibb);

				b=bb*b;

				fixed anic = pow(0-i.obj.x,2) + pow(-_Length-i.obj.y,2) - _Radiu;

				fixed c =  smoothstep( _Antialias , 0, anic);

				fixed anicc = _Radiu-_with - pow(0-i.obj.x,2) - pow(-_Length-i.obj.y,2);

				fixed cc =smoothstep( _Antialias ,0, anicc);

				c= cc*c;

				fixed d = max(c, max(a,b));
				
				_BackColor *=_BackColorStrong;

				_Color*=_ColorStrong;

				col = lerp(_BackColor,_Color,d);

				return col;
			}
			ENDCG
		}
	}
}
