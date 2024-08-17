using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[Obsolete]
public class GetShadowMap : ScriptableRendererFeature
{
	CopyShadowMapPass copyShadowMapPass;

	public override void Create()
	{
		copyShadowMapPass = new CopyShadowMapPass();
		copyShadowMapPass.renderPassEvent = RenderPassEvent.AfterRenderingShadows;
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		renderer.EnqueuePass(copyShadowMapPass);
	}
}