using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class PointSelector : MonoBehaviour
{
    private Vector3? pointA = null;
    private Vector3? pointB = null;
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 2;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
    }

    void Update()
    {
        if (IsRightTriggerPressed())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (pointA == null)
                {
                    pointA = hit.point;
                    Debug.Log($"Point A selected at: {pointA}");
                    lineRenderer.SetPosition(0, pointA.Value);
                    lineRenderer.SetPosition(1, pointA.Value); // Inicialmente, a linha começa e termina no ponto A
                }
                else if (pointB == null)
                {
                    pointB = hit.point;
                    Debug.Log($"Point B selected at: {pointB}");
                    lineRenderer.SetPosition(1, pointB.Value);
                    float distance = CalculateDistance(pointA.Value, pointB.Value);
                    Debug.Log($"Distance between Point A and Point B: {distance} meters");
                    pointA = null;
                    pointB = null;
                }
            }
        }

        // Atualiza a linha e a distância enquanto o ponto B não é selecionado
        if (pointA.HasValue && !pointB.HasValue)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                lineRenderer.SetPosition(1, hit.point);
                float distance = CalculateDistance(pointA.Value, hit.point);
                Debug.Log($"Current distance: {distance} meters");
            }
        }
    }

    bool IsRightTriggerPressed()
    {
        bool triggerValue;
        InputDevice rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        rightHand.TryGetFeatureValue(CommonUsages.triggerButton, out triggerValue);
        return triggerValue;
    }

    float CalculateDistance(Vector3 point1, Vector3 point2)
    {
        return Vector3.Distance(point1, point2);
    }
}