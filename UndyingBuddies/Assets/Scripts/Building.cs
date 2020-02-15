using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public int Health;
    public int maxHealth;
    public BuildingType BuildingType;
    public ResourceType resourceProducedAtBuilding;

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

    public GameObject StartVisuStockPileFlow;
    public GameObject EndVisuStockPileFlow;
    public ParticleFlowManager visuFlowParticle;

    public GameObject StockPileTrigger;

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

        Health = _aiManager.GameSettings.processorBuilding.BuildingHealth;

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

        ResetFlowVisu();

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
        if (currentStockage > 0)
        {
            GameObject.Find("Main Camera").GetComponent<ResourceManager>().amountOfEnergy += (currentStockage / 2);
        }

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

    void ResetFlowVisu()
    {
        visuFlowParticle.EndingPoint = null;
        visuFlowParticle.StartingPoint = StartVisuStockPileFlow;
        visuFlowParticle.active = false;
    }

    IEnumerator feedToNotLooseGame()
    {
        yield return new WaitForSeconds(3);

        ResetFlowVisu();

        if (Health < 0)
        {
            DestroyBuilding();
        }

        StartCoroutine(feedToNotLooseGame());
    }
}
