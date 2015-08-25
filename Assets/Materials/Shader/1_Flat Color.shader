Shader "shaderTest/customShaders/1 - Flat Color" {	
	Properties 
	{
		_Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
	}

	SubShader {
		Pass {
			CGPROGRAM

			//pragmas
			#pragma vertex vert
			#pragma fragment frag
			
			//user defined variables
			uniform float4 _Color;
			
			//base input structs
			struct vertexInput
			{
				float4 vertex : POSITION;
			};
			
			struct vertexOutput
			{
				float4 pos : SV_POSITION;
			};
			
			//vertex functions
			vertexOutput vert(vertexInput vIn)
			{
				vertexOutput vO;		
				vO.pos = mul(UNITY_MATRIX_MVP, vIn.vertex);
				
				return vO;
			}
			
			//fragment function
			float4 frag(vertexOutput i) : COLOR
			{
				return _Color;
			}

			ENDCG
		}
	}
	//Fallback commented out during developement; Fallback Shaders will be used when Shader is unaccesable
	//Fallback "Diffuse"
}