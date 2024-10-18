using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;  // Necessário para manipular cenas

public class FileDownloader : MonoBehaviour
{
    private string url = "http://172.27.90.203:8080/uploads/canteiro.fbx";

    void Start()
    {
        StartCoroutine(DownloadAndLoadModel());
    }

    IEnumerator DownloadAndLoadModel()
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Erro ao baixar o arquivo: " + www.error);
            yield break;
        }

        string fileName = Path.GetFileName(url);
        string filePath = Path.Combine(Application.dataPath, fileName);
        File.WriteAllBytes(filePath, www.downloadHandler.data);
        Debug.Log("Arquivo salvo em: " + filePath);

        AssetDatabase.ImportAsset("Assets/" + fileName);
        GameObject model = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/" + fileName);

        if (model != null)
        {
            // Verifica se a cena AUTVIX está carregada
            if (SceneManager.GetActiveScene().name != "Autvix")
            {
                SceneManager.LoadScene("AUTVIX");  // Carrega a cena AUTVIX se ela ainda não estiver ativa
                yield return null;  // Espera até que a cena seja carregada
            }

            // Instancia o modelo na cena AUTVIX
            Instantiate(model, Vector3.zero, Quaternion.identity);
            Debug.Log("Modelo carregado e instanciado na cena AUTVIX.");
        }
        else
        {
            Debug.LogError("Falha ao carregar o modelo.");
        }
    }
}
