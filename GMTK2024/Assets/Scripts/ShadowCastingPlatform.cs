using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Serialization;

public class ShadowCastingPlatform : MonoBehaviour{
	public static Transform lightPos, cameraPos;
	public static Collider targetCollider;
	public const int rayDist = 100;
	public float extrusionThickness = 0.5f;
	public float moveDistanceFraction = 0.5f;
	private List<Vector3> wallHitPoints = new List<Vector3>();

	public Collider collider;

	public void Start(){
		cameraPos = GameInfo.mainCamera.transform;
		lightPos = Stage.instance.currLight.transform;
		collider = new GameObject("ExtrudedCollider").AddComponent<MeshCollider>();
		collider.gameObject.AddComponent<MeshFilter>();
		collider.gameObject.AddComponent<MeshRenderer>();
		collider.GetComponent<MeshRenderer>().materials[0] = Stage.instance.shadowColliderMat;
		Stage.instance.updateShadow.AddListener(UpdateShadowCollider);
	}

	public void UpdateShadowCollider(){
		wallHitPoints.Clear();
		if (targetCollider is MeshCollider meshCollider){
			Vector3[] vertices = meshCollider.sharedMesh.vertices;
			Transform colliderTransform = targetCollider.transform;

			foreach (Vector3 vertex in vertices){
				// Convert vertex position to world space
				Vector3 worldVertex = colliderTransform.TransformPoint(vertex);

				// Cast a ray towards the vertex from a point (e.g., the camera or a specific point in the scene)
				Ray ray = new Ray(lightPos.position, (worldVertex - lightPos.position).normalized);
				RaycastHit[] hits;

				int l = 1 << LayerMask.NameToLayer("Wall");

				hits = Physics.RaycastAll(ray, rayDist, l);

				System.Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));

				foreach (RaycastHit hit in hits){
					Debug.Log("Hit a wall after passing through a platform: " + hit.collider.name);
					wallHitPoints.Add(hit.point);
				}
			}

			if (wallHitPoints.Count > 2){
				MovePointsTowardsCamera(wallHitPoints, moveDistanceFraction);
				var m = CreateMeshFromShape(wallHitPoints, extrusionThickness);
				collider.GetComponent<MeshFilter>().mesh = m;
				collider.GetComponent<MeshCollider>().sharedMesh = m;
			}
		}
	}

	void MovePointsTowardsCamera(List<Vector3> points, float distanceFraction){
		float length;
		if (cameraPos == null)
			return;

		Vector3 cameraPosition = cameraPos.transform.position;

		for (int i = 0; i < points.Count; i++){
			length = (cameraPosition - points[i]).magnitude * distanceFraction;
			Vector3 directionToCamera = (cameraPosition - points[i]).normalized;
			points[i] += directionToCamera * length;
		}
	}

	Mesh CreateMeshFromShape(List<Vector3> points, float thickness){
		points = SortPointsClockwise(points);

		collider.transform.position = Vector3.zero;

		return GenerateExtrudedMesh(points, thickness);
	}

	List<Vector3> SortPointsClockwise(List<Vector3> points){
		Vector3 center = Vector3.zero;
		foreach (var point in points){
			center += point;
		}

		center /= points.Count;

		points.Sort((a, b) => Mathf.Atan2(a.y - center.y, a.x - center.x)
			.CompareTo(Mathf.Atan2(b.y - center.y, b.x - center.x)));

		return points;
	}

	Mesh GenerateExtrudedMesh(List<Vector3> points, float thickness){
		Mesh mesh = new Mesh();

		// Define vertices
		Vector3[] vertices = new Vector3[points.Count * 2];
		int[] triangles = new int[(points.Count - 2) * 6 + points.Count * 6];
		Vector3[] normals = new Vector3[vertices.Length];
		Vector2[] uv = new Vector2[vertices.Length];

		for (int i = 0; i < points.Count; i++){
			vertices[i] = points[i];
			vertices[i + points.Count] = points[i] + Vector3.forward * thickness;
			normals[i] = -Vector3.forward;
			normals[i + points.Count] = Vector3.forward;
			uv[i] = new Vector2(vertices[i].x, vertices[i].y);
			uv[i + points.Count] = new Vector2(vertices[i + points.Count].x, vertices[i + points.Count].y);
		}

		// Create the triangles
		int triIndex = 0;
		for (int i = 0; i < points.Count - 2; i++){
			triangles[triIndex++] = 0;
			triangles[triIndex++] = i + 1;
			triangles[triIndex++] = i + 2;

			triangles[triIndex++] = points.Count;
			triangles[triIndex++] = points.Count + i + 2;
			triangles[triIndex++] = points.Count + i + 1;
		}

		for (int i = 0; i < points.Count; i++){
			int next = (i + 1) % points.Count;
			triangles[triIndex++] = i;
			triangles[triIndex++] = next;
			triangles[triIndex++] = i + points.Count;

			triangles[triIndex++] = next;
			triangles[triIndex++] = next + points.Count;
			triangles[triIndex++] = i + points.Count;
		}

		// Assign to mesh
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.normals = normals;
		mesh.uv = uv;

		mesh.RecalculateNormals();
		mesh.RecalculateBounds();

		return mesh;
	}
}