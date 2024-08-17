using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework.Constraints;

public class spawnShadowShape : MonoBehaviour
{
    public Camera targetCamera;
    public Collider targetCollider;
    public Transform startPoint;
    public PhysicsMaterial material;
    public float rayDistance = 100f;
    public int gridResolution = 10; // 10x10 grid
    // public float coneAngle = 30f; // Angle of the cone in degrees
    // public float squareSize = 0.1f;
    public float extrusionThickness = 1f;
    public float moveDistanceFraction = 0.5f;
    public int intervalsPerEdge = 10;

    private List<Vector3> wallHitPoints = new List<Vector3>();

    void Update()
    {
        wallHitPoints.Clear();
        if (Input.GetMouseButtonDown(0))
        {
            if (targetCollider is MeshCollider meshCollider)
            {
                Mesh mesh = meshCollider.sharedMesh;
                Vector3[] vertices = mesh.vertices;
                Transform colliderTransform = targetCollider.transform;

                foreach (Vector3 vertex in vertices)
                {
                    // Convert vertex position to world space
                    Vector3 worldVertex = colliderTransform.TransformPoint(vertex);

                    // Cast a ray towards the vertex from a point (e.g., the camera or a specific point in the scene)
                    Ray ray = new Ray(startPoint.position, (worldVertex - startPoint.position).normalized);
                    RaycastHit[] hits;

                    // Perform the raycast and get all hits
                    hits = Physics.RaycastAll(ray, rayDistance);

                    // Sort the hits by distance
                    System.Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));

                    // Draw the ray in the editor
                    // Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red, 2.0f);

                    // bool passedThroughPlatform = false;

                    // Iterate through the hits
                    foreach (RaycastHit hit in hits)
                    {
                        // Check if the ray has hit an object tagged "wall" after passing through a platform
                        if (hit.collider.CompareTag("Wall"))
                        {
                            Debug.Log("Hit a wall after passing through a platform: " + hit.collider.name);
                            wallHitPoints.Add(hit.point);
                            // Perform any additional actions here, like drawing a debug line
                            Debug.DrawLine(ray.origin, hit.point, Color.green, 200.0f);
                            break;
                        }
                    }
                }
                CastRaysThroughEdges(mesh, colliderTransform);

                if (wallHitPoints.Count > 2)
                {
                    // Move hit points towards the camera before creating the collider
                    MovePointsTowardsCamera(wallHitPoints, moveDistanceFraction);
                    CreateColliderFromShape(wallHitPoints, extrusionThickness);
                }
            }
            else
            {
                Debug.LogError("Target collider is not a MeshCollider.");
            }
        }
    }
    void CastRaysThroughEdges(Mesh mesh, Transform meshTransform)
    {
        // Get the vertices and triangles of the mesh
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        HashSet<(int, int)> edgeSet = new HashSet<(int, int)>();

        // Extract unique edges from the triangles
        for (int i = 0; i < triangles.Length; i += 3)
        {
            AddEdgeToSet(edgeSet, triangles[i], triangles[i + 1]);
            AddEdgeToSet(edgeSet, triangles[i + 1], triangles[i + 2]);
            AddEdgeToSet(edgeSet, triangles[i + 2], triangles[i]);
        }

        // Cast rays through each edge at intervals
        foreach (var edge in edgeSet)
        {
            Vector3 vertex1 = meshTransform.TransformPoint(vertices[edge.Item1]);
            Vector3 vertex2 = meshTransform.TransformPoint(vertices[edge.Item2]);

            for (int i = 0; i <= intervalsPerEdge; i++)
            {
                float t = (float)i / intervalsPerEdge;
                Vector3 pointOnEdge = Vector3.Lerp(vertex1, vertex2, t);
                CastRayFromCameraToPoint(pointOnEdge);
            }
        }
    }

    void CastRayFromCameraToPoint(Vector3 targetPoint)
    {
        Ray ray = new Ray(startPoint.position, (targetPoint - startPoint.position).normalized);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray, rayDistance);
        foreach (RaycastHit hit in hits)
        {
            // Check if the ray has hit an object tagged "wall" after passing through a platform
            if (hit.collider.CompareTag("Wall"))
            {
                Debug.Log("Hit a wall after passing through a platform: " + hit.collider.name);
                wallHitPoints.Add(hit.point);
                // Perform any additional actions here, like drawing a debug line
                Debug.DrawLine(ray.origin, hit.point, Color.red, 200.0f);
                break;
            }
        }
    }

    void AddEdgeToSet(HashSet<(int, int)> edgeSet, int index1, int index2)
    {
        var edge = (index1 < index2) ? (index1, index2) : (index2, index1);
        edgeSet.Add(edge);
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
        meshCollider.material = material;

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

        // Define the number of vertices (front and back faces)
        int vertexCount = points.Count * 2;

        // Define vertices
        Vector3[] vertices = new Vector3[vertexCount];
        Vector3[] normals = new Vector3[vertexCount];
        Vector2[] uv = new Vector2[vertexCount];

        // Calculate the number of triangles
        int sideTrianglesCount = points.Count * 2 * 3;
        int faceTrianglesCount = (points.Count - 2) * 3 * 2;
        int totalTrianglesCount = sideTrianglesCount + faceTrianglesCount;

        int[] triangles = new int[totalTrianglesCount];

        // Set the front face vertices and normals
        for (int i = 0; i < points.Count; i++)
        {
            vertices[i] = points[i];
            vertices[i + points.Count] = points[i] + Vector3.forward * thickness;

            normals[i] = -Vector3.forward;  // Normal for front face
            normals[i + points.Count] = Vector3.forward;  // Normal for back face

            uv[i] = new Vector2(vertices[i].x, vertices[i].y);
            uv[i + points.Count] = new Vector2(vertices[i + points.Count].x, vertices[i + points.Count].y);
        }

        int triIndex = 0;

        // Create triangles for the front face
        for (int i = 1; i < points.Count - 1; i++)
        {
            triangles[triIndex++] = 0;
            triangles[triIndex++] = i;
            triangles[triIndex++] = i + 1;
        }

        // Create triangles for the back face
        for (int i = 1; i < points.Count - 1; i++)
        {
            triangles[triIndex++] = points.Count;
            triangles[triIndex++] = points.Count + i + 1;
            triangles[triIndex++] = points.Count + i;
        }

        // Create triangles for the sides
        for (int i = 0; i < points.Count; i++)
        {
            int next = (i + 1) % points.Count;

            // First triangle
            triangles[triIndex++] = i;
            triangles[triIndex++] = next;
            triangles[triIndex++] = i + points.Count;

            // Second triangle
            triangles[triIndex++] = next;
            triangles[triIndex++] = next + points.Count;
            triangles[triIndex++] = i + points.Count;
        }

        // Assign the generated data to the mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uv;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }



    // Mesh GenerateExtrudedMesh(List<Vector3> points, float thickness)
    // {
    //     Mesh mesh = new Mesh();

    //     // Define vertices
    //     Vector3[] vertices = new Vector3[points.Count * 2];
    //     int[] triangles = new int[(points.Count - 2) * 6 + points.Count * 6];
    //     Vector3[] normals = new Vector3[vertices.Length];
    //     Vector2[] uv = new Vector2[vertices.Length];

    //     for (int i = 0; i < points.Count; i++)
    //     {
    //         vertices[i] = points[i];
    //         vertices[i + points.Count] = points[i] + Vector3.forward * thickness;
    //         normals[i] = -Vector3.forward;
    //         normals[i + points.Count] = Vector3.forward;
    //         uv[i] = new Vector2(vertices[i].x, vertices[i].y);
    //         uv[i + points.Count] = new Vector2(vertices[i + points.Count].x, vertices[i + points.Count].y);
    //     }

    //     // Create the triangles
    //     int triIndex = 0;
    //     for (int i = 0; i < points.Count - 2; i++)
    //     {
    //         triangles[triIndex++] = 0;
    //         triangles[triIndex++] = i + 1;
    //         triangles[triIndex++] = i + 2;

    //         triangles[triIndex++] = points.Count;
    //         triangles[triIndex++] = points.Count + i + 2;
    //         triangles[triIndex++] = points.Count + i + 1;
    //     }

    //     for (int i = 0; i < points.Count; i++)
    //     {
    //         int next = (i + 1) % points.Count;
    //         triangles[triIndex++] = i;
    //         triangles[triIndex++] = next;
    //         triangles[triIndex++] = i + points.Count;

    //         triangles[triIndex++] = next;
    //         triangles[triIndex++] = next + points.Count;
    //         triangles[triIndex++] = i + points.Count;
    //     }

    //     // Assign to mesh
    //     mesh.vertices = vertices;
    //     mesh.triangles = triangles;
    //     mesh.normals = normals;
    //     mesh.uv = uv;

    //     mesh.RecalculateNormals();
    //     mesh.RecalculateBounds();
        
    //     return mesh;
    // }
}