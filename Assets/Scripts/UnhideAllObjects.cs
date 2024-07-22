using UnityEngine;

public class UnhideAllObjects : MonoBehaviour
{
    public void UnhideAll()
    {
        // Obt�m todos os objetos na cena
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        // Percorre todos os objetos e define como ativo, ignorando objetos inativos por padr�o
        foreach (GameObject obj in allObjects)
        {
            // Ignora objetos inativos na cena (ativos apenas no editor)
            if (obj.hideFlags == HideFlags.None && obj.activeInHierarchy == false)
            {
                obj.SetActive(true);
            }
        }
    }
}
