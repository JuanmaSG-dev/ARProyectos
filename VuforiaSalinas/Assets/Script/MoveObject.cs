using UnityEngine;
using Vuforia; // Asegúrate de tener Vuforia instalada para usar sus funcionalidades

public class UfoController : MonoBehaviour
{
    private GameObject ufo;
    private Rigidbody ufoRigidbody;
    public float moveSpeed = 2f; // Velocidad de movimiento del Ufo

    public MidAirPositionerBehaviour midAirPositioner; // Referencia al Mid Air de Vuforia

    void Start()
    {
        // Busca el objeto con el tag "Ufo"
        ufo = GameObject.FindGameObjectWithTag("UFO");

        if (ufo == null)
        {
            Debug.LogError("No se encontró ningún objeto con el tag 'Ufo'");
            return;
        }

        // Obtiene el Rigidbody del UFO
        ufoRigidbody = ufo.GetComponent<Rigidbody>();
        if (ufoRigidbody == null)
        {
            Debug.LogError("El objeto Ufo no tiene un componente Rigidbody. Por favor, añádelo.");
            return;
        }

        // Encuentra el componente Mid Air de Vuforia
        midAirPositioner = FindObjectOfType<MidAirPositionerBehaviour>();
        if (midAirPositioner == null)
        {
            Debug.LogWarning("No se encontró un MidAirPositionerBehaviour en la escena.");
        }
    }

    // Funciones de movimiento
    public void MoveForward()
    {
        if (ufoRigidbody != null)
            ufoRigidbody.velocity = Vector3.forward * moveSpeed;
    }

    public void MoveBackward()
    {
        if (ufoRigidbody != null)
            ufoRigidbody.velocity = Vector3.back * moveSpeed;
    }

    public void MoveLeft()
    {
        if (ufoRigidbody != null)
            ufoRigidbody.velocity = Vector3.left * moveSpeed;
    }

    public void MoveRight()
    {
        if (ufoRigidbody != null)
            ufoRigidbody.velocity = Vector3.right * moveSpeed;
    }

    public void MoveUp()
    {
        if (ufoRigidbody != null)
            ufoRigidbody.velocity = Vector3.up * moveSpeed;
    }

    public void MoveDown()
    {
        if (ufoRigidbody != null)
            ufoRigidbody.velocity = Vector3.down * moveSpeed;
    }

    // Detiene el movimiento del Ufo
    public void StopMovement()
    {
        if (ufoRigidbody != null)
            ufoRigidbody.velocity = Vector3.zero;
    }

    // Función para desactivar el Mid Air de Vuforia
    public void DisableMidAir()
    {
        if (midAirPositioner != null)
        {
            midAirPositioner.enabled = false;
            Debug.Log("Mid Air de Vuforia desactivado.");
        }
        else
        {
            Debug.LogWarning("No se encontró el Mid Air Positioner para desactivarlo.");
        }
    }
}
