using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallFactory : MonoBehaviour
{
    [SerializeField] SettingsData _settingsData;

    public Wall CreateFire(Vector3 hit)
    {
        Wall newPrefab = Instantiate(_settingsData.PrefabWall, hit, new Quaternion());
        
        GameObject stainsBlack = Instantiate(_settingsData._BlackStainsPrefab, hit, new Quaternion());

        return newPrefab;
    }

    public GameObject CreateDeamon(Vector3 hit)
    {
        GameObject deamon = Instantiate(_settingsData.Deamon, hit, new Quaternion());

        return deamon;
    }
}
