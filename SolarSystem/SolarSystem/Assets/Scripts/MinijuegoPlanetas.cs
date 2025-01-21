using UnityEngine;
using UnityEngine.UI; // Para usar UI Text
using System.Collections;

public class MinijuegoPlanetas : MonoBehaviour
{
    public string[] planetNames = { "Mercury", "Venus", "Earth", "Mars", "Jupiter", "Saturn", "Neptune", "Uranus", "Sun" };
    public Text planetNameText;  // Aquí se mostrará el nombre del planeta
    public Text timerText;  // Aquí se mostrará el temporizador
    public float tiempoLimite = 10f;  // 10 segundos para adivinar
    public int maxRondas = 5;
    private int rondaActual = 0;
    private string planetaCorrecto;  // El nombre del planeta que debe adivinar el jugador
    private bool juegoActivo = true;
    
    private void Start()
    {
        IniciarRonda();
    }

    void IniciarRonda()
    {
        if (rondaActual >= maxRondas)
        {
            TerminarJuego();
            return;
        }

        // Elegir un planeta al azar
        planetaCorrecto = planetNames[Random.Range(0, planetNames.Length)];
        planetNameText.text = "Adivina el planeta: " + planetaCorrecto;

        // Iniciar el temporizador
        StartCoroutine(Temporizador());
    }

    IEnumerator Temporizador()
    {
        float tiempoRestante = tiempoLimite;
        while (tiempoRestante > 0)
        {
            timerText.text = "Tiempo restante: " + Mathf.Ceil(tiempoRestante) + "s";  // Actualizar el texto del temporizador
            tiempoRestante -= Time.deltaTime;
            yield return null;
        }

        // Si el tiempo se acaba sin respuesta correcta, se pierde la ronda
        PerdidaRonda();
    }

    public void AdivinarPlaneta(string planetaDetectado)
    {
        if (!juegoActivo) return;  // Si el juego terminó no hacer nada

        if (planetaDetectado == planetaCorrecto)
        {
            // El jugador ha adivinado correctamente
            Debug.Log("¡Has acertado!");
            timerText.text = "¡Has acertado!";
            rondaActual++;
            // Puedes añadir lógica de puntos aquí si lo deseas
        }
        else
        {
            // Si no, el jugador perdió
            timerText.text = "Tiempo agotado / Respuesta incorrecta";
            Debug.Log("¡Fallaste!");
            PerdidaRonda();
        }

        // Continuar al siguiente planeta
        IniciarRonda();
    }

    void PerdidaRonda()
    {
        // Mostrar mensaje de "perdiste" o alguna animación si deseas
        Debug.Log("Tiempo agotado o respuesta incorrecta.");
        rondaActual++;
        IniciarRonda();
    }

    void TerminarJuego()
    {
        // Mostrar puntuación final o mensaje de fin del juego
        timerText.text = "Juego terminado";
        Debug.Log("¡Juego terminado!");
        juegoActivo = false;
    }
}
