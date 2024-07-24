Shader "Custom/FogParticleShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Tint Color", Color) = (1,1,1,1)
        _Radius ("Radius", Float) = 1.0
        _PlayerPos ("Player Position", Vector) = (0,0,0,0)
    }
    SubShader
    {
        Tags {"Queue" = "Overlay"}
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            ZTest Always
            Cull Off
            Lighting Off
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
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            float _Radius;
            float4 _PlayerPos;
            sampler2D _MainTex;
            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Menghitung jarak dari kabut ke posisi pemain
                float distance = length(i.worldPos - _PlayerPos.xyz);
                // Mengatur alpha berdasarkan jarak
                float alpha = smoothstep(_Radius, _Radius * 0.5, distance);
                return tex2D(_MainTex, i.pos.xy) * _Color * alpha;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
