using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectSpawner : MonoBehaviour
{    
    // public GameObject[] objectPrefabs;     // Assign your .glb prefab here
    private GameObject spawnedObject;
    public TMP_Dropdown dropdown;
    private MoleculeInfo[] moleculeData;

    private Vector3 lastMousePos;   
    private float rotationSpeed = 15f;
    private float scaleSpeed = 0.01f;
    private float minScale = 0.001f;
    private float maxScale = 0.05f;   

    private bool autoRotate = true;
    private float rotationSpeedAuto = 10f;

    [SerializeField]
    public ObjectDescriptionManager objectDescriptionManager;

 void Start()
    {
        LoadMoleculeData();
        PopulateDropdown();
    }

    void LoadMoleculeData()
    {
        TextAsset jsonText = Resources.Load<TextAsset>("molecules_data");
        if (jsonText != null)
        {
            MoleculeInfoList list = JsonUtility.FromJson<MoleculeInfoList>("{\"molecules\":" + jsonText.text + "}");
            moleculeData = list.molecules;
        }
        else
        {
            Debug.LogError("Molecule JSON file not found in Resources");
        }
    }

    void PopulateDropdown()
    {
        dropdown.ClearOptions();
        List<string> options = new List<string>();

        foreach (var molecule in moleculeData)
        {
            options.Add(molecule.title);
        }

        dropdown.AddOptions(options);
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    void OnDropdownValueChanged(int index)
    {
        Despawn();
        string prefabName = moleculeData[index].prefabName;
        objectDescriptionManager.ShowObjectDescription(index);
        SpawnPrefabByName(prefabName);
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
    
    public void SpawnPrefabByName(string prefabName)
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/" + prefabName);
        if (prefab != null)
        {
            Vector3 spawnPosition = new Vector3(-0.2f, 1.5f, -0.1f); // Replace with any desired coordinates
            spawnedObject = Instantiate(prefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning($"Prefab '{prefabName}' not found in Resources/Molecules/");
        }
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
