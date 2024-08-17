using System;
using UnityEngine;
using UnityEngine.Rendering;

public class ShadowInfo : MonoBehaviour{
	public Camera shadowCamera;

	public Vector3 inputPos;

	public bool isInShadow;

	public static RenderTexture shadowMap;

	public bool checking;

	// public async void CheckShadow(){
	// 	IsInShadow(inputPos, (b) => { });
	// }

	public void Update(){
		if (Input.GetKeyDown(KeyCode.Space)){
			Debug.Log("a");
			isInShadow = IsInShadow(transform.position);
			Debug.Log(isInShadow);
		}
	}
	
	public bool IsInShadow(Vector3 position)
	{
		// Create a temporary texture to store the shadow map data
		Texture2D shadowMapTexture = new Texture2D(shadowMap.width, shadowMap.height, TextureFormat.RFloat, false);

		// Set the active render texture to the shadow map
		RenderTexture.active = shadowMap;

		// Read the pixels from the shadow map into the texture
		shadowMapTexture.ReadPixels(new Rect(0, 0, shadowMap.width, shadowMap.height), 0, 0);
		shadowMapTexture.Apply();

		// Transform the input position to shadow map coordinates
		Matrix4x4 worldToShadowMatrix = shadowCamera.projectionMatrix * shadowCamera.worldToCameraMatrix;
		Vector4 shadowCoord = worldToShadowMatrix * new Vector4(position.x, position.y, position.z, 1.0f);
		shadowCoord /= shadowCoord.w;

		// Check if the position is in shadow
		float shadowValue = shadowMapTexture.GetPixelBilinear(shadowCoord.x, shadowCoord.y).r;

		// Clean up
		RenderTexture.active = null;
		Destroy(shadowMapTexture);

		// Return true if the position is in shadow, false otherwise
		return shadowValue < 0.5f;
	}

	// public async void IsInShadow(Vector3 position, Action<bool> callback){
	// 	// Create a temporary texture to store the shadow map data
	// 	Texture2D shadowMapTexture = new Texture2D(shadowMap.width, shadowMap.height, TextureFormat.RFloat, false);
	//
	// 	// Request the shadow map data from the GPU
	// 	AsyncGPUReadback.Request(shadowMap, 0, request => {
	// 		if (request.hasError){
	// 			Debug.LogError("GPU readback error.");
	// 			return;
	// 		}
	//
	// 		// Copy the data to the texture
	// 		shadowMapTexture.LoadRawTextureData(request.GetData<float>());
	// 		shadowMapTexture.Apply();
	// 	});
	//
	// 	int i = 0;
	//
	//
	// 	// Transform the input position to shadow map coordinates
	// 	Matrix4x4 worldToShadowMatrix = shadowCamera.projectionMatrix * shadowCamera.worldToCameraMatrix;
	// 	Vector4 shadowCoord = worldToShadowMatrix * new Vector4(position.x, position.y, position.z, 1.0f);
	// 	shadowCoord /= shadowCoord.w;
	//
	// 	// Check if the position is in shadow
	// 	float shadowValue = shadowMapTexture.GetPixelBilinear(shadowCoord.x, shadowCoord.y).r;
	//
	// 	// Clean up
	// 	Destroy(shadowMapTexture);
	//
	// 	// Return true if the position is in shadow, false otherwise
	// }
}