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
            menu.SetActive(!menu.activeSelf);

            if (menu.activeSelf)
            {
                // Calcula a nova posição do menu em relação à cabeça
                Vector3 spawnPosition = head.position + new Vector3(head.forward.x, 0, head.forward.z).normalized * spawnDistance;
                menu.transform.position = spawnPosition;
            }
        }

        //Verificação para manter o menu rodando 
        if (menu.activeSelf)
        {
            Vector3 lookDirection = (head.position - menu.transform.position).normalized;
            lookDirection.y = 0;
            menu.transform.rotation = Quaternion.LookRotation(lookDirection);

            menu.transform.forward *= -1;
        }
    }
}