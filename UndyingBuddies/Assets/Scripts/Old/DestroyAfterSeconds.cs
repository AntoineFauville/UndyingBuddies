using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    [SerializeField] private float seconds;

    void Start()
    {
        StartCoroutine(waittoDie());
    }

    IEnumerator waittoDie()
    {
        yield return new WaitForSeconds(seconds);
        DestroyImmediate(this.gameObject);
    }
}
