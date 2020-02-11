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
    public int amountOfActiveWorker = 0;

    [SerializeField] private AiManager _aiManager;

    void Start()
    {
        BoudingBoxTag.SetActive(false);
        detectPlacement.gameObject.SetActive(false);

        if (visualsOnTable != null)
            visualsOnTable.SetActive(false);

        if (_aiManager == null)
        {
            _aiManager = GameObject.Find("Main Camera").GetComponent<AiManager>();
        }

        if (BuildingType == BuildingType.Barrack)
        {
            Health = _aiManager.GameSettings.Barrack.BuildingHealth;
        }
        else if (BuildingType == BuildingType.CityHall)
        {
            Health = _aiManager.GameSettings.cityhall.BuildingHealth;
        }
        else if (BuildingType == BuildingType.FoodStock)
        {
            Health = _aiManager.GameSettings.foodHouse.BuildingHealth;
            _aiManager.FoodStockageBuilding.Add(this.gameObject);
        }
        else if(BuildingType == BuildingType.WoodStock)
        {
            Health = _aiManager.GameSettings.woodHouse.BuildingHealth;
            _aiManager.WoodStockageBuilding.Add(this.gameObject);
        }
        else if(BuildingType == BuildingType.WoodProcessor)
            Health = _aiManager.GameSettings.woodCutter.BuildingHealth;
        else if(BuildingType == BuildingType.FoodProcessor)
            Health = _aiManager.GameSettings.foodProcessor.BuildingHealth;
        else if(BuildingType == BuildingType.EnergyGenerator)
            Health = _aiManager.GameSettings.spellHouse.BuildingHealth;

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
        if (_aiManager.Buildings.Contains(this.gameObject))
        {
            _aiManager.Buildings.Remove(this.gameObject);
        }

        if (_aiManager.BuildingWithJobs.Contains(this.gameObject))
        {
            _aiManager.BuildingWithJobs.Remove(this.gameObject);
        }

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

        if (Health < 0)
        {
            DestroyBuilding();
        }

        StartCoroutine(feedToNotLooseGame());
    }
}
