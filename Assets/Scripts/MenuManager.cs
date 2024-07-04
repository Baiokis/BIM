using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    public Transform head;
    public float spawnDistance = 2;
    public GameObject menu;
    public InputActionProperty showButton;

    void Start()
    {
        // Inicialmente, desativa o menu
        menu.SetActive(false);
    }

    void Update()
    {
        if (showButton.action.WasPressedThisFrame())
        {
            // Alterna o estado ativo do menu
            menu.SetActive(!menu.activeSelf);

            // Calcula a nova posi��o do menu em rela��o � cabe�a
            Vector3 spawnPosition = head.position + new Vector3(head.forward.x, 0, head.forward.z).normalized * spawnDistance;
            menu.transform.position = spawnPosition;

            // Faz o menu olhar para a cabe�a
            Vector3 lookDirection = (head.position - menu.transform.position).normalized;
            lookDirection.y = 0; // Mant�m a rota��o no eixo Y
            menu.transform.rotation = Quaternion.LookRotation(lookDirection);

            menu.transform.forward *= -1;
        }
    }
}
