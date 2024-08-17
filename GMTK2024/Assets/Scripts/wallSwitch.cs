using UnityEngine;

public class RaycastChecker : MonoBehaviour{
	public Transform startPoint; // The point from which to cast the rays
	public float maxRayDistance = 100f; // Maximum distance for raycasts

	public void Check(){
		if (CheckRaycasts()){
			TriggerFunction();
		}
	}

	private bool CheckRaycasts(){
		Collider collider = GetComponent<MeshCollider>();
		MeshCollider meshCollider = collider as MeshCollider;

		Mesh mesh = meshCollider.sharedMesh;

		Vector3[] vertices = mesh.vertices;
		Transform colliderTransform = meshCollider.transform;
		bool allRaysHitPlatform = true;

		foreach (Vector3 vertex in vertices){
			Vector3 worldVertex = colliderTransform.TransformPoint(vertex);
			Ray ray = new Ray(startPoint.position, worldVertex - startPoint.position);
			RaycastHit[] hits;


			bool hitPlatform = Physics.Raycast(ray, maxRayDistance, Stage.platformLayer);

			if (!hitPlatform){
				allRaysHitPlatform = false;
				break;
			}
		}

		return allRaysHitPlatform;
	}

	private void TriggerFunction(){
		Debug.Log("Button Triggered");
	}
}