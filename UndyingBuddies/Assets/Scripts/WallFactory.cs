using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallFactory : MonoBehaviour
{
    [SerializeField] SettingsData _settingsData;

    public Wall CreateFire(Vector3 hit, Quaternion cubeOrientation)
    {
        Wall newPrefab = Instantiate(_settingsData.PrefabWall, hit, cubeOrientation);
        
        GameObject stainsBlack = Instantiate(_settingsData._BlackStainsPrefab, hit, cubeOrientation);

        return newPrefab;
    }

    public GameObject CreateDeamon(Vector3 hit, Quaternion cubeOrientation)
    {
        GameObject deamon = Instantiate(_settingsData.Deamon, hit, cubeOrientation);

        return deamon;
    }
}
