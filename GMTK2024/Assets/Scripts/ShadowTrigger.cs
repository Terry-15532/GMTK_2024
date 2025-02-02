using UnityEngine;

public class ShadowTrigger : MonoBehaviour{
	public Transform startPoint; // The point from which to cast the rays
	public float maxRayDistance = 100f; // Maximum distance for raycasts

	public void Update(){
		if (CheckRaycasts()){
			TriggerFunction();
		}
	}

	private bool CheckRaycasts(){
        //Debug.Log("Checking rays");
        MeshCollider meshCollider = GetComponent<MeshCollider>();

		if (meshCollider != null){
			Mesh mesh = meshCollider.sharedMesh;

			Vector3[] vertices = mesh.vertices;
			Transform colliderTransform = meshCollider.transform;
			bool allRaysHitPlatform = true;

			foreach (Vector3 vertex in vertices){
				Vector3 worldVertex = colliderTransform.TransformPoint(vertex);
				Ray ray = new Ray(startPoint.position, worldVertex - startPoint.position);
				// RaycastHit[] hits;


				bool hitPlatform = Physics.Raycast(ray, maxRayDistance, Stage.platformLayer);

				if (!hitPlatform){
					allRaysHitPlatform = false;
					//Debug.Log("Ray didn't hit platform");
					break;
				}
			}

			return allRaysHitPlatform;
		}

		return false;
	}

	private void TriggerFunction(){
		//Debug.Log("Button Triggered");
	}
}