using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class FileDownloader : MonoBehaviour
{
    private string fileNameUrl = "http://172.27.90.203:8080/get-latest-file";  // URL para obter o nome do arquivo mais recente
    private string baseUrl = "http://172.27.90.203:8080/uploads/";  // URL base para o download
    public string fileName;  // Variável para o nome do arquivo (inicialmente vazia)


    [System.Serializable]
    public class FileResponse
    {
        public string filename;
    }

    void Start()
    {
        StartCoroutine(GetFileNameAndDownload());
    }

    IEnumerator GetFileNameAndDownload()
    {
        // Faz uma requisição GET para obter o nome do arquivo
        UnityWebRequest request = UnityWebRequest.Get(fileNameUrl);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Erro ao obter o nome do arquivo: " + request.error);
            yield break;
        }

        // Usa JsonUtility para parsear a resposta JSON
        string jsonResponse = request.downloadHandler.text;
        FileResponse fileResponse = JsonUtility.FromJson<FileResponse>(jsonResponse);

        if (fileResponse != null && !string.IsNullOrEmpty(fileResponse.filename))
        {
            fileName = fileResponse.filename;
            Debug.Log("Nome do arquivo obtido: " + fileName);

            // Inicia o download do modelo
            StartCoroutine(DownloadAndLoadModel(fileName));
        }
        else
        {
            Debug.LogError("Nome do arquivo não obtido corretamente.");
        }
    }

    IEnumerator DownloadAndLoadModel(string modelFileName)
    {
        string url = baseUrl + modelFileName;  // Concatena a URL base com o nome do arquivo
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Erro ao baixar o arquivo: " + www.error);
            yield break;
        }

        string filePath = Path.Combine(Application.dataPath, modelFileName);
        File.WriteAllBytes(filePath, www.downloadHandler.data);
        Debug.Log("Arquivo salvo em: " + filePath);

        AssetDatabase.ImportAsset("Assets/" + modelFileName);
        GameObject model = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/" + modelFileName);

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
