using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public int Health;
    public int maxHealth;
    public BuildingType BuildingType;
    public BuildingArchetype buildingArchetype;

    public bool canBeInteractable;

    public GameObject SpawningPoint_01;

    public List<GameObject> AiAttributedToBuilding = new List<GameObject>();

    public List<GameObject> Stokpile = new List<GameObject>();

    public bool WhatsBeenWorkedOnTheTableExist;
    public bool WorkedOnTableBeenProcessed;

    public List<GameObject> StockPileVisuals = new List<GameObject>();
    public int maxStockage = 48;
    public int currentStockage = 0;

    public UiHealth UiHealth;

    public GameObject BoudingBoxTag;
    public DetectPlacement detectPlacement;

    public GameObject EmplacementWorker;
    public GameObject visualsOnTable;

    public int amountOfWorkerAllowed = 1;

    void Start()
    {
        if(visualsOnTable != null)
            visualsOnTable.SetActive(false);

        if (BuildingType == BuildingType.Barrack)
        {
            Health = GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.Barrack.BuildingHealth;
        }
        else if (BuildingType == BuildingType.CityHall)
        {
            Health = GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.cityhall.BuildingHealth;
            GameObject.Find("Main Camera").GetComponent<AiManager>().Buildings.Add(this.gameObject);
        }
        else if (BuildingType == BuildingType.FoodStock)
        {
            Health = GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.foodHouse.BuildingHealth;
            GameObject.Find("Main Camera").GetComponent<AiManager>().FoodStockageBuilding.Add(this.gameObject);
        }
        else if(BuildingType == BuildingType.WoodStock)
        {
            Health = GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.woodHouse.BuildingHealth;
            GameObject.Find("Main Camera").GetComponent<AiManager>().WoodStockageBuilding.Add(this.gameObject);
        }
        else if(BuildingType == BuildingType.WoodProcessor)
            Health = GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.woodCutter.BuildingHealth;
        else if(BuildingType == BuildingType.FoodProcessor)
            Health = GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.foodProcessor.BuildingHealth;
        else if(BuildingType == BuildingType.EnergyGenerator)
            Health = GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.spellHouse.BuildingHealth;

        maxHealth = Health;

        this.gameObject.GetComponent<CharacterTypeTagger>().characterType = CharacterType.neutral;
        
        UiHealth.life = Health;
        UiHealth.maxLife = maxHealth;
        
        BoudingBoxTag.SetActive(false);

        for (int i = 0; i < StockPileVisuals.Count; i++)
        {
            StockPileVisuals[i].SetActive(false);
        }

        UpdateStockVisu();

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

    public void AddToStockage()
    {
        currentStockage += 1;

        UpdateStockVisu();
    }
    
    public void UpdateStockVisu()
    {
        for (int i = 0; i < StockPileVisuals.Count; i++)
        {
            if (i < currentStockage) // index offset because i'm only calling this 6 times so it will always go under
            {
                StockPileVisuals[i].SetActive(true);
            }
            else
            {
                StockPileVisuals[i].SetActive(false);
            }
        }
    }

    IEnumerator feedToNotLooseGame()
    {
        yield return new WaitForSeconds(6);

        if (BuildingType == BuildingType.CityHall)
        {
            //GameObject.Find("Main Camera").GetComponent<ResourceManager>().amountOfFood += 3;
            //GameObject.Find("Main Camera").GetComponent<ResourceManager>().amountOfWood += 3;
        }

        StartCoroutine(feedToNotLooseGame());
    }
    
}
