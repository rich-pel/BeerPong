Shader "Hidden/ImageEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Radius("Radius", float) = 0.5
		_Angle("Angle", float) = 0
    }
    SubShader
    {
        // No culling or depth
        Cull Off 
		ZWrite Off 
		ZTest Always

        Pass
        {
			CGPROGRAM
			//include useful shader functions
			#include "UnityCG.cginc"

			//define vertex and fragment shader
			#pragma vertex vert
			#pragma fragment frag

			// use Vales from above
			sampler2D _MainTex;
			float _Radius;
			float _Angle;

			//the object data that's put into the vertex shader
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
			
			//the data that's used to generate fragments and can be read by the fragment shader
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

			//the vertex shader
			v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

			//the fragment shader
			fixed4 frag (v2f i) : SV_Target
            {
				float 

                fixed4 col = tex2D(_MainTex, i.uv);
                // just invert the colors
                col.rgb = 1 - col.rgb;
                return col;
            }
            ENDCG
        }
    }
}
