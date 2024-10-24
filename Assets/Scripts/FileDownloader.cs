using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class FileDownloader : MonoBehaviour
{
    private string fileNameUrl = "http://172.27.90.203:8080/get-latest-file";  
    private string baseUrl = "http://172.27.90.203:8080/uploads/";  
    public string fileName;  

    [System.Serializable]
    public class FileResponse
    {
        public string filename;
    }

    // Starts the process to get the filename and download the file
    void Start()
    {
        StartCoroutine(GetFileNameAndDownload());
    }

    // Gets the file name from a remote server and initiates the download process
    IEnumerator GetFileNameAndDownload()
    {
        UnityWebRequest request = UnityWebRequest.Get(fileNameUrl);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Erro ao obter o nome do arquivo: " + request.error);
            yield break;
        }

        string jsonResponse = request.downloadHandler.text;
        FileResponse fileResponse = JsonUtility.FromJson<FileResponse>(jsonResponse);

        if (fileResponse != null && !string.IsNullOrEmpty(fileResponse.filename))
        {
            fileName = fileResponse.filename;
            StartCoroutine(DownloadAndLoadModel(fileName));
        }
        else
        {
            Debug.LogError("Nome do arquivo não obtido corretamente.");
        }
    }

    // Downloads the model file and loads it into the scene
    IEnumerator DownloadAndLoadModel(string modelFileName)
    {
        string url = baseUrl + modelFileName;
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Erro ao baixar o arquivo: " + www.error);
            yield break;
        }

        string filePath = Path.Combine(Application.dataPath, modelFileName);
        File.WriteAllBytes(filePath, www.downloadHandler.data);

        AssetDatabase.ImportAsset("Assets/" + modelFileName);
        GameObject model = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/" + modelFileName);

        if (model != null)
        {
            if (SceneManager.GetActiveScene().name != "Autvix")
            {
                SceneManager.LoadScene("AUTVIX");  
                yield return null;  
            }

            Instantiate(model, Vector3.zero, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Falha ao carregar o modelo.");
        }
    }
}
