using System.Collections;
using UnityEngine;

public class CreateEnemy : MonoBehaviour
{
    public GameObject enemy;
    public Transform mainCameraTransform;
    private bool isCreate = false;
    public float altura = 0;

    // Update is called once per frame
    void Update()
    {
        if (isCreate)
            return;
        StartCoroutine(CreateNewEnemy(enemy));
        isCreate = false;

    }

    IEnumerator CreateNewEnemy(GameObject enemy)
    {
        yield return new WaitForSeconds(Random.Range(1, 3));
        isCreate = true;
        float y = Random.Range(1, 3) + altura;
        Instantiate(enemy, mainCameraTransform.position + new Vector3(Random.Range(1, 3), y, 0), Quaternion.identity).GetComponent<DeleteEnemy>().planoY = altura;


    }
}