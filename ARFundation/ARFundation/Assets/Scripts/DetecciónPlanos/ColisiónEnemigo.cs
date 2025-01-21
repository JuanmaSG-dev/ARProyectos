using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ColisiónEnemigo : MonoBehaviour
{
    public GameObject explosionPrefab;
    public float alturaSuelo;
    private int points;
    public Juego juego;
    // Start is called before the first frame update
    void Start()
    {
        juego = FindObjectOfType<Juego>();
        if (juego != null)
        {
            Debug.Log("Se encontró el componente Juego en la escena.");
        }
        else
        {
            Debug.LogError("No se encontró el componente Juego en la escena.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.y <= alturaSuelo)
        {
            points = -1;
            juego.SumPoints(points);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bala"))
        {
            points = 1;
            juego.SumPoints(points);
            Vector3 collisionPosition = collision.contacts[0].point;

            GameObject explosion = new GameObject();
            Instantiate(explosion, collisionPosition, Quaternion.identity);

            Destroy(explosion, 1f);
            Destroy(collision.gameObject);
            Destroy(gameObject, 0.5f);
        }
    }
}
