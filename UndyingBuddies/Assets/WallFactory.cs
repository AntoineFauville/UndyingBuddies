using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallFactory : MonoBehaviour
{
    [SerializeField] private Wall PrefabWall;

    public Wall CreateWall(Vector3 hit, Quaternion cubeOrientation)
    {
        Wall newWall = Instantiate(PrefabWall, hit, cubeOrientation);
        
        return newWall;
    }
}
