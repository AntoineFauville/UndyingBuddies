using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoyFactory : MonoBehaviour
{
    public int BoyAmount = 5;
    public GameObject BoySpawningPlace;
    public GameObject BoyPrefab;

    void Start()
    {
        for (int i = 0; i < BoyAmount; i++)
        {
            CreateBoy(BoySpawningPlace);
        }
    }

    public void CreateBoy(GameObject spawnLocation)
    {
        Instantiate(BoyPrefab, spawnLocation.transform.position, new Quaternion(), null);
    }
}
