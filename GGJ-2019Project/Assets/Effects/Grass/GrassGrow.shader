Shader "Unlit/GrassGrow"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		[PerRendererData]
		_growthOrigin ("Local Growth Origin", Vector) = (0,0,0,0)
		_baseGrowth ("Starting Growth", Float) = 1
		[PerRendererData]
		_growthRadius( "Growth Expansion", Float) = 0
		_detectionThreshold ("Detection Forward Threshold", Float) = .2
		_growthRamp ("Growth Ramp Scale", Float) = 3
		_startGrowth( "Start Height", Float) = .2
    }
    SubShader
    {
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}
		// No culling or depth
		Cull Off ZWrite Off ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha

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

            sampler2D _MainTex;
            float4 _MainTex_ST;

			float4 _growthOrigin;
			float _baseGrowth;
			float _detectionThreshold;
			float _growthRadius;
			float _growthRamp;
			float _startGrowth;

            v2f vert (appdata v)
            {	
                v2f o;
				//the magic
				float4 interVertex = v.vertex;

				//find dist between vert and radiating growth
				float dist = distance(_growthOrigin, v.vertex);
				float mag = clamp(_growthRadius - dist, 0, _growthRamp);
				float ramping = mag / _growthRamp;

				//interVertex.y = interVertex.y + v.vertex.z/ _detectionThreshold + _baseGrowth;
				interVertex.x = interVertex.x - (_startGrowth * (v.vertex.z / _detectionThreshold))  + _baseGrowth * ramping * (v.vertex.z / _detectionThreshold);


                //o.vertex = UnityObjectToClipPos(interVertex);
                o.vertex = UnityObjectToClipPos(interVertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}
