Shader "Custom/BlinkingTransparentShader"
{
    Properties
    {
        _Color("Color", Color) = (1, 1, 1, 1)
        _EmissionColor("Emission Color", Color) = (1, 1, 1, 1)
        _Speed("Blink Speed", Float) = 1
    }
        SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            fixed4 _Color;
            fixed4 _EmissionColor;
            float _Speed;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float blink = abs(sin(_Time.y * _Speed));

            fixed4 finalColor = lerp(_Color, _EmissionColor, blink);

            finalColor.a = blink;

            return finalColor;
        }
        ENDCG
    }
    }
}
