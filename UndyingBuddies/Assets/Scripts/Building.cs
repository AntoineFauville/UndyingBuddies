using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public int Health;
    public int maxHealth;
    public BuildingType BuildingType;

    public bool canBeInteractable;

    public GameObject SpawningPoint_01;
    public GameObject SpawningPoint_02;

    public List<GameObject> AiAttributedToBuilding = new List<GameObject>();

    public List<GameObject> StockPile = new List<GameObject>();

    public UiHealth UiHealth;

    public GameObject BoudingBoxTag;
    public DetectPlacement detectPlacement;

    void Start()
    {
        if (BuildingType == BuildingType.Barrack)
            Health = GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.Barrack.BuildingHealth;
        if (BuildingType == BuildingType.CityHall)
            Health = GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.cityhall.BuildingHealth;
        if (BuildingType == BuildingType.FoodHouse)
            Health = GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.foodHouse.BuildingHealth;
        if (BuildingType == BuildingType.WoodHouse)
            Health = GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.woodHouse.BuildingHealth;
        if (BuildingType == BuildingType.WoodCutter)
            Health = GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.woodCutter.BuildingHealth;
        if (BuildingType == BuildingType.FoodProcessor)
            Health = GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.foodProcessor.BuildingHealth;
        if (BuildingType == BuildingType.SpellHouse)
            Health = GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.spellHouse.BuildingHealth;

        maxHealth = Health;

        this.gameObject.GetComponent<CharacterTypeTagger>().characterType = CharacterType.neutral;

        if (BuildingType == BuildingType.CityHall)
        {
            GameObject.Find("Main Camera").GetComponent<AiManager>().Buildings.Add(this.gameObject);

        }

        UiHealth.life = Health;
        UiHealth.maxLife = maxHealth;
        
        BoudingBoxTag.SetActive(false);

        StartCoroutine(feedToNotLooseGame());
    }

    public void GetAttack(int damage)
    {
        if (Health > 0)
        {
            Health -= damage;

            UiHealth.life = Health;
        }
        else
        {
            DestroyBuilding();
        }
    }

    void DestroyBuilding()
    {
        GameObject.Find("Main Camera").GetComponent<AiManager>().Buildings.Remove(this.gameObject);

        DestroyImmediate(this.gameObject);
    }

    IEnumerator feedToNotLooseGame()
    {
        yield return new WaitForSeconds(6);

        if (BuildingType == BuildingType.CityHall)
        {
            GameObject.Find("Main Camera").GetComponent<ResourceManager>().amountOfFood += 3;
            GameObject.Find("Main Camera").GetComponent<ResourceManager>().amountOfWood += 3;
        }

        StartCoroutine(feedToNotLooseGame());
    }
    
}
