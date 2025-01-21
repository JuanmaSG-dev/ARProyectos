using UnityEngine;

public class DeleteEnemy : MonoBehaviour
{

    public float planoY;


    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime);
        if (Physics.Raycast(transform.position, -transform.up) || planoY - 2 > transform.position.y)
        {
            Destroy(gameObject);
        }
    }
}