using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //public Vector3 startPosition;
    public Vector3 endPosition;
    //public Vector3 endPosition;
    private float speed = 100f;

    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(endPosition);
        transform.Rotate(Vector3.right * 90);
        //transform.localRotation = Quaternion.;
        StartCoroutine(FlyForward(transform.position, endPosition));
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator FlyForward(Vector3 fromPos, Vector3 toPos)
    {
        float distance = Vector3.Distance(fromPos, toPos);
        float remainingDistance = distance;

        while (remainingDistance > 0)
        {
            transform.position = Vector3.Lerp(fromPos, toPos, 1 - (remainingDistance / distance));
            remainingDistance -= speed * Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
