using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushFactory : MonoBehaviour
{

    [SerializeField] private GameObject[] BushLocation;
    [SerializeField] private Usables Usables;

    [SerializeField] private BushLife BushlifePrefab;

    [SerializeField] private float radius = 2;
    [SerializeField] private int MaxBushOnMap = 10;

    void Start()
    {
        for (int i = 0; i < BushLocation.Length; i++)
        {
            CreateBush(i);
        }

        StartCoroutine(randomLifeCreator());
    }

    public BushLife CreateBush(int i)
    {
        Vector3 randomAreaAroundPoint = new Vector3(Random.Range(0, radius), 0,Random.Range(0, radius));

        BushLife bushlif = Instantiate(BushlifePrefab, BushLocation[i].transform.position + randomAreaAroundPoint, new Quaternion(),null);

        Usables.Bush.Add(bushlif.gameObject);

        bushlif.Setup(Usables);

        return bushlif;
    }

    IEnumerator randomLifeCreator()
    {
        yield return new WaitForSeconds(Random.Range(2, 5));
        if (Usables.Bush.Count < MaxBushOnMap)
        {
            CreateBush(Random.Range(0, BushLocation.Length));
        }

        StartCoroutine(randomLifeCreator());
    }
}
