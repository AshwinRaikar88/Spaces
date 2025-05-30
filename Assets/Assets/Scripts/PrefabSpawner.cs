using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public string prefabName; // Assign this via dropdown, input field, etc.

    public void SpawnPrefabByName(string prefabName)
    {
        GameObject prefab = Resources.Load<GameObject>("Molecules/" + prefabName);
        if (prefab != null)
        {
            Instantiate(prefab, new Vector3(0, 1, 0), Quaternion.identity);
        }
        else
        {
            Debug.LogWarning($"Prefab '{prefabName}' not found in Resources/Molecules/");
        }
    }
}
