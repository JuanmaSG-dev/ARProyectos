using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class AssetBundleManager : MonoBehaviour
{
    private string prefabBundleUrl = "https://drive.google.com/uc?export=download&id=13KunngZQNAdvPn08VkpuiyMHdej-XqSi";
    private string prefabsBundleName = "planetbundle";

    // Diccionario para mapear nombres a objetos cargados
    private Dictionary<string, GameObject> loadedPrefabs = new Dictionary<string, GameObject>();

    void Start()
    {
        StartCoroutine(FetchGameObjectFromServer(prefabBundleUrl, prefabsBundleName, 0, new Hash128()));
    }

    IEnumerator FetchGameObjectFromServer(string url, string manifestFileName, uint crcR, Hash128 hashR)
    {
        uint crcNumber = crcR;
        Hash128 hashCode = hashR;

        // Descargar el AssetBundle
        UnityWebRequest webRequest =
            UnityWebRequestAssetBundle.GetAssetBundle(url, new CachedAssetBundle(manifestFileName, hashCode), crcNumber);

        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Error descargando AssetBundle: {webRequest.error}");
            yield break;
        }

        AssetBundle assetBundle = DownloadHandlerAssetBundle.GetContent(webRequest);
        if (assetBundle == null)
        {
            Debug.LogError("No se pudo cargar el AssetBundle.");
            yield break;
        }

        // Registrar todos los prefabs cargados en el diccionario
        string[] allAssetNames = assetBundle.GetAllAssetNames();
        Debug.Log($"Nombres en el AssetBundle: {string.Join(", ", allAssetNames)}");

        foreach (string assetName in allAssetNames)
        {
            string prefabName = assetName; // Usar nombre completo
            GameObject prefab = assetBundle.LoadAsset<GameObject>(assetName);
            if (prefab != null)
            {
                loadedPrefabs[prefabName] = prefab;
                Debug.Log($"Prefab {prefabName} cargado y registrado.");
            }
        }

        // Comentamos temporalmente la descarga del AssetBundle
        // assetBundle.Unload(false);
    }

    // Método para activar un prefab por su nombre
    public void ActivatePrefab(string prefabName)
    {
        if (loadedPrefabs.ContainsKey(prefabName))
        {
            GameObject instance = Instantiate(loadedPrefabs[prefabName]);
            instance.SetActive(true); // Activarlo si es necesario
            Debug.Log($"Instancia de {prefabName} creada y activada.");
        }
        else
        {
            Debug.LogError($"Prefab {prefabName} no está cargado.");
        }
    }
}
