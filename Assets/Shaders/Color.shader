// Jeff Schoch

Shader "Custom/Color" 
{
	Properties{}

	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct VertexInput
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
			};

			struct VertexOutput
			{
				float4 vertex : SV_POSITION;
				float4 color : COLOR;
			};

			VertexOutput vert(VertexInput input)
			{
				VertexOutput output;
				output.color = input.color;
				output.vertex = UnityObjectToClipPos(input.vertex);
				return output;
			}

			fixed4 frag(VertexOutput input) : COLOR
			{
				return input.color;
			}
			ENDCG
		}
	}
	Fallback "Unlit/Color"
}