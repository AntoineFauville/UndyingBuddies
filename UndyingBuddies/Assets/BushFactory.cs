using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushFactory : MonoBehaviour
{
    [SerializeField] SettingsData _settingsData;

    [SerializeField] private GameObject[] BushLocation;
    [SerializeField] private Usables Usables;
    
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
        Vector3 randomAreaAroundPoint = new Vector3(Random.Range(0, _settingsData.radius), 0,Random.Range(0, _settingsData.radius));

        BushLife bushlif = Instantiate(_settingsData.BushlifePrefab, BushLocation[i].transform.position + randomAreaAroundPoint, new Quaternion(),null);

        Usables.Bush.Add(bushlif.gameObject);

        bushlif.Setup(Usables, _settingsData.BushHealth);

        return bushlif;
    }

    IEnumerator randomLifeCreator()
    {
        yield return new WaitForSeconds(Random.Range(_settingsData.BushSpawnTimeMinimum, _settingsData.BushSpawnTimeMaxium));
        if (Usables.Bush.Count < _settingsData.MaxBushOnMap)
        {
            CreateBush(Random.Range(0, BushLocation.Length));
        }

        StartCoroutine(randomLifeCreator());
    }
}
