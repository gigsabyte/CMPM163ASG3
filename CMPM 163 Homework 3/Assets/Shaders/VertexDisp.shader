Shader "Custom/VertexDisp"
{
    Properties
    {
		_Color ("Color", Color) = (1,1,1,1)
        _SpecularColor ("Specular Color", Color) = (1, 1, 1, 1)
        _MainTex ("Texture", 2D) = "white" {}
		_Shininess("Shininess", Float) = 1.0
		_AmbientColor("Ambient Color", Color) = (0.1, 0.1, 0.1, 1)
		_Cube ("Cubemap", CUBE) = "" {}
    }
    SubShader
    {
        Pass
        {
			Tags { "LightMode" = "ForwardAdd" "RenderType" = "Opaque"}
			//Blend SrcAlpha SrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

			float4 _Color;
			float4 _SpecularColor;
			float _Shininess;
			sampler2D _MainTex;
            float4 _MainTex_ST;
			float4 _LightColor0;
			float4 _AmbientColor;
            samplerCUBE _Cube; // skybox cube texture


            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				float3 normal: NORMAL; 
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
				float3 vertInWorldCoords: TEXCOORD1;
				float3 normal: NORMAL;
            };

            

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.vertex.x += (sin((_Time.y * 24/11) - length(_WorldSpaceCameraPos.yz - o.vertex.yz)) + 1)/2;

				o.vertInWorldCoords = mul(unity_ObjectToWorld, v.vertex);
				o.normal = UnityObjectToWorldNormal(v.normal);

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // position
             float3 P = i.vertInWorldCoords.xyz;
             
             //get normalized incident ray (from camera to vertex)
             float3 vIncident = normalize(P - _WorldSpaceCameraPos);
             
             //reflect that ray around the normal using built-in HLSL command
             float3 vReflect = reflect( vIncident, i.normal );
             
             
             //use the reflect ray to sample the skybox
             float4 reflectColor = texCUBE( _Cube, vReflect );
             
             //refract the incident ray through the surface using built-in HLSL command
             float3 vRefract = refract( vIncident, i.normal, 0.5 );
                          
             // refract RGB values by arbitrary but different amounts
             float3 vRefractRed = refract( vIncident, i.normal, 0.1 );
             float3 vRefractGreen = refract( vIncident, i.normal, 0.4 );
             float3 vRefractBlue = refract( vIncident, i.normal, 0.7 );
             
			 // sample the cube at the places where the refraction rays hit
             float4 refractColorRed = texCUBE( _Cube, float3( vRefractRed ) );
             float4 refractColorGreen = texCUBE( _Cube, float3( vRefractGreen ) );
             float4 refractColorBlue = texCUBE( _Cube, float3( vRefractBlue ) );

			 // get composite refraction color
             float4 refractColor = float4(refractColorRed.r, refractColorGreen.g, refractColorBlue.b, 1.0);
             
             
             return float4(lerp(reflectColor, refractColor, 0.2).rgb, 0.8);
            }
            ENDCG
        }
    }
}
