using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDeath : MonoBehaviour
{
    [SerializeField] private GameObject part01;
    [SerializeField] private GameObject Pol;

    void Start()
    {
        Pol.SetActive(false);
        part01.transform.localPosition = new Vector3(Random.Range(-0.5f,0.5f), 0,0);

        int Rand = Random.Range(0, 100);
        if (Rand < 15)
        {
            Pol.SetActive(true);
        }
    }
}
