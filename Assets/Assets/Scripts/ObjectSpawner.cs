using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.InputSystem;
// using UnityEngine.UIElements;

public class ObjectSpawner : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField]
    private Slider scaleSlider;
    [SerializeField]
    private ObjectDescriptionManager objectDescriptionManager;
    [SerializeField]
    private TMP_Dropdown dropdown;

    private GameObject spawnedObject;
    private MoleculeInfo[] moleculeData;

    private InputSystem_Actions inputActions;
    private InputAction pointerDeltaAction;
    private InputAction scrollAction;
    private InputAction rightClickAction;
    private InputAction leftClickAction;
    
    [Header("Scaling Settings")]
    [SerializeField]
    private int rotationSpeed = 500;
    [SerializeField]
    private int rotationSpeedAuto = 10;
    [SerializeField]
    private int scaleDivisions = 100;    
    [SerializeField]
    private float minScale = 0.001f;
    [SerializeField]
    private float maxScale = 0.05f;       
    private float scaleStep;
    private bool autoRotate = true;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Enable();

        pointerDeltaAction = inputActions.UI.PointerDelta;
        scrollAction = inputActions.UI.ScrollWheel;
        rightClickAction = inputActions.UI.RightClick;
        leftClickAction = inputActions.UI.Click;
        
        scaleStep = (maxScale - minScale) / scaleDivisions;
        if (scaleSlider != null)
        {
            scaleSlider.minValue = minScale;
            scaleSlider.maxValue = maxScale;
            scaleSlider.value = 0.001f;
        }
    }

    void Start()
    {
        LoadMoleculeData();
        PopulateDropdown();
        if (scaleSlider != null)
            scaleSlider.onValueChanged.AddListener(OnSliderChanged);
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

    public void OnSliderChanged(float sliderValue)
    {
        if (spawnedObject == null) return;

        XRGrabInteractable interactable = spawnedObject.GetComponent<XRGrabInteractable>();
        if (interactable == null) return;

        Vector3 newScale = Vector3.one * sliderValue;
        interactable.SetTargetLocalScale(newScale);               
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

    public void ScaleUp()
    {
        if (spawnedObject != null)
        {
            XRGrabInteractable interactable = spawnedObject.GetComponent<XRGrabInteractable>();
            Vector3 scale = interactable.GetTargetLocalScale();
            scale += Vector3.one * scaleStep;
            scale = Vector3.Min(scale, Vector3.one * maxScale);
            interactable.SetTargetLocalScale(scale);

            if (scaleSlider != null)
            {
                scaleSlider.value = scale.x;
            }
        }    
    }

    public void ScaleDown()
    {
        if (spawnedObject != null)
        {
            XRGrabInteractable interactable = spawnedObject.GetComponent<XRGrabInteractable>();
            Vector3 scale = interactable.GetTargetLocalScale();
            scale -= Vector3.one * scaleStep;
            scale = Vector3.Max(scale, Vector3.one * minScale);
            interactable.SetTargetLocalScale(scale);
            
            if (scaleSlider != null)
            {
                scaleSlider.value = scale.x;
            }
        }    
    }

    void HandleRotation()
    {   
        if (spawnedObject == null) return;

        if (leftClickAction.IsPressed())
        {
            autoRotate = true;
        }
        else if (rightClickAction.IsPressed())
        {
            autoRotate = false;
            Vector2 delta = pointerDeltaAction.ReadValue<Vector2>();

            float rotX = delta.y * rotationSpeed * Time.deltaTime;
            float rotY = -delta.x * rotationSpeed * Time.deltaTime;

            spawnedObject.transform.Rotate(Vector3.right, rotX, Space.World);
            spawnedObject.transform.Rotate(Vector3.up, rotY, Space.World);
        }

        if (autoRotate)
        {
            spawnedObject.transform.Rotate(Vector3.up, rotationSpeedAuto * Time.deltaTime);
        }            
    }

    void HandleScaling()
    {
        if (spawnedObject == null) return;

        Vector2 scrollDelta = scrollAction.ReadValue<Vector2>();
        float scroll = scrollDelta.y;

        if (Mathf.Abs(scroll) > 0.01f)
        {
            Vector3 scale = spawnedObject.transform.localScale;
            scale += Vector3.one * scroll * scaleStep;
            scale = Vector3.Max(scale, Vector3.one * minScale);
            scale = Vector3.Min(scale, Vector3.one * maxScale);
            spawnedObject.transform.localScale = scale;

            if (scaleSlider != null)
            {
                scaleSlider.value = scale.x;
            }
        }
    }
}
