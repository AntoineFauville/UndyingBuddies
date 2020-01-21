using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BuildingArchetype : ScriptableObject
{
    public GameObject PrefabBuilding;
    public int BuildingHealth = 50;
    public int BuildingCostInWood = 5;
    public int BuildingCostInFood = 5;
}
