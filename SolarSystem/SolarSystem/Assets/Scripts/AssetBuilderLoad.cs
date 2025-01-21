using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class AssetBuilderLoad : MonoBehaviour
{
    private string prefabBundleUrl = "https://drive.google.com/uc?export=download&id=13KunngZQNAdvPn08VkpuiyMHdej-XqSi";
    private string prefabsBundleName = "planetbundle";

    // Diccionario para almacenar los prefabs instanciados desactivados
    private Dictionary<string, GameObject> loadedPrefabs = new Dictionary<string, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FetchGameObjectFromServer(prefabBundleUrl, prefabsBundleName, 0, new Hash128()));
    }

    IEnumerator FetchGameObjectFromServer(string url, string manifestFileName, uint crcR, Hash128 hashR)
    {
        uint crcNumber = crcR;
        Hash128 hashCode = hashR;

        Debug.Log("Iniciando descarga del AssetBundle...");
        UnityWebRequest webrequest =
            UnityWebRequestAssetBundle.GetAssetBundle(url, new CachedAssetBundle(manifestFileName, hashCode), crcNumber);

        webrequest.SendWebRequest();

        while (!webrequest.isDone)
        {
            Debug.Log($"Progreso de descarga: {webrequest.downloadProgress * 100}%");
            yield return null;
        }

        if (webrequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Error descargando el AssetBundle: {webrequest.error}");
            yield break;
        }

        Debug.Log("AssetBundle descargado correctamente.");
        AssetBundle assetBundle = DownloadHandlerAssetBundle.GetContent(webrequest);
        if (assetBundle == null)
        {
            Debug.LogError("El AssetBundle descargado es nulo.");
            yield break;
        }

        string[] allAssetNames = assetBundle.GetAllAssetNames();
        Debug.Log($"{allAssetNames.Length} objetos encontrados en el AssetBundle.");

        foreach (string assetName in allAssetNames)
        {
            string prefabName = Path.GetFileNameWithoutExtension(assetName);
            Debug.Log($"Intentando cargar prefab: {prefabName}");

            GameObject prefab = assetBundle.LoadAsset<GameObject>(assetName);
            if (prefab != null)
            {
                GameObject instance = Instantiate(prefab);
                instance.name = prefabName; // Asignar un nombre legible
                instance.SetActive(false); // Mantener desactivado al inicio

                loadedPrefabs[prefabName] = instance; // Guardar en el diccionario
                Debug.Log($"Prefab '{prefabName}' cargado y desactivado.");
            }
            else
            {
                Debug.LogError($"No se pudo cargar el prefab '{assetName}' desde el AssetBundle.");
            }
        }

        Debug.Log("Cargando todos los prefabs completado.");
        assetBundle.Unload(false);
    }

    public void ActivatePrefab(string prefabName)
    {
        Debug.Log($"Intentando activar el prefab: '{prefabName}'");

        if (loadedPrefabs.ContainsKey(prefabName))
        {
            Debug.Log($"Prefab encontrado en el diccionario: '{prefabName}'");
            GameObject prefabToActivate = loadedPrefabs[prefabName];
            prefabToActivate.SetActive(true);
            Debug.Log($"Prefab '{prefabName}' activado correctamente.");
        }
        else
        {
            Debug.LogWarning($"El prefab '{prefabName}' no existe en el diccionario.");
            Debug.Log("Claves actualmente en el diccionario:");
            foreach (string key in loadedPrefabs.Keys)
            {
                Debug.Log($" - {key}");
            }
        }
    }
}
