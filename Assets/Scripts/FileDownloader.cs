using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class FileDownloader : MonoBehaviour
{
    [SerializeField]
    private string fileListUrl = "http://172.27.90.203:8080/uploads/list";

    [SerializeField]
    private string sceneName = "AUTVIX";

    [SerializeField]
    private Vector3 modelPosition = Vector3.zero;

    void Start()
    {
        StartCoroutine(GetAvailableFileAndDownload());
    }

    // Obtém a lista de arquivos e baixa o primeiro arquivo disponível
    IEnumerator GetAvailableFileAndDownload()
    {
        string availableFileUrl = null;

        using (UnityWebRequest www = UnityWebRequest.Get(fileListUrl))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Erro ao obter a lista de arquivos: {www.error}");
                yield break;
            }

            string fileList = www.downloadHandler.text;
            string[] files = fileList.Split(',');

            if (files.Length > 0)
            {
                availableFileUrl = "http://172.27.90.203:8080/uploads/" + files[0].Trim();
            }
            else
            {
                Debug.LogError("Nenhum arquivo disponível para download.");
                yield break;
            }
        }

        if (!string.IsNullOrEmpty(availableFileUrl))
        {
            yield return DownloadAndLoadModel(availableFileUrl);
        }
    }

    // Baixa e carrega o modelo
    IEnumerator DownloadAndLoadModel(string url)
    {
        byte[] modelData = null;
        string fileName = Path.GetFileName(url);
        string filePath = Path.Combine(Application.dataPath, fileName);

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Erro ao baixar o arquivo: {www.error}");
                yield break;
            }

            modelData = www.downloadHandler.data;
        }

        if (modelData == null || modelData.Length == 0)
        {
            Debug.LogError("Falha ao baixar o arquivo ou arquivo vazio.");
            yield break;
        }

        SaveModelToFile(filePath, modelData);

        if (!ImportModelAsset(fileName))
        {
            Debug.LogError("Falha ao importar o modelo.");
            yield break;
        }

        yield return LoadSceneAndInstantiateModel(fileName);
    }

    // Salva o arquivo baixado localmente
    void SaveModelToFile(string filePath, byte[] data)
    {
        try
        {
            File.WriteAllBytes(filePath, data);
            Debug.Log($"Arquivo salvo em: {filePath}");
        }
        catch (IOException ex)
        {
            Debug.LogError($"Erro ao salvar o arquivo: {ex.Message}");
        }
    }

    // Importa o modelo para o Unity
    bool ImportModelAsset(string fileName)
    {
        string assetPath = "Assets/" + fileName;
        try
        {
            AssetDatabase.ImportAsset(assetPath);
            Debug.Log("Modelo importado.");
            return true;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Erro ao importar o modelo: {ex.Message}");
            return false;
        }
    }

    // Carrega a cena e instancia o modelo na cena
    IEnumerator LoadSceneAndInstantiateModel(string fileName)
    {
        string assetPath = "Assets/" + fileName;

        if (SceneManager.GetActiveScene().name != sceneName)
        {
            SceneManager.LoadScene(sceneName);
            yield return new WaitForSeconds(0.5f);
        }

        GameObject model = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

        if (model != null)
        {
            InstantiateModelInScene(model);
        }
        else
        {
            Debug.LogError("Falha ao carregar o modelo.");
        }
    }

    // Instancia o modelo na cena
    void InstantiateModelInScene(GameObject model)
    {
        try
        {
            Instantiate(model, modelPosition, Quaternion.identity);
            Debug.Log("Modelo instanciado na cena.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Erro ao instanciar o modelo: {ex.Message}");
        }
    }
}
