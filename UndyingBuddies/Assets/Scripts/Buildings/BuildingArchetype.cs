using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BuildingArchetype : ScriptableObject
{
    public string TheName;
    public GameObject PrefabBuilding;
    public int BuildingHealth = 50;
}
