using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoyFactory : MonoBehaviour
{
    [SerializeField] SettingsData _settingsData;
    [SerializeField] private GameManager _gameManager;
    public GameObject[] BoySpawningPlace;

    public List<GameObject> TotalOfTheBoys = new List<GameObject>();
    private int boyName;

    void Start()
    {
        for (int i = 0; i < _gameManager.BoyAmount; i++)
        {
            CreateBoy(BoySpawningPlace[Random.Range(0,BoySpawningPlace.Length)], 3);
        }
    }

    public void CreateBoy(GameObject spawnLocation, float Radius)
    {
        Vector3 spawnLocationOfBoy = new Vector3(spawnLocation.transform.position.x + Random.Range(-Radius, +Radius),
            spawnLocation.transform.position.y,
            spawnLocation.transform.position.z + Random.Range(-Radius, +Radius));

        GameObject boy = Instantiate(_settingsData.BoyPrefab, spawnLocationOfBoy, new Quaternion(), null);

        boy.name = "Boy " + boyName;
        boyName++;

        TotalOfTheBoys.Add(boy);
    }
}
