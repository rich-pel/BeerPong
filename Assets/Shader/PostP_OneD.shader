Shader "Richard/PostP_OneD_Blur"
{
   //show values to edit in inspector
	Properties{
		[HideInInspector] _MainTex ("Texture", 2D) = "white" {}
		_BlurSize("Blur Size", Range(0,0.5)) = 0
   //    [KeywordEnum(Vertical, Horizontal)] _Direction ("Direction", Float) = 0
        [Toggle(ASPECT)] _Aspect ("invAspect", float) = 0
	}

	SubShader{
		// markers that specify that we don't need culling 
		// or reading/writing to the depth buffer
		Cull Off
		ZWrite Off 
		ZTest Always

		// Vertical
		Pass{

			CGPROGRAM
			//include useful shader functions
			#include "UnityCG.cginc"

			//define vertex and fragment shader
			#pragma vertex vert
			#pragma fragment frag

            // #pragma multi_compile _DIRECTION_VERTICAL _DIRECTION_HORIZONTAL
            #pragma shader_feature ASPECT 
            
			// texture and transforms of the texture
			sampler2D _MainTex;
			float _BlurSize;

            // [10 100]
			#define SAMPLES 3
			
               
			//the object data that's put into the vertex shader
			struct appdata{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			//the data that's used to generate fragments and can be read by the fragment shader
			struct v2f{
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			//the vertex shader
			v2f vert(appdata v){
				v2f o;
				//convert the vertex positions from object space to clip space so they can be rendered
				o.position = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			//the fragment shader
			fixed4 frag(v2f i) : SV_TARGET
			{
               
                //calculate aspect ratio
				float invAspect = _ScreenParams.y / _ScreenParams.x;
			
                //init color variable
                float4 col = 0;
                float sum = SAMPLES;

				//iterate over blur samples
				for(float index = 0; index < SAMPLES; index++)
				{
				//get the offset of the sample
				#if ASPECT
                    float offset = (index/(SAMPLES-1) - 0.5) * _BlurSize;
					float2 uv = i.uv + float2(offset, 0);
				#else
					float offset = (index/(SAMPLES-1) - 0.5) * _BlurSize * invAspect;
					float2 uv = i.uv + float2(0, offset);
				#endif
					//get uv coordinate of sample
					//simply add the color if we don't have a gaussian blur (box)
					col += tex2D(_MainTex, uv);
				}
				//divide the sum of values by the amount of samples
				col = col / sum;
				return col;
			}

			ENDCG
		}
	}
}