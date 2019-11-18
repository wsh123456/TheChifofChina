Shader "mgo/water" 
{
	Properties
	{
		_MainTex("MainTex", 2D) = "White" {}
		_MainColor("MainColor", Color) = (1, 1, 1, 1)
		_F("length", range(1, 200)) = 50
		_A("amplitude", range(0, 0.2)) = 0.04 //幅度值
		_S("speed", range(0, 30)) = 2
		_Tiling("tiling", range(0, 1)) = 1
 
		_BumpTex("BumpTex", 2D) = "White" {}
		_BumpA("BumpAmplitude", range(0, 100)) = 1 //幅度值
 
		_UVSpeed("UVSpeed", range(0, 1)) = 0.2
 
	}
 
	SubShader
	{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
		pass
		{
			Cull Off
			Lighting Off
			ZWrite On
			Blend SrcAlpha OneMinusSrcAlpha
 
			CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag
	#include "UnityCG.cginc"
	#pragma multi_compile_fog
 
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform float _F;
			uniform float _A;
			uniform float _S;
			uniform float _Tiling;
			uniform half4 _MainColor;
 
			uniform sampler2D _BumpTex;
			uniform float _BumpA;
 
			uniform float _UVSpeed;
 
			struct v2f {
				float4 pos:POSITION;
				float2 uv:TEXCOORD0;
				UNITY_FOG_COORDS(2)
			};
 
			struct data
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};
 
			v2f vert(data v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o, o.pos);
				return o;
			}
			fixed4 frag(v2f IN) : COLOR
			{
				float2 uv = IN.uv;
				float scale = 0;
 
				scale += _A * sin(-uv.y * _Tiling * _F + _BumpA * (tex2D(_BumpTex, uv).r) + _Time.y * _S);
				uv = uv + uv * scale;
				uv.y = uv.y + -_Time.y * _UVSpeed;
				
				fixed4 color = tex2D(_MainTex, uv);
				color.rgb *= _MainColor.rgb;
				color.a = _MainColor.a;
				UNITY_APPLY_FOG(IN.fogCoord, color);
				
				return color;
			}
			ENDCG
		}
	}
		FallBack "Diffuse"
}