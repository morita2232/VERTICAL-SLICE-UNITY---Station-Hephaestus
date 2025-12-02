Shader "Unlit/ConduitShader"
{
    Properties
    {
        _BaseColor   ("Base Color", Color) = (0.05, 0.05, 0.05, 1)
        _FillColor   ("Fill Color", Color) = (0, 1, 1, 1)   // blue/green
        _TargetColor ("Target Color", Color) = (1, 0, 0, 1) // red zone

        _Fill      ("Fill Amount", Range(0,1)) = 0.0        // how high we've charged
        _TargetMin ("Target Min", Range(0,1)) = 0.4
        _TargetMax ("Target Max", Range(0,1)) = 0.6
        _GlowStrength ("Glow Strength", Range(0,10)) = 4
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
            };

            fixed4 _BaseColor;
            fixed4 _FillColor;
            fixed4 _TargetColor;
            float  _Fill;
            float  _TargetMin;
            float  _TargetMax;
            float  _GlowStrength;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv  = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float along = i.uv.y; // 0 at one end, 1 at the other

                // RED ZONE: between TargetMin and TargetMax
                float targetMask = step(_TargetMin, along) * step(along, _TargetMax);

                // FILL: from 0 up to _Fill
                float fillMask = step(0.0, along) * step(along, _Fill);

                // Base
                fixed4 col = _BaseColor;

                // Add fill color
                col.rgb += _FillColor.rgb * fillMask * _GlowStrength;

                // Add red target color on top (so you always see the red zone)
                col.rgb = lerp(col.rgb, _TargetColor.rgb, targetMask * 0.7);

                return col;
            }
            ENDCG
        }
    }
}
