using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameSettings : ScriptableObject
{
    public int demonLife = 10;
    public int demonRangeOfDetection = 20;
    public int demonRangeOfCloseBy = 5;

    public BuildingArchetype Barrack;
    public BuildingArchetype cityhall;
    public BuildingArchetype foodHouse;
    public BuildingArchetype woodHouse;
    public BuildingArchetype woodCutter;
    public BuildingArchetype foodProcessor;
    public BuildingArchetype spellHouse;
    
    public int CostOfNewDemonFood = 40;
    public GameObject DemonPrefab;
    public int damageDemons = 5;

    public int PriestHealth = 10;
    public int PriestBuildingHealth = 50;
    public int PriestAttackAmount = 5;

    public SpellArchetype fireSpell;

    public int woodSmallContainer = 20;
    public int woodMediumContainer = 50;

    public int foodSmallContainer = 20;
    public int foodMediumContainer = 50;

    public int energyAmount = 1;

    public int foodAmountToUnlockTerrain = 100;
    public int woodAmountToUnlockTerrain = 100;
    
    public GameObject PriestPrefab;
    public GameObject AIFormationPrefab;
}
