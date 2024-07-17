using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class HideObjectOnTrigger : MonoBehaviour
{
    public XRController controller; // O controlador XR
    public XRRayInteractor rayInteractor; // O interator do raio

    private void Update()
    {
        if (controller != null && rayInteractor != null)
        {
            if (controller.inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool isPressed) && isPressed)
            {
                if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
                {
                    GameObject hitObject = hit.collider.gameObject;
                    hitObject.SetActive(false);
                }
            }
        }
    }
}
