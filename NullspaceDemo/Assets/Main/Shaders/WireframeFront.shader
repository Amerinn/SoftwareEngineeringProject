Shader "Unlit/WireframeFront"
{
    Properties
    {
        _BaseMap("Texture", 2D) = "white" {}
        _WireframeColour("Wireframe front colour", color) = (1.0, 1.0, 1.0, 1.0)
        _WireframeAliasing("Wireframe aliasing", float) = 1.25
    }
    SubShader
    {
        Tags {
            "RenderPipeline"="UniversalPipeline"
            "Queue"="Transparent"
            }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Back
        ZWrite Off

        Pass
        {
            HLSLPROGRAM
            #pragma target 4.5
            #pragma require geometry
            #pragma multi_compile_instancing
            #pragma instancing_options renderinglayer
            #pragma multi_compile _ DOTS_INSTANCING_ON
            #pragma vertex vert
            #pragma fragment frag
            #pragma geometry geom

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct g2f {
                float4 pos : SV_POSITION;
                float3 barycentric : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            CBUFFER_START(UnityPerMaterial)
            float4 _BaseMap_ST;
            float4 _WireframeColour;
            float _WireframeAliasing;
            CBUFFER_END

            #ifdef UNITY_DOTS_INSTANCING_ENABLED
                UNITY_DOTS_INSTANCING_START(MaterialPropertyMetadata)
                    UNITY_DOTS_INSTANCED_PROP(float4, _BaseMap_ST)
                    UNITY_DOTS_INSTANCED_PROP(float4, _WireframeColour)
                    UNITY_DOTS_INSTANCED_PROP(float, _WireframeAliasing)
                UNITY_DOTS_INSTANCING_END(MaterialPropertyMetadata)

            #define _BaseMap_ST UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4, _BaseMap_ST)
            #define _WireframeColour UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4, _WireframeColour)
            #define _WireframeAliasing UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float, _WireframeAliasing)
            #endif



            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            v2f vert(appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                o.vertex = v.vertex;
                o.uv = TRANSFORM_TEX(v.uv, _BaseMap);
                return o;
            }

            [maxvertexcount(3)]
            void geom(triangle v2f IN[3], inout TriangleStream<g2f> triStream) {
                g2f o = (g2f)0;
                UNITY_SETUP_INSTANCE_ID(IN[0]);
                UNITY_TRANSFER_INSTANCE_ID(IN[0], o);

                float edgeLengthX = length(IN[1].vertex - IN[2].vertex);
                float edgeLengthY = length(IN[0].vertex - IN[2].vertex);
                float edgeLengthZ = length(IN[0].vertex - IN[1].vertex);
                float3 modifier = float3(0.0, 0.0, 0.0);
                if ((edgeLengthX > edgeLengthY) && (edgeLengthX > edgeLengthZ))
                    modifier = float3(1.0, 0.0, 0.0);
                else if ((edgeLengthY > edgeLengthX) && (edgeLengthY > edgeLengthZ))
                    modifier = float3(0.0, 1.0, 0.0);
                else if ((edgeLengthZ > edgeLengthX) && (edgeLengthZ > edgeLengthY))
                    modifier = float3(0.0, 0.0, 1.0);

                o.pos = mul(UNITY_MATRIX_MVP, IN[0].vertex);
                o.barycentric = float3(1, 0, 0) + modifier;
                triStream.Append(o);

                o.pos = mul(UNITY_MATRIX_MVP, IN[1].vertex);
                o.barycentric = float3(0, 1, 0) + modifier;
                triStream.Append(o);

                o.pos = mul(UNITY_MATRIX_MVP, IN[2].vertex);
                o.barycentric = float3(0, 0, 1) + modifier;
                triStream.Append(o);
            }

            float4 frag(g2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);

                float3 unitWidth = fwidth(i.barycentric);
                float3 aliased = smoothstep(float3(0.0, 0.0, 0.0), unitWidth * _WireframeAliasing, i.barycentric);
                float alpha = 1 - min(aliased.x, min(aliased.y, aliased.z));
                return float4(_WireframeColour.r, _WireframeColour.g, _WireframeColour.b, alpha);
            }
            ENDHLSL
        }
    }
}
