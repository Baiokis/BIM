using UnityEngine;
using UnityEngine.XR;

public class MeasurementTool : MonoBehaviour
{
    public Transform leftHand;          // Referência à mão esquerda
    public Transform rightHand;         // Referência à mão direita, se necessário
    public LineRenderer lineRenderer;   // Referência ao LineRenderer

    private bool isMeasuring = false;   // Controle de estado da medição
    private Vector3 startPoint;         // Ponto inicial da medição
    private Vector3 endPoint;           // Ponto final da medição

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            if (!isMeasuring)
            {
                // Iniciar medição
                startPoint = leftHand.position;
                isMeasuring = true;
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, startPoint);
            }
            else
            {
                // Finalizar medição
                endPoint = leftHand.position;
                isMeasuring = false;
                lineRenderer.SetPosition(1, endPoint);

                float distance = Vector3.Distance(startPoint, endPoint);
                Debug.Log("Distance: " + distance + " meters");
            }
        }

        if (isMeasuring)
        {
            // Atualizar linha de medição em tempo real
            lineRenderer.SetPosition(1, leftHand.position);
        }
    }
}
