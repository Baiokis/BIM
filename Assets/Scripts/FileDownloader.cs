using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class FileDownloader : MonoBehaviour
{
    private string uploadUrl = "http://172.27.90.203:8080/upload";  // URL para upload ou onde pega o nome
    private string baseUrl = "http://172.27.90.203:8080/uploads/";  // URL base para o download
    public string fileName;  // Variável para o nome do arquivo (inicialmente vazia)

    void Start()
    {
        StartCoroutine(GetFileNameAndDownload());
    }

    IEnumerator GetFileNameAndDownload()
    {
        // Faz uma requisição para obter o nome do arquivo
        UnityWebRequest request = UnityWebRequest.Get(uploadUrl);  // Supondo que o nome é obtido de um GET

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Erro ao obter o nome do arquivo: " + request.error);
            yield break;
        }

        // Supondo que o nome do arquivo está retornado em um campo "filename" no JSON
        string jsonResponse = request.downloadHandler.text;
        SimpleJSON.JSONNode jsonResponseParsed = SimpleJSON.JSON.Parse(jsonResponse);
        fileName = jsonResponseParsed["filename"];
        Debug.Log("Nome do arquivo obtido: " + fileName);

        if (!string.IsNullOrEmpty(fileName))
        {
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
