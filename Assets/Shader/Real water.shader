Shader "Unlit/Real water"
{
    Properties
    {
        _Tint("Tint", Color) = (1, 1, 1, 1)
        [Gamma] _Metallic ("Metallic", Range(0, 1)) = 0
        _Smoothness ("Smoothness", Range(0, 1)) = 0.5
        _LUT ("LUT", 2D) = "white" {}
        _depth ("Depth", float) = 1
        _strength ("Strength", float) = 1
        _shallowColor ("ShallowColor", Color) = (1, 1, 1, 1)
        _depthColor ("DepthColor", Color) = (1, 1, 1, 1)
        _waveDir1 ("WaveDir1", vector) = (1, 1, 1, 1)
        _waveParam1 ("WaveParam1", vector) = (1, 1, 1, 1)
        _waveDir2 ("WaveDir2", vector) = (1, 1, 1, 1)
        _waveParam2 ("WaveParam2", vector) = (1, 1, 1, 1)
        _waveDir3 ("WaveDir3", vector) = (1, 1, 1, 1)
        _waveParam3 ("WaveParam3", vector) = (1, 1, 1, 1)
        _waveDir4 ("WaveDir4", vector) = (1, 1, 1, 1)
        _waveParam4 ("WaveParam4", vector) = (1, 1, 1, 1)
        _Cubemap ("CubeMap", Cube) = "" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline"="UniversalPipeline"}



        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

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
                float4 screenPos : TEXCOORD1;
                float3 normal : NORMAL;
                float3 worldViewDir : TEXCOORD2;
                float3 worldReflectDir : TEXCOORD3;
            };

            CBUFFER_START(UnityPerMaterial)
            half4 _Tint;
            half _Metallic;
            half _Smoothness;
            float _depth;
            float _strength;
            half4 _shallowColor;
            half4 _depthColor;
            half4 _waveDir1;
            half4 _waveDir2;
            half4 _waveDir3;
            half4 _waveDir4;
            half4 _waveParam1;
            half4 _waveParam2;
            half4 _waveParam3;
            half4 _waveParam4;
            samplerCUBE _Cubemap;
            CBUFFER_END

            struct Wave2
            {
                float3 wavePos;
                float3 waveNormal;
            };

            Wave2 GerstnerWave_float (float4 waveDir, float4 waveParam, float3 p)
            {
                Wave2 o;
                // waveParam : steepness, waveLength, speed, amplify
                float steepness = waveParam.x;
                float wavelength = waveParam.y;
                float speed = waveParam.z;
                float amplify = waveParam.w;
                float2 d = normalize(waveDir.xz);
            
                float w = 2 * 3.1415 / wavelength;
                float f = w * (dot(d, p.xz) - _Time.y * speed);
                float sinf = sin(f);
                float cosf = cos(f);
            
                steepness = clamp(steepness, 0, 1 / (w*amplify));

                o.waveNormal = float3(
                    - amplify * w * d.x * cosf,
                    - steepness * amplify * w * sinf,
                    - amplify * w * d.y * cosf
                );

                o.wavePos = float3(
                    steepness * amplify * d.x * cosf,
                    amplify * sinf,
                    steepness * amplify * d.y * cosf
                );

                return o;
            }

            v2f vert (appdata v)
            {
                v2f o;
                float3 waveOffset = float3(0.0, 0.0, 0.0);
                float3 waveNormal = float3(0.0, 0.0, 0.0);

                Wave2 wave1 = GerstnerWave_float(_waveDir1, _waveParam1, v.vertex.xyz);
                waveOffset += wave1.wavePos;
                waveNormal += wave1.waveNormal;
                Wave2 wave2 = GerstnerWave_float(_waveDir2, _waveParam2, v.vertex.xyz);
                waveOffset += wave2.wavePos;
                waveNormal += wave2.waveNormal;
                Wave2 wave3 = GerstnerWave_float(_waveDir3, _waveParam3, v.vertex.xyz);
                waveOffset += wave3.wavePos;
                waveNormal += wave3.waveNormal;
                Wave2 wave4 = GerstnerWave_float(_waveDir4, _waveParam4, v.vertex.xyz);
                waveOffset += wave4.wavePos;
                waveNormal += wave4.waveNormal;

                v.vertex.xyz += waveOffset;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.normal = TransformObjectToWorldNormal(waveNormal);
                o.worldViewDir = GetCameraPositionWS();
                o.worldReflectDir = reflect(-o.worldViewDir, o.normal);
                o.uv = v.uv;
                o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                //潜水与深水MASK
                float sceneRawDepth = SampleSceneDepth(i.screenPos.xy / i.screenPos.w);
                float sceneEyeDepth = LinearEyeDepth(sceneRawDepth, _ZBufferParams);
                float sceneDepth = i.screenPos.w;
                float depthMask = sceneEyeDepth - (sceneDepth + _depth);

                //浅水与深水颜色
                depthMask *= _strength; //浅水与深水交界处平滑度
                depthMask = saturate(depthMask);
                half4 waterColor = lerp(_shallowColor, _depthColor, depthMask);

                float3 worldNormal = normalize(i.normal);
                float3 worldLightDir = normalize(_MainLightPosition.xyz);
                float3 worldViewDir = normalize(i.worldViewDir);
                float3 worldReflectDir = normalize(i.worldReflectDir);

                float3 Lambert = waterColor.rgb * saturate(dot(worldNormal, worldLightDir));
                float3 reflection = texCUBE(_Cubemap, worldReflectDir).rgb;

                return half4(Lambert + reflection, 1);
            }
            ENDHLSL
        }
    }
}
