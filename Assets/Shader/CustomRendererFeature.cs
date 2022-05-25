using System;
using System.Security.Cryptography.X509Certificates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CustomRendererFeature : ScriptableRendererFeature
{
    public class CustomRendererPass : ScriptableRenderPass
    {
        private Setting setting;
        public Material material = null;
        string m_ProfilerTag;

        RenderTargetIdentifier source;
        RenderTargetIdentifier destination;
        RenderTargetHandle m_TemporaryColorTexture;
        public CustomRendererPass(RenderPassEvent evt, Setting setting, string tag)
        {
            renderPassEvent = evt;
            this.setting = setting;
            material = setting.material;
            m_ProfilerTag = tag;
            m_TemporaryColorTexture.Init("_TemporaryColorTexture");
        }

        public void Setup(RenderTargetIdentifier source) {}
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if(material == null && renderingData.cameraData.postProcessEnabled)
            {
                Debug.LogError("Material Not Appoint Or Not Enable PostProcess");
                return;
            }

            CommandBuffer cmd = CommandBufferPool.Get(m_ProfilerTag);
            RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
            opaqueDesc.depthBufferBits = 0;

            source = renderingData.cameraData.renderer.cameraColorTarget;
            destination = renderingData.cameraData.renderer.cameraColorTarget;

            cmd.GetTemporaryRT(m_TemporaryColorTexture.id, opaqueDesc, FilterMode.Bilinear);
            Blit(cmd, source, m_TemporaryColorTexture.Identifier(), material, setting.materialPassIndex);
            Blit(cmd, m_TemporaryColorTexture.Identifier(), destination);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(m_TemporaryColorTexture.id);
        }
    }

    [System.Serializable]
    public class Setting
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
        public Material material;
        public int materialPassIndex = 0;
    }

    public Setting setting = new Setting();
    CustomRendererPass customRendererPass;

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        customRendererPass.Setup(renderer.cameraColorTarget);
        renderer.EnqueuePass(customRendererPass);
    }

    public override void Create()
    {
        var passIndex = setting.material != null ? setting.material.passCount - 1 : 1;
		setting.materialPassIndex = Mathf.Clamp(setting.material.passCount, -1, passIndex);
        customRendererPass = new CustomRendererPass(setting.renderPassEvent, setting, name);
    }
}
