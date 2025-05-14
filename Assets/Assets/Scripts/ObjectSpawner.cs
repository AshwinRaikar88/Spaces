using UnityEngine;
using UnityEngine.UI;

public class ObjectSpawner : MonoBehaviour
{    
    public GameObject[] objectPrefabs;     // Assign your .glb prefab here
    private GameObject spawnedObject;

    private Vector3 lastMousePos;
    private float rotationSpeed = 15f;
    private float scaleSpeed = 0.01f;
    private float minScale = 0.001f;
    private float maxScale = 0.05f;   

    private bool autoRotate = true;
    private float rotationSpeedAuto = 10f;

    [SerializeField]
    private ObjectDescriptionManager objectDescriptionManager;

    public void DropdownIndex(int index)
    {   
        Despawn();

        if (index >= 0 && index <= objectPrefabs.Length)
        {            
            
            objectDescriptionManager.ShowObjectDescription(index);
                                
            HandleSpawn(index);        
        }        
    }

    void Update()
    {        
        HandleRotation();
        HandleScaling();
    }

    public void Despawn()
    {
       if (spawnedObject != null)
        {
            Destroy(spawnedObject);
        }
    }
    private void HandleSpawn(int index)
    {
    // if (Input.GetMouseButtonDown(0)) // Left click
    // {
        Vector3 spawnPosition = new Vector3(-0.2f, 1.5f, -0.1f); // Replace with any desired coordinates
            spawnedObject = Instantiate(objectPrefabs[index], spawnPosition, Quaternion.identity);
        
        // else
        // {
        //     spawnedObject.transform.position = spawnPosition;
        // }
    // }
    }  

    void HandleRotation()
    {
        if (spawnedObject != null && Input.GetMouseButton(0))
        {                
            autoRotate = true;
        }
        else{
            if (spawnedObject != null && Input.GetMouseButton(1)) // Right mouse drag
            {
                autoRotate = false;
                Vector3 delta = Input.mousePosition - lastMousePos;
                float rotX = delta.y * rotationSpeed * Time.deltaTime;
                float rotY = -delta.x * rotationSpeed * Time.deltaTime;
                spawnedObject.transform.Rotate(Vector3.right, rotX, Space.World);
                spawnedObject.transform.Rotate(Vector3.up, rotY, Space.World);
            }
            lastMousePos = Input.mousePosition;
        }
            
        if (autoRotate && spawnedObject != null)
        {
            spawnedObject.transform.Rotate(Vector3.up, rotationSpeedAuto * Time.deltaTime);
        }
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
