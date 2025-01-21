using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;
using TMPro;

public class Juego : MonoBehaviour
{
    private System.Collections.Generic.List<ARPlane> planes = new System.Collections.Generic.List<ARPlane>();
    [SerializeField] private ARPlaneManager aRPlaneManager;
    public bool isGenerarEnemigos = false;
    private float alturaPlanoDetectado;

    public GameObject[] enemigos;
    public float generarEnemigoTime = 10f;
    float elapsedTime = 0f;
    public GameObject bala;
    public GameObject arCamara;
    private ARPlane suelo;
    public TextMeshProUGUI pointsUI;
    public TextMeshProUGUI gameOverText;
    public int points;
    // Start is called before the first frame update
    void Start()
    {   
        points = 0;
        Physics.gravity = new Vector3(0, -0.2f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (points < -10) {
            gameOverText.gameObject.SetActive(true);
            points = 0;
        }
        pointsUI.text = points.ToString();
        elapsedTime += Time.deltaTime;
        if (elapsedTime > generarEnemigoTime && isGenerarEnemigos)
        {
            elapsedTime = 0f;
            // Generar un nuevo enemigo
            GenerarEnemigoAleatorio();
            gameOverText.gameObject.SetActive(false);
        }
    }

    /*public Vector3 GetRandomPos()
    {
        Mesh planeMesh = suelo.GetComponent<MeshFilter>().mesh;
        Bounds bounds = planeMesh.bounds;

        float minX = suelo.transform.position.x - suelo.transform.localScale.x * bounds.size.x * 0.5f;
        float minZ = suelo.transform.position.z - suelo.transform.localScale.z * bounds.size.z * 0.5f;

        Vector3 newVec = new Vector3(Random.Range(minX, -minX), suelo.transform.position.y + 2, Random.Range(minZ, -minZ));
        return newVec;
    }*/

    void GenerarEnemigoAleatorio()
    {
        if(isGenerarEnemigos)
        {
            int index = Random.Range(0, enemigos.Length);
            Vector3 spawnPosition = new Vector3(Random.Range(-5, 5), 10f, 4f);
            Instantiate(enemigos[index], spawnPosition, Quaternion.Euler(-90, 0, 0));
        }
    }

    public void dispararBala()
    {
        GameObject nuevaBala = Instantiate(bala, arCamara.transform.position, arCamara.transform.rotation * Quaternion.Euler(90, 0, 0));

        nuevaBala.GetComponent<Rigidbody>().AddForce(arCamara.transform.forward * 2000);

        Destroy(nuevaBala, 5f);
    }


    private void OnEnable()
    {
        aRPlaneManager.planesChanged += PlanesFound;
    }

    private void OnDisable()
    {
        aRPlaneManager.planesChanged -= PlanesFound;
    }

    private void PlanesFound(ARPlanesChangedEventArgs datosPlanos){
        if(datosPlanos.added != null && datosPlanos.added.Count > 0){
            planes.AddRange(datosPlanos.added);
        }

        foreach(ARPlane plane in planes){
            if(plane.extents.x * plane.extents.y >= 1){
                alturaPlanoDetectado = plane.center.y;
                DetenerDeteccionPlanos();
                isGenerarEnemigos = true;
            }
        }
    }

    public void DetenerDeteccionPlanos(){
        aRPlaneManager.requestedDetectionMode = UnityEngine.XR.ARSubsystems.PlaneDetectionMode.None;
        
        foreach (ARPlane plane in planes)
        {
            plane.gameObject.SetActive(false);
        }

        isGenerarEnemigos = true;
    }

    public void SumPoints(int puntos)
    {
        points += puntos;
    }

}
