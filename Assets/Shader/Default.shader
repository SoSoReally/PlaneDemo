Shader "Plane/differe"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color("Selfluminous",Color)=(0,0,0,1)
		_SliderColor("SliderColor",Color) = (0,0,0,1)
		_Alpha("Alpha",Range(0,1))=1
	}
	SubShader
	{
		Tags { "RenderType"="Transparent"}
		LOD 100

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv  : TEXCOORD0;
				float4 normal :NORMAL;
			
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				
				float3 wordpos :TEXCOORD1;
				float4 objectpos:TEXCOORD2;
				float4 normal :TEXCOORD3;
				float4 color :TEXCOORD4;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Color;
			float4 _SliderColor;
			half _Alpha;
			v2f vert (appdata v)
			{
				v2f o;
				half3 normal = UnityObjectToWorldNormal(v.normal);
				float4 posword=mul(unity_ObjectToWorld,v.vertex);
				o.wordpos = posword.xyz;
				o.normal.xyz = normal.xyz;


				half3 eyeVec = normalize(posword.xyz - _WorldSpaceCameraPos);
				o.normal.w = pow((1 - saturate(dot(normal.xyz, -eyeVec))),4.0);
				o.objectpos = v.vertex;


				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);


				float d = max(0, dot(o.normal,-eyeVec));
				_SliderColor *= (1-d);
				_Color *= d;
				o.color = _SliderColor+_Color;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				
				col *=i.color;
				col.a = _Alpha;

				return col;
			}
			ENDCG
		}
	}
}
