using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtSimple : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(slowUpdate());
    }
   
    IEnumerator slowUpdate()
    {
        yield return new WaitForSeconds(0.5f);

        transform.LookAt(GameObject.Find("Main Camera").transform);

        StartCoroutine(slowUpdate());
    }
}
