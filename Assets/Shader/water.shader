Shader "water/water shader"
{
    Properties
    {
        _Foam ("Foam", 2D) = "white" {}
        _ShallowColor ("ShallowColor", Color) = (1, 1, 1, 1)
        _DepthColor ("DepthColor", Color) = (1, 1, 1, 1)
        _Depth ("Depth", float) = 1
        _DepthStrength ("DepthStrength", float) = 1

        _NormalMap1 ("NormalMap1", 2D) = "bump" {}
        _NormalMap2 ("NormalMap2", 2D) = "bump" {}
        _NormalSpeed ("NormalSpeed", vector) = (0, 0, 0, 0)
        _NormalScale ("NormalScale", Range(0, 1)) = 1

        _SpecularStrength ("SpecularStrength", Range(0, 1)) = 1
        _WaterGloss ("WaterGloss", Range(0, 255)) = 30

        _RimPower ("RimPower", Range(1, 30)) = 8

        _FoamColor ("FomaColor", Color) = (1,1,1,1)
        _FoamFactor ("FoamFactor",Range(0,10)) = 0.2
        _FoamOffset("FoamOffset", vector) = (-0.01,0.01, 2, 0.01)

        _waveDir1 ("WaveDir1", vector) = (1, 1, 1, 1)
        _waveParam1 ("WaveParam1", vector) = (1, 1, 1, 1)
        _waveDir2 ("WaveDir2", vector) = (1, 1, 1, 1)
        _waveParam2 ("WaveParam2", vector) = (1, 1, 1, 1)
        _waveDir3 ("WaveDir3", vector) = (1, 1, 1, 1)
        _waveParam3 ("WaveParam3", vector) = (1, 1, 1, 1)
        _waveDir4 ("WaveDir4", vector) = (1, 1, 1, 1)
        _waveParam4 ("WaveParam4", vector) = (1, 1, 1, 1)
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

            CBUFFER_START(UnityPerMaterial)
            half4 _ShallowColor;
            half4 _DepthColor;
            float4 _Foam_ST;
            float4 _NormalSpeed;
            float4 _NormalMap1_ST;
            float4 _NormalMap2_ST;
            half _NormalScale;

            half _SpecularStrength;
            float _WaterGloss;

            half _RimPower;

            half _Depth;
            half _DepthStrength;

            half4 _FoamColor;
            float _FoamFactor;
            half4 _FoamOffset;

            float4 _waveDir1;
            float4 _waveDir2;
            float4 _waveDir3;
            float4 _waveDir4;
            float4 _waveParam1;
            float4 _waveParam2;
            float4 _waveParam3;
            float4 _waveParam4;
            CBUFFER_END

            TEXTURE2D(_Foam); SAMPLER(sampler_Foam);
            TEXTURE2D(_NormalMap1); SAMPLER(sampler_NormalMap1);
            TEXTURE2D(_NormalMap2); SAMPLER(sampler_NormalMap2);

            half3 BlendNormals(half3 n1, half3 n2)
            {
                return normalize(half3(n1.xy + n2.xy, n1.z * n2.z));
            }

            float3 GerstnerWave_float (float4 waveDir, float4 waveParam, float3 p)
            {
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

                return float3
                (
                    steepness * amplify * d.x * cosf,
                    amplify * sinf,
                    steepness * amplify * d.y * cosf
                );
            }

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
                float4 vertex : SV_POSITION;
                float4 TtoW0 : TEXCOORD2;
                float4 TtoW1 : TEXCOORD3;
                float4 TtoW2 : TEXCOORD4;
                float4 screenPos : TEXCOORD5;
            };

            v2f vert (appdata v)
            {
                v2f o;

                float3 waveOffset = float3(0.0, 0.0, 0.0);

                waveOffset = GerstnerWave_float(_waveDir1, _waveParam1, v.vertex.xyz);
                waveOffset = GerstnerWave_float(_waveDir2, _waveParam2, v.vertex.xyz);
                waveOffset = GerstnerWave_float(_waveDir3, _waveParam3, v.vertex.xyz);
                waveOffset = GerstnerWave_float(_waveDir4, _waveParam4, v.vertex.xyz);

                v.vertex.xyz += waveOffset;

                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv.xy = TRANSFORM_TEX(v.uv, _Foam);
                o.uv1.xy = TRANSFORM_TEX(v.uv, _NormalMap1);
                o.uv1.zw = TRANSFORM_TEX(v.uv, _NormalMap2);

                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                float3 worldNormal = TransformObjectToWorldNormal(v.normal);
                float3 worldTangent = TransformObjectToWorldDir(v.tangent.xyz);
                float3 worldBinormal = cross(worldNormal, worldTangent) * v.tangent.w;

                o.TtoW0 = float4(worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x);
                o.TtoW1 = float4(worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y);
                o.TtoW2 = float4(worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z);

                o.screenPos = ComputeScreenPos(o.vertex);

                return o;
            }

            half4 frag (v2f i) : SV_Target
            {

                float sceneRawDepth = SampleSceneDepth(i.screenPos.xy / i.screenPos.w);
                float sceneEyeDepth = LinearEyeDepth(sceneRawDepth, _ZBufferParams);
                float sceneDepth = i.screenPos.w;
                float depthMask = sceneEyeDepth - (sceneDepth + _Depth);

                depthMask *= _DepthStrength; //浅水与深水交界处平滑度
                depthMask = saturate(depthMask);

                half2 panner1 = _Time.y * _NormalSpeed.xy + i.uv1.xy;
                half2 panner2 = _Time.y * _NormalSpeed.zw + i.uv1.zw;
                half3 worldNormal = BlendNormals(UnpackNormal(SAMPLE_TEXTURE2D(_NormalMap1, sampler_NormalMap1, panner1)) , UnpackNormal(SAMPLE_TEXTURE2D(_NormalMap2, sampler_NormalMap2, panner2)));
                worldNormal = lerp(half3(0, 0, 1), worldNormal, _NormalScale);
                worldNormal = normalize(half3(dot(i.TtoW0.xyz, worldNormal), dot(i.TtoW1.xyz, worldNormal), dot(i.TtoW2.xyz, worldNormal)));

                half3 water = SAMPLE_TEXTURE2D(_Foam, sampler_Foam, i.uv + worldNormal.xy * _FoamOffset.w).rgb;
                half3 water2 = SAMPLE_TEXTURE2D(_Foam, sampler_Foam, _Time.y * _FoamOffset.xy + i.uv + worldNormal.xy*_FoamOffset.w).rgb;
                half4 diffuse = lerp(_ShallowColor, _DepthColor, depthMask);

                half temp_output = saturate((water.g + water2.g) * (1 - depthMask) * water.g - _FoamFactor);
                diffuse = lerp(diffuse, _FoamColor, temp_output);

                // half temp = saturate(sin((depthMask) + _Time.y * _FoamSpeed)) * (1 - depthMask);
                // half3 foamcolor = water.r * _FoamColor.rgb * temp;
                // return half4(foamcolor, 1);

                half3 worldPos = half3(i.TtoW0.w, i.TtoW1.w, i.TtoW2.w);
                half3 viewDir = normalize(GetCameraPositionWS() - worldPos);
                float NdotV = saturate(dot(worldNormal, viewDir));

                half3 halfDir = normalize(_MainLightPosition.xyz + viewDir);
                half3 specular = _MainLightColor.rgb * _SpecularStrength * pow(max(0, dot(worldNormal, halfDir)), _WaterGloss);
                specular = lerp(diffuse.rgb * specular, specular, _WaterGloss / 255);

                half3 rim = pow(1 - saturate(NdotV), _RimPower) * _MainLightColor.rgb;

                return diffuse + half4(rim + specular, 0);
            }
            ENDHLSL
        }
    }
}
