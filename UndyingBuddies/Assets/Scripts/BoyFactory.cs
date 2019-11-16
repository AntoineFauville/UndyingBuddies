using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoyFactory : MonoBehaviour
{
    [SerializeField] SettingsData _settingsData;
    public GameObject BoySpawningPlace;

    public List<GameObject> Boys = new List<GameObject>();
    private int boyName;

    void Start()
    {
        for (int i = 0; i < _settingsData.BoyAmount; i++)
        {
            CreateBoy(BoySpawningPlace);
        }
    }

    public void CreateBoy(GameObject spawnLocation)
    {
        GameObject boy = Instantiate(_settingsData.BoyPrefab, spawnLocation.transform.position, new Quaternion(), null);

        boy.name = "Boy " + boyName;
        boyName++;

        Boys.Add(boy);
    }
}
