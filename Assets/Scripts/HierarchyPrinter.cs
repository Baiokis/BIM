using UnityEngine;

public class HierarchyPrinter : MonoBehaviour
{
    void Start()
    {
        PrintHierarchy(transform, "");
    }

    void PrintHierarchy(Transform parent, string indent)
    {
        Debug.Log(indent + parent.name);

        for (int i = 0; i < parent.childCount; i++)
        {
            PrintHierarchy(parent.GetChild(i), indent + "--");
        }
    }
}
