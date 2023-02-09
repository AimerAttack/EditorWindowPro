// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

 Shader "UIEDITOR/DefaultShader" 
 {
    Properties 
	{
        _MainTex ("Base (RGB)", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
    }

    Category 
	{
       Lighting Off
	   Fog { Mode Off }
	   Blend SrcAlpha OneMinusSrcAlpha
	   Cull Off
	   ZWrite Off

	    BindChannels {
			Bind "Color", color
			Bind "Vertex", vertex
			Bind "TexCoord", texcoord
		}

		SubShader {
			Pass {
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"
			
				sampler2D _MainTex;
				float4 _MainTex_ST;
				float4 _Color;

				// vertex input: position, color
				struct appdata {
					float4 color : COLOR0;
					float4 vertex : POSITION;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f {
					float4 vertex : POSITION;
					float4 color : COLOR;
					float2 texcoord : TEXCOORD0;
				};

				v2f vert (appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.color = v.color;
					o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
					return o;
				}
				// Fragment PRogram
				float4 frag (v2f IN) : COLOR
				{
					float4 color = tex2D (_MainTex, IN.texcoord) * _Color;
					color *= IN.color;
					return color;
				}
				ENDCG
			}
		}
    }
}
