using UnityEngine;
using UnityEngine.XR;

public class MeasurementTool : MonoBehaviour
{
    public Transform leftHand;          // Refer�ncia � m�o esquerda
    public Transform rightHand;         // Refer�ncia � m�o direita, se necess�rio
    public LineRenderer lineRenderer;   // Refer�ncia ao LineRenderer

    private bool isMeasuring = false;   // Controle de estado da medi��o
    private Vector3 startPoint;         // Ponto inicial da medi��o
    private Vector3 endPoint;           // Ponto final da medi��o

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            if (!isMeasuring)
            {
                // Iniciar medi��o
                startPoint = leftHand.position;
                isMeasuring = true;
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, startPoint);
            }
            else
            {
                // Finalizar medi��o
                endPoint = leftHand.position;
                isMeasuring = false;
                lineRenderer.SetPosition(1, endPoint);

                float distance = Vector3.Distance(startPoint, endPoint);
                Debug.Log("Distance: " + distance + " meters");
            }
        }

        if (isMeasuring)
        {
            // Atualizar linha de medi��o em tempo real
            lineRenderer.SetPosition(1, leftHand.position);
        }
    }
}
