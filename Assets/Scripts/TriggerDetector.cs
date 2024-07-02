using UnityEngine;

public class TriggerDetection : MonoBehaviour
{
    void Update()
    {
        // Verificar se o gatilho esquerdo foi pressionado
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            Debug.Log("Gatilho esquerdo pressionado!");
        }

        // Verificar se o gatilho direito foi pressionado
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            Debug.Log("Gatilho direito pressionado!");
        }
    }
}
