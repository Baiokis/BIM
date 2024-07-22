using UnityEngine;

public class Manager : MonoBehaviour
{
    public Transform head; // Referência à cabeça do jogador
    public float spawnDistance = 0.2f; // Distância de spawn do menu em relação à cabeça
    public GameObject menu; // Referência ao menu

    void Start()
    {
        // Inicialmente, ativa o menu
        menu.SetActive(true);
    }

    void Update()
    {
        // Calcula a nova posição do menu à esquerda da cabeça
        Vector3 spawnPosition = head.position + (Quaternion.Euler(0, -135, 0) * head.right).normalized * spawnDistance;
        spawnPosition.y = head.position.y; // Mantém a mesma altura da cabeça
        menu.transform.position = spawnPosition;

        // Define a rotação do menu para ser fixa (não olha para a cabeça)
        menu.transform.rotation = Quaternion.Euler(0, head.rotation.eulerAngles.y, 0);
    }
}
