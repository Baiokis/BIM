using UnityEngine;

public class DistanceMeasurement : MonoBehaviour
{
    public GameObject pointPrefab; // Prefab para o ponto
    private GameObject pointA;
    private GameObject pointB;
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger)) // Para controladores Oculus, pode ajustar conforme seu SDK
        {
            if (pointA == null)
            {
                pointA = Instantiate(pointPrefab, OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch), Quaternion.identity);
            }
            else if (pointB == null)
            {
                pointB = Instantiate(pointPrefab, OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch), Quaternion.identity);
                DrawLine();
                MeasureDistance();
            }
            else
            {
                Destroy(pointA);
                Destroy(pointB);
                pointA = Instantiate(pointPrefab, OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch), Quaternion.identity);
                pointB = null;
            }
        }
    }

    void DrawLine()
    {
        lineRenderer.SetPosition(0, pointA.transform.position);
        lineRenderer.SetPosition(1, pointB.transform.position);
    }

    void MeasureDistance()
    {
        float distance = Vector3.Distance(pointA.transform.position, pointB.transform.position);
        Debug.Log("Distância: " + distance);
    }
}
