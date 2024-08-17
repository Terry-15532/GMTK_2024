using UnityEngine;

public class RaycastChecker : MonoBehaviour
{
    public Transform startPoint; // The point from which to cast the rays
    public float maxRayDistance = 100f; // Maximum distance for raycasts

    private void Update()
    {
        if (CheckRaycasts())
        {
            TriggerFunction();
        }
    }

    private bool CheckRaycasts()
    {
        // Get the collider component
        Collider collider = GetComponent<Collider>();
        if (collider == null)
        {
            Debug.LogError("No collider found on this object.");
            return false;
        }

        // Get the vertices of the collider
        MeshCollider meshCollider = collider as MeshCollider;
        if (meshCollider == null)
        {
            Debug.LogError("This script requires a MeshCollider.");
            return false;
        }

        Mesh mesh = meshCollider.sharedMesh;
        if (mesh == null)
        {
            Debug.LogError("No mesh found in the MeshCollider.");
            return false;
        }

        Vector3[] vertices = mesh.vertices;
        Transform colliderTransform = meshCollider.transform;
        bool allRaysHitPlatform = true;

        foreach (Vector3 vertex in vertices)
        {
            Vector3 worldVertex = colliderTransform.TransformPoint(vertex);
            Ray ray = new Ray(startPoint.position, worldVertex - startPoint.position);
            RaycastHit[] hits;

            // Perform the raycast and get all hits
            hits = Physics.RaycastAll(ray, maxRayDistance);

            bool hitPlatform = false;
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.CompareTag("Platform"))
                {
                    // Ray hits a Platform object
                    hitPlatform = true;
                }
            }
            if (!hitPlatform)
            {
                allRaysHitPlatform = false;
                break;
            }
        }

        return allRaysHitPlatform;
    }

    private void TriggerFunction()
    {
        Debug.Log("All raycasts passed through a Platform object before hitting the collider vertices.");
        // Place your custom function logic here
    }
}
