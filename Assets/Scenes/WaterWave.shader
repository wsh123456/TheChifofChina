// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'
 
Shader "Custom/WaterWave" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _WaveStrength("Wave Strength",Float) = 0.01
        _WaveFactor("Wave Factor",Float) = 50
        _TimeScale("Time Scale",Float) = 10
    }
    SubShader {
        
        Pass
        {
            CGPROGRAM
 
            #pragma vertex vert 
            #pragma fragment frag
 
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
 
            };
 
            struct v2f {
                float4 vertex:SV_POSITION;
                float2 uv:TEXCOORD0;
            };
 
            sampler2D _MainTex;
            float _WaveStrength;
            float _WaveFactor;
            float _TimeScale;
 
            v2f vert (appdata v) 
            {
                v2f o;
                o.uv = v.uv;
                o.vertex = UnityObjectToClipPos(v.vertex);
 
                return o;
            }
 
            fixed4 frag(v2f IN):COLOR
            {                
                fixed2 uvDir = normalize(IN.uv - fixed2(0.5, 0.5));
                fixed dis = distance(IN.uv, fixed2(0.5, 0.5));
 
                fixed2 uv = IN.uv + _WaveStrength * uvDir * sin(_Time.y * _TimeScale + dis * _WaveFactor);
                return tex2D(_MainTex, uv);
            }
 
            ENDCG
        }
 
    } 
    FallBack "Diffuse"	FallBack "Diffuse"
}