using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameSettings : ScriptableObject
{
    public int demonLife = 10;
    public int demonRangeOfDetection = 20;
    public int demonRangeOfCloseBy = 5;
    
    public BuildingArchetype processorBuilding;

    public int CostOfNewDemon = 40;
    public GameObject DemonPrefab;
    public int damageDemons = 5;

    public int PriestHealth = 100;
    public int PriestAttackAmount = 10;
    public int PriestMaxMentalHealth = 100;
    public int PriestMaxFearAmount = 100;

    public SpellArchetype fireSpell;
    public SpellArchetype eyeSpell;
    public SpellArchetype spikeSpell;
    public SpellArchetype tentacleSpell;
    public SpellArchetype poisonExplosionSpell;
    public SpellArchetype ratsSpell;

    public int amountOfExplosionForEye = 5;
    public int damageModForSpike_Physical = 2;
    public int damageModForSpike_MentalHealth = 2;
    public int damageModForSpike_Poison = 2;
    public int damageModRat_Physical = 3;
    public int damageModEyeHorror = 10;

    public GameObject RatExplosion;
    public GameObject TentacleReplacingFlammes;
    public GameObject PoisonousTentacles;
    public GameObject FlamesExplosion;

    public GameObject PriestPrefab;
    public GameObject AIFormationPrefab;

    public int BrokenSoulsOnResource = 3;//local
    public int SoulsOnSacrificeResource = 1;// amount of food i get when i sacrifice a bush
    public int SoulConversionOnStockpile = 4;// conversion if i left click on stockpile 
    public int BrokenSoulValueInEnergy = 5; // amount of energy i get when i sacrifice a bush

    public int EnergyGetOutOfSacrificingHouse = 20;
    public int EnergyOutOfDemonSacrifice = 10;

    public int[] CostSpell;

    public CanvasDamage CanvasDamagePrefab;

    public int CostOfNewBuilding = 30;
    
    public int WhiteSoulValueInEnergy = 2;
    public int BlueVioletSoulValueInEnergy = 9;
    public int VioletSoulValueInEnergy = 30;
    public int BlueSoulValueInEnergy = 65;
    public int RedSoulValueInEnergy = 90;

    public Color brokenSoulColor;
    public Color whiteSoulColor;
    public Color blueVioletColor;
    public Color violetColor;
    public Color blueColor;
    public Color redColor;
    public Color defaultWhenNotWorking;

    public int CostToUnlockTwoInputInEnergy = 100;

    public int timeToPrepareWithACamp = 120;
    public GameObject UIAttack;
    public int CampSightIncrease = 4;

    public GameObject DeathPriest;

    public Buff BuffPrefab;
}
