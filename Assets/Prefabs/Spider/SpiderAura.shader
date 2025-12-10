Shader "Custom/SpiderAura_Saiyan"
{
    Properties
    {
        _AuraColor   ("Aura Color", Color) = (1,1,0,1)
        _Intensity   ("Glow Intensity", Float) = 3
        _AuraStrength("Rim Strength", Float) = 2
        _NoiseScale  ("Noise Scale", Float) = 4
        _NoiseSpeed  ("Noise Speed", Float) = 1.5
        _HeightScale ("Vertical Stretch", Float) = 1.5
        _Alpha       ("Base Alpha", Range(0,1)) = 0.7
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend One One         // additive – bright glow
        Cull Back
        ZWrite Off

        Pass
        {
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
                float4 pos        : SV_POSITION;
                float3 worldPos   : TEXCOORD0;
                float3 worldNormal: TEXCOORD1;
            };

            float4 _AuraColor;
            float  _Intensity;
            float  _AuraStrength;
            float  _NoiseScale;
            float  _NoiseSpeed;
            float  _HeightScale;
            float  _Alpha;

            // simple hash-based noise
            float hash(float3 p)
            {
                p  = frac(p * 0.3183099 + 0.1);
                p *= 17.0;
                return frac(p.x * p.y * p.z * (p.x + p.y + p.z));
            }

            v2f vert (appdata v)
            {
                v2f o;
                float4 wpos = mul(unity_ObjectToWorld, v.vertex);
                // stretch upwards a bit for that “flame” look
                wpos.y += (v.vertex.y * (_HeightScale - 1));
                o.pos          = UnityObjectToClipPos(v.vertex);
                o.worldPos     = wpos.xyz;
                o.worldNormal  = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
                float3 n       = normalize(i.worldNormal);

                // Fresnel rim
                float ndotv   = saturate(dot(n, viewDir));
                float fresnel = pow(1.0 - ndotv, _AuraStrength);

                // Vertical factor – stronger higher up
                float height = saturate((i.worldPos.y - _WorldSpaceCameraPos.y * 0.0) * 0.2 + 0.5);

                // Scrolling noise
                float t   = _Time.y * _NoiseSpeed;
                float3 np = i.worldPos * _NoiseScale + float3(0, t, 0);
                float noise = hash(np);
                noise = smoothstep(0.3, 1.0, noise);

                // Combine – noisy, pulsing rim
                float aura = fresnel * height * noise;

                // small time pulse
                float pulse = 0.8 + 0.2 * sin(_Time.y * 6.0);
                aura *= pulse;

                float3 col = _AuraColor.rgb * aura * _Intensity;
                float  a   = aura * _Alpha;

                return float4(col, a);
            }
            ENDCG
        }
    }
}
