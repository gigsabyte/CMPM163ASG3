Shader "Custom/ParticleShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_StartColor ("Start Color", Color) = (0, 0, 1, 1)
		_EndColor ("End Color", Color) = (0, 1, 0, 1)
        //Define properties for Start and End Color
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Opaque" }
        LOD 100
        
        Blend One One
        ZWrite off
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            struct appdata
            {
                float4 vertex : POSITION;
				float3 uv : TEXCOORD0;
                //Define UV data
            };

            struct v2f
            {   
                float4 vertex : SV_POSITION;
				float3 uv : TEXCOORD0;
                //Define UV data
            };

            sampler2D _MainTex;
			float4 _StartColor;
			float4 _EndColor;
          

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv; //Correct this for particle shader
             
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                //Get particle age percentage
                float ParticleAgePercentage = i.uv.z;

                //Sample color from particle texture
                float4 col = tex2D(_MainTex, i.uv.xy);

                //Find "start color"
				float4 start = _StartColor;
                
                //Find "end color"
				float4 end = _EndColor;
                
                //Do a linear interpolation of start color and end color based on particle age percentage
				float4 finalcol = lerp(start, end, ParticleAgePercentage) * (col.a);

                return finalcol;
            }
            ENDCG
        }
    }
}
