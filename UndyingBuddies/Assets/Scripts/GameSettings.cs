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
    
    public int CostOfNewDemon = 40;
    public GameObject DemonPrefab;
    public int damageDemons = 5;

    public int PriestHealth = 100;
    public int PriestAttackAmount = 10;
    public int PriestMaxMentalHealth = 100;

    public SpellArchetype fireSpell;
    public SpellArchetype eyeSpell;
    public SpellArchetype spikeSpell;
    public SpellArchetype tentacleSpell;
    
    public GameObject PriestPrefab;
    public GameObject AIFormationPrefab;

    public int FoodOnBush = 3;//local
    public int FoodOnSacrificeBush = 1;// amount of food i get when i sacrifice a bush
    public int FoodConversionOnStockpile = 4;// conversion if i left click on stockpile 
    public int EnergyOnSacrificeBush = 5; // amount of energy i get when i sacrifice a bush

    public int WoodOnTree = 3;//local
    public int WoodOnSacrificeTree = 2;// amount of food i get when i sacrifice a bush
    public int WoodConversionOnStockpile = 3;// conversion if i left click on stockpile 
    public int EnergyOnSacrificeTree = 2; // amount of energy i get when i sacrifice a bush

    public int EnergyGetOutOfSacrificingHouse = 20;
    public int EnergyOutOfDemonSacrifice = 10;

    public int[] CostSpell;

    public CanvasDamage CanvasDamagePrefab;
}
