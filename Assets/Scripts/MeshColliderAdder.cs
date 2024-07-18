using UnityEngine;

public class MeshColliderAdder : MonoBehaviour
{
    void Start()
    {
    }
    void Update()
    {
        AddMeshColliderToHierarchy(transform);
    }

    void AddMeshColliderToHierarchy(Transform parent)
    {
        // Tenta adicionar um MeshCollider ao GameObject atual
        if (parent.gameObject.GetComponent<MeshFilter>() != null)
        {
            if (parent.gameObject.GetComponent<MeshCollider>() == null)
            {
                parent.gameObject.AddComponent<MeshCollider>();
                Debug.Log($"MeshCollider adicionado com sucesso ao GameObject: {parent.name}");
            }
            else
            {
                Debug.Log($"MeshCollider já existe no GameObject: {parent.name}");
            }
        }
        else
        {
            Debug.Log($"MeshFilter não encontrado no GameObject: {parent.name}. MeshCollider não adicionado.");
        }

        for (int i = 0; i < parent.childCount; i++)
        {
            AddMeshColliderToHierarchy(parent.GetChild(i));
        }
    }
}
