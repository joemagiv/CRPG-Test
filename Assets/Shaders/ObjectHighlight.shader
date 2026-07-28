Shader "Custom/ObjectHighlight"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _HighlightColor ("Highlight Color", Color) = (1,0.8,0,1)
        _HighlightIntensity ("Highlight Intensity", Range(0, 5)) = 2.0
        _OutlineWidth ("Outline Width", Range(0.001, 0.1)) = 0.02
        _OutlineColor ("Outline Color", Color) = (0,0.8,1,1)
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        
        // Main pass
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };
            
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldNormal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
            };
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float4 _HighlightColor;
            float _HighlightIntensity;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the texture
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                
                // Apply highlight based on normal
                float highlight = dot(normalize(float3(0, 0, 1)), normalize(i.worldNormal));
                highlight = pow(saturate(highlight), 4.0) * _HighlightIntensity;
                
                // Mix base color with highlight color
                col.rgb = lerp(col.rgb, _HighlightColor.rgb, highlight);
                
                return col;
            }
            ENDCG
        }
        
        // Outline pass
        Pass
        {
            Name "Outline"
            Cull Front
            ZWrite Off
            ZTest Always
            Blend SrcAlpha OneMinusSrcAlpha
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 pos : TEXCOORD0;
            };
            
            float _OutlineWidth;
            float4 _OutlineColor;
            
            v2f vert (appdata v)
            {
                v2f o;
                // Expand vertices along normals
                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                float3 normal = UnityObjectToWorldNormal(v.normal);
                worldPos.xyz += normal * _OutlineWidth;
                o.vertex = UnityWorldToClipPos(worldPos);
                o.pos = worldPos;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDCG
        }
    }
    
    FallBack "Diffuse"
}