using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallFactory : MonoBehaviour
{
    [SerializeField] SettingsData _settingsData;

    public Wall CreateFire(Vector3 hit, float scale)
    {
        Wall newPrefab = Instantiate(_settingsData.PrefabWall, hit, new Quaternion());

        newPrefab.transform.localScale = new Vector3(scale, scale, scale);

        GameObject stainsBlack = Instantiate(_settingsData._BlackStainsPrefab, hit, new Quaternion());

        stainsBlack.transform.localScale = new Vector3(scale, scale, scale);

        return newPrefab;
    }

    public GameObject CreateDeamon(Vector3 hit, float scale)
    {
        GameObject deamon = Instantiate(_settingsData.Deamon, hit, new Quaternion());

        return deamon;
    }
}
