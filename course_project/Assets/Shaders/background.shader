Shader "Hidden/Panorama"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Transparency ("Transparency", Range(0, 1)) = 1.0
    }
    SubShader
    {
        Cull Front

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                // float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                // float2 uv : TEXCOORD0;

                float3 direction : TEXCOORD0; // Pass a view direction instead of a UV coordinate.
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                // o.uv = v.uv;

                // Compute worldspace direction from the camera to this vertex.
                o.direction = mul(unity_ObjectToWorld, v.vertex).xyz - _WorldSpaceCameraPos;
                return o;
            }

            sampler2D _MainTex;
            float _Transparency;
            fixed4 _Color;

            fixed4 frag (v2f i) : SV_Target
            {
                // Convert direction into lat/long coordinates
                float3 dir = normalize(i.direction);
                float2 uv = float2(0.5f * atan2(dir.z, dir.x), asin(dir.y));
                uv = uv / UNITY_PI + 0.5f;
                
                fixed4 col = tex2D(_MainTex, uv) * _Color;
                col.a = _Transparency;

                return col;
            }
            ENDCG
        }
    }
}