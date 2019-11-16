using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallFactory : MonoBehaviour
{
    [SerializeField] SettingsData _settingsData;

    public Wall CreateWall(Vector3 hit, Quaternion cubeOrientation)
    {
        Wall newWall = Instantiate(_settingsData.PrefabWall, hit, cubeOrientation);
        
        return newWall;
    }
}
