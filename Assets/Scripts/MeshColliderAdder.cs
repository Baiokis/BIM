using UnityEngine;

public class MeshColliderAdder : MonoBehaviour
{
    // M�todo p�blico para adicionar MeshColliders a todos os objetos na cena
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
                Debug.Log($"MeshCollider j� existe no GameObject: {obj.name}");
            }
        }
    }

    // M�todo p�blico para remover MeshColliders de todos os objetos na cena
    public void RemoveMeshColliders()
    {
        foreach (MeshCollider meshCollider in FindObjectsOfType<MeshCollider>())
        {
            Destroy(meshCollider);
            Debug.Log($"MeshCollider removido do GameObject: {meshCollider.gameObject.name}");
        }
    }

    // M�todo para ser chamado pelo Toggle
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
