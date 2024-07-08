Shader "EzDim/GlowingGridUnlit" {
    Properties{
        _GridSize("Grid Size", Range(1, 50)) = 10
        _CircleRadius("Circle Radius", Range(0.01, 0.5)) = 0.1
        _EdgeBlur("Edge Blur", Range(0.001, 0.1)) = 0.01
        _Brightness("Brightness", Range(0, 10)) = 1
        _EmissionColor("Emission Color", Color) = (1, 1, 1, 1)
        _MinDistance("Minimum Distance", Range(0, 30)) = 1
        _MaxDistance("Maximum Distance", Range(0, 30)) = 30
    }
        SubShader{
            Tags { "RenderType" = "Opaque" }
            LOD 100

            Pass {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata {
                    float4 vertex : POSITION;
                };

                struct v2f {
                    float4 vertex : SV_POSITION;
                    float2 uv : TEXCOORD0;
                    float distance : TEXCOORD1;
                };

                float _GridSize;
                float _CircleRadius;
                float _EdgeBlur;
                float _Brightness;
                fixed4 _EmissionColor;
                float _MinDistance;
                float _MaxDistance;

                v2f vert(appdata v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.vertex.xy * (_GridSize - 1.0) / _GridSize + 0.5 / _GridSize;
                    o.distance = length(UnityObjectToViewPos(v.vertex));
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target {
                    // Calculate the position within the grid
                    float2 gridPosition = floor(i.uv * _GridSize);

                    // Calculate the relative position within the grid cell
                    float2 cellPosition = i.uv * _GridSize - floor(i.uv * _GridSize);

                    // Calculate distance from the center of the cell
                    float2 center = float2(0.5, 0.5);
                    float distance = length(cellPosition - center);

                    // Determine if the current pixel is inside the circle based on radius
                    float insideCircle = step(distance, _CircleRadius);

                    // Calculate the edge blur based on the circle radius
                    float edgeBlur = smoothstep(_CircleRadius - _EdgeBlur, _CircleRadius, distance);

                    // Determine if the current pixel is on a horizontal or vertical line
                    float onLine = step(cellPosition.x, _EdgeBlur) + step(cellPosition.y, _EdgeBlur);

                    // Calculate the brightness based on the distance to the camera
                    float distanceBrightness = 1.0 - smoothstep(_MinDistance, _MaxDistance, i.distance);

                    // Calculate the final color based on brightness, emission color, inside circle, edge blur, on line, and distance brightness
                    fixed4 color = _EmissionColor * _Brightness * insideCircle * (1 - edgeBlur + onLine) * distanceBrightness;

                    return color;
                }
                ENDCG
            }
    }
        FallBack "Diffuse"
}
