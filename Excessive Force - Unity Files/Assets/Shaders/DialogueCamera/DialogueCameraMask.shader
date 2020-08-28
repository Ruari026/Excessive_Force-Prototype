Shader "Unlit/DialogueCameraMask"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Angle("Mask Angle", float) = 0.0
        _ScreenDims("Screen Dimentions", vector) = (1080, 1920, 0, 0)
        _Invert("Invert Mask", Range(0.0, 1.0)) = 0.0
    }
        SubShader
        {
            Tags { "RenderType" = "Fade" }
            LOD 100

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
                float _Angle;
                float2 _ScreenDims;
                float _Invert;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    // Making mask indipendant of screen size
                    float2 fragPos = float2(_ScreenDims.x * i.uv.x, _ScreenDims.y * i.uv.y);
                    float2 centerPos = float2(_ScreenDims.x / 2, _ScreenDims.y / 2);

                    float angle = atan2(fragPos.y - centerPos.y, fragPos.x - centerPos.x);
                    angle = (angle * 180 / 3.14) + 180;

                    // sample the texture
                    fixed4 col = tex2D(_MainTex, i.uv);
                    float minAngle = (_Angle + 90);
                    float maxAngle = (_Angle + 270);


                    // Handle inversion of mask
                    float invert = round(_Invert);
                    if (angle > minAngle && angle < maxAngle)
                    {
                        col.a = (0.0 + invert);
                    }
                    else
                    {
                        col.a = (1.0 - invert);
                    }
                    return col;
            }
            ENDCG
        }
    }
}
