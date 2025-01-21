using UnityEngine;
using Vuforia;
using static UnityEngine.CullingGroup;
using static Vuforia.CloudRecoBehaviour;
using System.Collections;

public class SimpleCloudRecoEventHandler : MonoBehaviour
{
    CloudRecoBehaviour mCloudRecoBehaviour;
    bool mIsScanning = false;
    string mTargetMetadata = "";
    string mTargetName = "";
    
    public AssetBuilderLoad assetLoader;

    public ImageTargetBehaviour ImageTargetTemplate;
    public MinijuegoPlanetas minijuegoPlanetas;

    // Register cloud reco callbacks
    void Awake()
    {
        mCloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();
        mCloudRecoBehaviour.RegisterOnInitializedEventHandler(OnInitialized);
        mCloudRecoBehaviour.RegisterOnInitErrorEventHandler(OnInitError);
        mCloudRecoBehaviour.RegisterOnUpdateErrorEventHandler(OnUpdateError);
        mCloudRecoBehaviour.RegisterOnStateChangedEventHandler(OnStateChanged);
        mCloudRecoBehaviour.RegisterOnNewSearchResultEventHandler(OnNewSearchResult);
    }
    //Unregister cloud reco callbacks when the handler is destroyed
    void OnDestroy()
    {
        mCloudRecoBehaviour.UnregisterOnInitializedEventHandler(OnInitialized);
        mCloudRecoBehaviour.UnregisterOnInitErrorEventHandler(OnInitError);
        mCloudRecoBehaviour.UnregisterOnUpdateErrorEventHandler(OnUpdateError);
        mCloudRecoBehaviour.UnregisterOnStateChangedEventHandler(OnStateChanged);
        mCloudRecoBehaviour.UnregisterOnNewSearchResultEventHandler(OnNewSearchResult);
    }

    public void OnInitialized(CloudRecoBehaviour cloudRecoBehaviour)
    {
        Debug.Log("Cloud Reco initialized");
    }

    public void OnInitError(CloudRecoBehaviour.InitError initError)
    {
        Debug.Log("Cloud Reco init error " + initError.ToString());
    }

    public void OnUpdateError(CloudRecoBehaviour.QueryError updateError)
    {
        Debug.Log("Cloud Reco update error " + updateError.ToString());

    }

    public void OnStateChanged(bool scanning)
    {
        mIsScanning = scanning;

        if (scanning)
        {
            // Clear all known targets
        }
    }

    // Here we handle a cloud target recognition event
    public void OnNewSearchResult(CloudRecoBehaviour.CloudRecoSearchResult cloudRecoSearchResult)
    {
        // Store the target metadata
        mTargetMetadata = cloudRecoSearchResult.MetaData;
        mTargetName = cloudRecoSearchResult.TargetName;

        //assetLoader.ActivatePrefab(mTargetName);
        StartCoroutine(ActivateTarget(mTargetName));
        minijuegoPlanetas.AdivinarPlaneta(mTargetName);

        // Stop the scanning by disabling the behaviour
        mCloudRecoBehaviour.enabled = false;

        // Build augmentation based on target 
        if (ImageTargetTemplate)
        {
            /* Enable the new result with the same ImageTargetBehaviour: */
            mCloudRecoBehaviour.EnableObservers(cloudRecoSearchResult, ImageTargetTemplate.gameObject);
        }
    }

    IEnumerator ActivateTarget(string targetName)
    {
        // Obtener el objeto base que contiene los planetas
        Transform cloudRecognitionTransform = transform;

        // Buscar todos los hijos, incluidos los desactivados
        foreach (Transform child in cloudRecognitionTransform.GetComponentsInChildren<Transform>(true))
        {
            if (child.gameObject.name == targetName)
            {
                // Activar solo el objetivo
                child.gameObject.SetActive(true);
                Debug.Log($"Se activó el objeto: {targetName}");

                // Asegurarse de que el MeshRenderer esté activado
                MeshRenderer meshRenderer = child.GetComponentInChildren<MeshRenderer>();
                if (meshRenderer != null)
                {
                    meshRenderer.enabled = true; // Activar el Mesh Renderer si estaba desactivado
                    Debug.Log($"Mesh Renderer de {targetName} activado.");
                }
                
                yield return new WaitForSeconds(2f);
                child.gameObject.SetActive(false);
            }
        }

        Debug.LogError($"No se encontró el objeto '{targetName}' para activar.");
    }



}
