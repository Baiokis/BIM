using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class XRObjectSelector : MonoBehaviour
{
    public XRRayInteractor rightRayInteractor;
    public InputActionProperty selectAction;
    public LayerMask interactableLayer;

    void Update()
    {
        if (rightRayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            if (selectAction.action.triggered)
            {
                GameObject selectedObject = hit.transform.gameObject;
                Debug.Log("Selected object: " + selectedObject.name);

                // Tamanho do Objeto
                Vector3 size = selectedObject.GetComponent<Renderer>().bounds.size;
                Debug.Log("Size: " + size);

                // Material do Objeto
                Material material = selectedObject.GetComponent<Renderer>().material;
                Debug.Log("Material: " + material.name);

                // Cor do Material
                Color color = material.color;
                Debug.Log("Color: " + color);

                // Modelo do Objeto (se aplicável)
                MeshFilter meshFilter = selectedObject.GetComponent<MeshFilter>();
                if (meshFilter != null)
                {
                    Mesh mesh = meshFilter.mesh;
                    Debug.Log("Model: " + mesh.name);
                }

                // Outras propriedades adicionais podem ser adicionadas aqui
            }
        }
    }
}
