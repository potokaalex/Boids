Shader "Custom/DMIIShader"
{
    Properties
    {
        [NoScaleOffset] _MainTex("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags
        {
            "Queue" = "Background"
            "RenderType" = "Background"
        }
        Cull Back
        ZWrite Off
        Lighting Off
        Blend SrcAlpha OneMinusSrcAlpha
        
        Pass
        {            
            CGPROGRAM
            #pragma vertex vertexProgram
            #pragma fragment fragmentProgram
            #pragma multi_compile_instancing
            #pragma instancing_options procedural:setup

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            fixed4 _Color;

            struct vertexInput
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct fragmentInput
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            struct fragmentOutput
            {
                float4 color : SV_Target;
            };

            CBUFFER_START(Data)
                float4 PositionsAndRotationsBuffer[1023];
            CBUFFER_END

            fragmentInput vertexProgram(vertexInput v)
            {
                UNITY_SETUP_INSTANCE_ID(v);
                fragmentInput output;

                output.pos = UnityObjectToClipPos(v.pos);
                output.uv = v.uv;

                return output;
            }

            fragmentOutput fragmentProgram(fragmentInput f) 
            {
                fragmentOutput output;

                output.color = tex2D(_MainTex, f.uv) * float4(_Color.r, _Color.g, _Color.b, 1);
                
                return output;
            }

            #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
            void setup() 
            {
                float2 position = PositionsAndRotationsBuffer[unity_InstanceID].xy - _ScreenParams.xy / 2;
                float2 rotations = float2(
                    cos(PositionsAndRotationsBuffer[unity_InstanceID].z * 0.0174533),
                    sin(PositionsAndRotationsBuffer[unity_InstanceID].w * 0.0174533));

                unity_ObjectToWorld = float4x4(
                    rotations.x, -rotations.y, 0, position.x,
                    rotations.y, rotations.x, 0, position.y,
                    0, 0, 1, 0,
                    0, 0, 0, 1);
            }
            #endif
            ENDCG
        }
    }
}
