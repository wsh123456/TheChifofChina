Shader "Hidden/WaveEffect"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	    _Amount("Amount",Range(0,1))=0.5
		_W("W",Range(0,200))=100
		 _Speed("Speed",Range(0,500)) = 200
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

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
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			float _Amount;
			float _W;
			float _Speed;

			fixed4 frag (v2f i) : SV_Target
			{
				//wave y= A sin(wt + theta)
				//amount=A/(spend*len)
				float2 center_uv = {0.8,0.1};
		     	float2 uv = i.uv;
				float2 dt = center_uv - uv;
				//点乘可以加快效率
				float len = sqrt(dot(dt, dt));
					//振幅
				float amount = _Amount / (_Amount + len * _Speed);
			    
				if (amount<0.001)
				{
					amount = 0;
				}
				uv.y += amount * cos(len* _W*UNITY_PI);
				fixed4 col = tex2D(_MainTex, i.uv);
				// just invert the colors
				//col.rgb = 1 - col.rgb;
				return col;
			}
			ENDCG
		}
	}
}
