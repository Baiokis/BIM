using UnityEngine;

public class MeshColliderAdder : MonoBehaviour
{
    // Método público para adicionar MeshColliders a todos os objetos na cena
    public void AddMeshColliders()
    {
        foreach (MeshFilter meshFilter in FindObjectsOfType<MeshFilter>())
        {
            GameObject obj = meshFilter.gameObject;
            if (obj.GetComponent<MeshCollider>() == null)
            {
                obj.AddComponent<MeshCollider>();
                Debug.Log($"MeshCollider adicionado com sucesso ao GameObject: {obj.name}");
            }
            else
            {
                Debug.Log($"MeshCollider já existe no GameObject: {obj.name}");
            }
        }
    }

    // Método público para remover MeshColliders de todos os objetos na cena
    public void RemoveMeshColliders()
    {
        foreach (MeshCollider meshCollider in FindObjectsOfType<MeshCollider>())
        {
            Destroy(meshCollider);
            Debug.Log($"MeshCollider removido do GameObject: {meshCollider.gameObject.name}");
        }
    }

    // Método para ser chamado pelo Toggle
    public void ToggleMeshColliders(bool state)
    {
        if (state)
        {
            AddMeshColliders();
        }
        else
        {
            RemoveMeshColliders();
        }
    }
}
