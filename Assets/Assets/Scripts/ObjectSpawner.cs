using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectPrefab;     // Assign your .glb prefab here
    private GameObject spawnedObject;

    private Vector3 lastMousePos;
    private float rotationSpeed = 15f;
    private float scaleSpeed = 0.01f;
    private float minScale = 0.001f;
    private float maxScale = 0.01f;

    void Update()
    {
        HandleSpawn();
        HandleRotation();
        HandleScaling();
    }

    void HandleSpawn()
{
    if (Input.GetMouseButtonDown(0)) // Left click
    {
        Vector3 spawnPosition = new Vector3(-0.2f, 1f, -0.1f); // Replace with any desired coordinates

        if (spawnedObject == null)
        {
            spawnedObject = Instantiate(objectPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            spawnedObject.transform.position = spawnPosition;
        }
    }
}

    // void HandleSpawn()
    // {
    //     if (Input.GetMouseButtonDown(0)) // Left click
    //     {
    //         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //         if (Physics.Raycast(ray, out RaycastHit hit))
    //         {
    //             if (spawnedObject == null)
    //             {
    //                 spawnedObject = Instantiate(objectPrefab, hit.point, Quaternion.identity);
    //             }
    //             else
    //             {
    //                 spawnedObject.transform.position = hit.point;
    //             }
    //         }
    //     }
    // }

    void HandleRotation()
    {
        if (spawnedObject != null && Input.GetMouseButton(1)) // Right mouse drag
        {
            Vector3 delta = Input.mousePosition - lastMousePos;
            float rotX = delta.y * rotationSpeed * Time.deltaTime;
            float rotY = -delta.x * rotationSpeed * Time.deltaTime;
            spawnedObject.transform.Rotate(Vector3.right, rotX, Space.World);
            spawnedObject.transform.Rotate(Vector3.up, rotY, Space.World);
        }
        lastMousePos = Input.mousePosition;
    }

    void HandleScaling()
    {
        if (spawnedObject != null)
        {
            float scroll = Input.mouseScrollDelta.y;
            if (Mathf.Abs(scroll) > 0.01f)
            {
                Vector3 scale = spawnedObject.transform.localScale;
                scale += Vector3.one * scroll * scaleSpeed;
                scale = Vector3.Max(scale, Vector3.one * minScale);
                scale = Vector3.Min(scale, Vector3.one * maxScale);
                spawnedObject.transform.localScale = scale;
            }
        }
    }
}
