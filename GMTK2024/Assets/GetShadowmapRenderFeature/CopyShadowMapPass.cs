using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Obsolete]
public class CopyShadowMapPass : ScriptableRenderPass
{
	private RenderTargetHandle shadowMapCopy;
	private RenderTargetIdentifier shadowMap;

	public CopyShadowMapPass()
	{
		shadowMapCopy.Init("_ShadowMapCopy");
	}

	[Obsolete]
	public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
	{
		shadowMap = BuiltinRenderTextureType.CurrentActive;
		cmd.GetTemporaryRT(shadowMapCopy.id, cameraTextureDescriptor);
	}

	[Obsolete]
	public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		CommandBuffer cmd = CommandBufferPool.Get("Copy Shadow Map");
		cmd.Blit(shadowMap, shadowMapCopy.Identifier());
		context.ExecuteCommandBuffer(cmd);
		CommandBufferPool.Release(cmd);

		ShadowInfo.shadowMap = Shader.GetGlobalTexture("_ShadowMapCopy") as RenderTexture;
	}

	public override void FrameCleanup(CommandBuffer cmd)
	{
		cmd.ReleaseTemporaryRT(shadowMapCopy.id);
	}
}