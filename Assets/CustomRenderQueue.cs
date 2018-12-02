using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteInEditMode]
public class CustomRenderQueue : MonoBehaviour
{

    public UnityEngine.Rendering.CompareFunction comparison = UnityEngine.Rendering.CompareFunction.Always;

    public bool apply = false;

    private void Update()
    {
        if (apply)
        {
            apply = false;
            Debug.Log("Updated ObjectName material val");
            TextMeshProUGUI image = transform.Find("ObjectName").GetComponent<TextMeshProUGUI>();
            Material existingGlobalMat = image.materialForRendering;
            Material updatedMaterial = new Material(existingGlobalMat);
            updatedMaterial.SetInt("unity_GUIZTestMode", (int)comparison);
            image.material = updatedMaterial;
        }
    }
}