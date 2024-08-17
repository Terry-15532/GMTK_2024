using UnityEngine;
using System.Collections.Generic;

public class RaycastGridCone : MonoBehaviour
{
    public Camera targetCamera;
    public Transform rayOrigin;
    public float rayDistance = 100f;
    public int gridResolution = 10; // 10x10 grid
    public float coneAngle = 30f; // Angle of the cone in degrees
    public float squareSize = 0.1f;
    public float extrusionThickness = 0.5f;
    public float moveDistanceFraction = 0.5f;

    private List<Vector3> wallHitPoints = new List<Vector3>();

    void Check()
    {
        wallHitPoints.Clear();
        if (Input.GetMouseButtonDown(0))
        {
            // Loop over gridResolution x gridResolution grid
            for (int x = 0; x < gridResolution; x++)
            {
                for (int y = 0; y < gridResolution; y++)
                {
                    // Calculate grid position in normalized coordinates (-0.5 to 0.5)
                    float u = (x / (float)(gridResolution - 1)) - 0.5f;
                    float v = (y / (float)(gridResolution - 1)) - 0.5f;

                    // Calculate the direction of the ray in the cone
                    Vector3 direction = Quaternion.Euler(v * coneAngle, u * coneAngle, 0) * rayOrigin.forward;

                    // Perform the raycast
                    Ray ray = new Ray(rayOrigin.position, direction);
                    RaycastHit[] hits;

                    // Perform the raycast and get all hits
                    hits = Physics.RaycastAll(ray, rayDistance, Stage.wallLayer);

                    // Sort the hits by distance
                    System.Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));

                    // Draw the ray in the editor
                    // Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red, 2.0f);

                    bool passedThroughPlatform = false;

                    // Iterate through the hits
                    foreach (RaycastHit hit in hits)
                    {
                        // Check if the ray has hit an object tagged "platform"
                        if (hit.collider.CompareTag("Platform"))
                        {
                            passedThroughPlatform = true; // Mark that we've passed through a platform
                        }
                        // Check if the ray has hit an object tagged "wall" after passing through a platform
                        if (hit.collider.CompareTag("Wall") && passedThroughPlatform)
                        {
                            Debug.Log("Hit a wall after passing through a platform: " + hit.collider.name);
                            wallHitPoints.Add(hit.point);
                            // Perform any additional actions here, like drawing a debug line
                            // Debug.DrawLine(ray.origin, hit.point, Color.green, 2.0f);
                        }
                    }
                }
            }
            if (wallHitPoints.Count > 2)
            {
                MovePointsTowardsCamera(wallHitPoints, moveDistanceFraction);
                CreateColliderFromShape(wallHitPoints, extrusionThickness);
            }
        }
    }

    void MovePointsTowardsCamera(List<Vector3> points, float distanceFraction)
    {
        float length;
        if (targetCamera == null)
            return;

        Vector3 cameraPosition = targetCamera.transform.position;

        for (int i = 0; i < points.Count; i++)
        {
            length = (cameraPosition - points[i]).magnitude * distanceFraction;
            Vector3 directionToCamera = (cameraPosition - points[i]).normalized;
            points[i] += directionToCamera * length;
        }
    }

    void CreateColliderFromShape(List<Vector3> points, float thickness)
    {
        // Sort points in clockwise or counterclockwise order
        points = SortPointsClockwise(points);

        // Create a new GameObject to hold the collider
        GameObject shapeObject = new GameObject("ExtrudedCollider");
        shapeObject.transform.position = Vector3.zero;

        // Generate the mesh
        Mesh mesh = GenerateExtrudedMesh(points, thickness);
        MeshFilter meshFilter = shapeObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        // Add a MeshCollider
        MeshCollider meshCollider = shapeObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
        meshCollider.convex = true; // Use convex for proper collision detection

    }

    List<Vector3> SortPointsClockwise(List<Vector3> points)
    {
        Vector3 center = Vector3.zero;
        foreach (var point in points)
        {
            center += point;
        }
        center /= points.Count;

        points.Sort((a, b) => Mathf.Atan2(a.y - center.y, a.x - center.x)
                        .CompareTo(Mathf.Atan2(b.y - center.y, b.x - center.x)));

        return points;
    }

    Mesh GenerateExtrudedMesh(List<Vector3> points, float thickness)
    {
        Mesh mesh = new Mesh();

        // Define vertices
        Vector3[] vertices = new Vector3[points.Count * 2];
        int[] triangles = new int[(points.Count - 2) * 6 + points.Count * 6];
        Vector3[] normals = new Vector3[vertices.Length];
        Vector2[] uv = new Vector2[vertices.Length];

        for (int i = 0; i < points.Count; i++)
        {
            vertices[i] = points[i];
            vertices[i + points.Count] = points[i] + Vector3.forward * thickness;
            normals[i] = -Vector3.forward;
            normals[i + points.Count] = Vector3.forward;
            uv[i] = new Vector2(vertices[i].x, vertices[i].y);
            uv[i + points.Count] = new Vector2(vertices[i + points.Count].x, vertices[i + points.Count].y);
        }

        // Create the triangles
        int triIndex = 0;
        for (int i = 0; i < points.Count - 2; i++)
        {
            triangles[triIndex++] = 0;
            triangles[triIndex++] = i + 1;
            triangles[triIndex++] = i + 2;

            triangles[triIndex++] = points.Count;
            triangles[triIndex++] = points.Count + i + 2;
            triangles[triIndex++] = points.Count + i + 1;
        }

        for (int i = 0; i < points.Count; i++)
        {
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