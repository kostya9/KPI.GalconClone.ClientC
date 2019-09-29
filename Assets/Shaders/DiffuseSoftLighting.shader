Shader "Unlit/NewUnlitShader"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_LightPower("Light Power", Range(0, 2)) = 1
	}
		SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag
			// make fog work
	#pragma multi_compile_fog

	#include "UnityCG.cginc"
			fixed4 _Color;
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
					float4 vertex : SV_POSITION;
				fixed3 worldNorm : TEXCOORD2;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _LightPower;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldNorm = mul((float3x3)unity_ObjectToWorld, v.normal);
				o.worldNorm = normalize(o.worldNorm);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = _Color * _LightPower;

				fixed diff = max(0, dot(i.worldNorm, _WorldSpaceLightPos0.xyz));
				col.rgb *= diff;

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
		ENDCG
		
		}
		
	}
		FallBack "Diffuse"
}