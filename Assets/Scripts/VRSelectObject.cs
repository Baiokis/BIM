using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class VRSelectObject : MonoBehaviour
{
    public XRController leftController;
    public XRController rightController;

    private XRRayInteractor leftRayInteractor;
    private XRRayInteractor rightRayInteractor;

    private void Start()
    {
        leftRayInteractor = leftController.GetComponent<XRRayInteractor>();
        rightRayInteractor = rightController.GetComponent<XRRayInteractor>();
    }

    private void Update()
    {
        CheckForSelection(leftController, leftRayInteractor);
        CheckForSelection(rightController, rightRayInteractor);
    }

    private void CheckForSelection(XRController controller, XRRayInteractor rayInteractor)
    {
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            if (hit.collider != null)
            {
                if (controller.inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool isTriggerPressed) && isTriggerPressed)
                {
                    SelectObject(hit.collider.gameObject);
                }
            }
        }
    }

    private void SelectObject(GameObject obj)
    {
        // Aqui você pode adicionar o comportamento de seleção, como alterar a cor ou exibir um highlight
        Debug.Log($"Objeto {obj.name} selecionado.");
    }
}
