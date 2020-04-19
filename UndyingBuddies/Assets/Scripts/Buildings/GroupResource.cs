using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupResource : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> listOfResource;

    void Start()
    {
        StartCoroutine(waitForSec());
    }

    IEnumerator waitForSec()
    {
        for (int i = 0; i < listOfResource.Count; i++)
        {
            if (listOfResource[i] == null)
            {
                listOfResource.Remove(listOfResource[i]);
            }
        }

        if (listOfResource.Count == 0)
        {
            DestroyImmediate(this.gameObject);
        }

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(waitForSec());
    }
}
