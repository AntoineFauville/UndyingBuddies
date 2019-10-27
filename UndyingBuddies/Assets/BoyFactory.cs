using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoyFactory : MonoBehaviour
{
    public int BoyAmount = 5;
    public GameObject BoySpawningPlace;
    public GameObject BoyPrefab;

    public List<GameObject> Boys = new List<GameObject>();
    private int boyName;

    void Start()
    {
        for (int i = 0; i < BoyAmount; i++)
        {
            CreateBoy(BoySpawningPlace);
        }
    }

    public void CreateBoy(GameObject spawnLocation)
    {
        GameObject boy = Instantiate(BoyPrefab, spawnLocation.transform.position, new Quaternion(), null);

        boy.name = "Boy " + boyName;
        boyName++;

        Boys.Add(boy);
    }
}
