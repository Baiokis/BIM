using UnityEngine;

public class Manager : MonoBehaviour
{
    public Transform head; // Refer�ncia � cabe�a do jogador
    public float spawnDistance = 0.2f; // Dist�ncia de spawn do menu em rela��o � cabe�a
    public GameObject menu; // Refer�ncia ao menu

    void Start()
    {
        // Inicialmente, ativa o menu
        menu.SetActive(true);
    }

    void Update()
    {
        // Calcula a nova posi��o do menu � esquerda da cabe�a
        Vector3 spawnPosition = head.position + (Quaternion.Euler(0, -135, 0) * head.right).normalized * spawnDistance;
        spawnPosition.y = head.position.y; // Mant�m a mesma altura da cabe�a
        menu.transform.position = spawnPosition;

        // Define a rota��o do menu para ser fixa (n�o olha para a cabe�a)
        menu.transform.rotation = Quaternion.Euler(0, head.rotation.eulerAngles.y, 0);
    }
}
