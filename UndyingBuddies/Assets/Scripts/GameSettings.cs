using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameSettings : ScriptableObject
{
    public int demonLife = 10;
    public int demonRangeOfDetection = 20;
    public int demonRangeOfCloseBy = 5;
    public GameObject woodResourcePrefab;
    public int maxWoodCanCarry = 5;
    public GameObject foodResourcePrefab;
    public int maxFoodCanCarry = 5;
    public GameObject energyResourcePrefab;

    public BuildingArchetype Barrack;
    public BuildingArchetype cityhall;
    public BuildingArchetype foodHouse;
    public BuildingArchetype woodHouse;
    public BuildingArchetype woodCutter;
    public BuildingArchetype foodProcessor;

    public int initialFoodAmount = 20;
    public int initialWoodAmount = 20;
    public int initialEnergyAmount = 0;

    public int CostOfNewDemonFood = 40;
    public GameObject DemonPrefab;
    public int damageDemons = 5;

    public int PriestHealth = 10;
    public int PriestBuildingHealth = 50;
    public int PriestAttackAmount = 5;
}
