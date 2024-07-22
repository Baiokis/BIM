using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using TMPro; // Importar TextMeshPro
using UnityEngine.UI; // Para o Toggle

public class XRObjectSelector : MonoBehaviour
{
    public XRRayInteractor rightRayInteractor;
    public InputActionProperty selectAction;
    public LayerMask interactableLayer;
    public TextMeshProUGUI nameObject;
    public TextMeshProUGUI materialObject;
    public TextMeshProUGUI sizeObject;
    public Toggle toggle; // Referência ao Toggle do menu

    private GameObject currentSelectedObject;

    void Start()
    {
        if (toggle != null)
        {
            // Adiciona um listener ao evento OnValueChanged do Toggle
            toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }
    }

    void Update()
    {
        if (rightRayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            if (selectAction.action.triggered)
            {
                GameObject selectedObject = hit.transform.gameObject;
                currentSelectedObject = selectedObject; // Armazena o objeto selecionado
                Debug.Log("Selected object: " + selectedObject.name);

                // Atualiza o nome do objeto
                if (nameObject != null)
                {
                    nameObject.text = "<b>" + selectedObject.name + "</b>";
                }

                // Obtém o tamanho do objeto e formata com duas casas decimais
                Vector3 size = selectedObject.GetComponent<Renderer>().bounds.size;
                Debug.Log("Size: " + size);
                if (sizeObject != null)
                {
                    sizeObject.text = string.Format("{0:F2}m x {1:F2}m x {2:F2}m", size.x, size.y, size.z);
                }

                // Obtém o material do objeto
                Material material = selectedObject.GetComponent<Renderer>().material;
                Debug.Log("Material: " + material.name);
                if (materialObject != null)
                {
                    materialObject.text = material.name;
                }
            }
        }
    }

    private void OnToggleValueChanged(bool isOn)
    {
        // Verifica se há um objeto selecionado atualmente
        if (currentSelectedObject != null)
        {
            // Define o estado ativo do objeto com base no estado do Toggle
            currentSelectedObject.SetActive(!isOn);
        }
    }

    private void OnDestroy()
    {
        if (toggle != null)
        {
            // Remove o listener quando o objeto for destruído
            toggle.onValueChanged.RemoveListener(OnToggleValueChanged);
        }
    }
}
