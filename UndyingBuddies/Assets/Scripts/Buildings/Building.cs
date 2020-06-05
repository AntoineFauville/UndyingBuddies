using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public bool BrokenSoulOnTableExist;
    public bool WhiteSoulOnTableExist;
    public bool BlueVioletSoulOnTableExist;
    public bool VioletSoulOnTableExist;

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
    public GameObject EndVisuStockPileFlow2;
    public ParticleFlowManager ParticleFlowManagerOfInput_01;
    public ParticleFlowManager ParticleFlowManagerOfInput_02;
    public GameObject BuildingLinkedToGenerateFlow_Input01;
    public GameObject BuildingLinkedToGenerateFlow_Input02;

    public GameObject StockPileTrigger;
    public GameObject Highlight;

    public GameObject SoulsInWhell_01;
    public GameObject SoulsInWhell_02;

    [SerializeField] private SoulColor ParticleWell_01;
    [SerializeField] private SoulColor ParticleWell_02;

    public GameObject LoadingBarForProcessing;
    public Image imageFillingProcessing;

    public List <BuildingCommunicator> buildingCommunicators = new List<BuildingCommunicator>();

    [SerializeField] private SoulColor ParticleToKnowWhatWeProduce;

    void Start()
    {
        LoadingBarForProcessing.SetActive(false);

        SoulsInWhell_01.SetActive(false);
        SoulsInWhell_02.SetActive(false);

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

        UpdateVisuParticleWell(ResourceType.whiteSoul);

        StartCoroutine(feedToNotLooseGame());
    }

    void Update()
    {
        for (int i = 0; i < buildingCommunicators.Count; i++)
        {
            if (buildingCommunicators[i] == null)
            {
                buildingCommunicators.Remove(buildingCommunicators[i]);
            }
        }

        if (BuildingLinkedToGenerateFlow_Input01 != null)
        {
            if (BuildingLinkedToGenerateFlow_Input01.GetComponent<Building>().currentStockage <= 0)
            {
                ParticleFlowManagerOfInput_01.active = false;
                ParticleFlowManagerOfInput_01.EndingPoint = null;
                ParticleFlowManagerOfInput_01.StartingPoint = null;
            }
            else if (BuildingLinkedToGenerateFlow_Input01.GetComponent<Building>().currentStockage > 0)
            {
                ParticleFlowManagerOfInput_01.EndingPoint = EndVisuStockPileFlow;
                ParticleFlowManagerOfInput_01.StartingPoint = BuildingLinkedToGenerateFlow_Input01.GetComponent<Building>().StartVisuStockPileFlow;
                ParticleFlowManagerOfInput_01.active = true;
            }

            if (BuildingLinkedToGenerateFlow_Input01.GetComponent<Building>().resourceProducedAtBuilding == ResourceType.whiteSoul)
            {
                ParticleFlowManagerOfInput_01.color = _aiManager.GameSettings.whiteSoulColor;
            }
            else if (BuildingLinkedToGenerateFlow_Input01.GetComponent<Building>().resourceProducedAtBuilding == ResourceType.blueVioletSoul)
            {
                ParticleFlowManagerOfInput_01.color = _aiManager.GameSettings.blueVioletColor;
            }
            else if (BuildingLinkedToGenerateFlow_Input01.GetComponent<Building>().resourceProducedAtBuilding == ResourceType.violetSoul)
            {
                ParticleFlowManagerOfInput_01.color = _aiManager.GameSettings.violetColor;
            }
        }
        else
        {
            ParticleFlowManagerOfInput_01.active = false;
        }
        
        if (BuildingLinkedToGenerateFlow_Input02 != null)
        {
            if (BuildingLinkedToGenerateFlow_Input02.GetComponent<Building>().currentStockage <= 0)
            {
                ParticleFlowManagerOfInput_02.active = false;
                ParticleFlowManagerOfInput_02.EndingPoint = null;
                ParticleFlowManagerOfInput_02.StartingPoint = null;
            }
            else if (BuildingLinkedToGenerateFlow_Input02.GetComponent<Building>().currentStockage > 0)
            {
                ParticleFlowManagerOfInput_02.EndingPoint = EndVisuStockPileFlow2;
                ParticleFlowManagerOfInput_02.StartingPoint = BuildingLinkedToGenerateFlow_Input02.GetComponent<Building>().StartVisuStockPileFlow;
                ParticleFlowManagerOfInput_02.active = true;
            }

            if (BuildingLinkedToGenerateFlow_Input02.GetComponent<Building>().resourceProducedAtBuilding == ResourceType.whiteSoul)
            {
                ParticleFlowManagerOfInput_02.color = _aiManager.GameSettings.whiteSoulColor;
            }
            else if (BuildingLinkedToGenerateFlow_Input02.GetComponent<Building>().resourceProducedAtBuilding == ResourceType.blueVioletSoul)
            {
                ParticleFlowManagerOfInput_02.color = _aiManager.GameSettings.blueVioletColor;
            }
            else if (BuildingLinkedToGenerateFlow_Input02.GetComponent<Building>().resourceProducedAtBuilding == ResourceType.violetSoul)
            {
                ParticleFlowManagerOfInput_02.color = _aiManager.GameSettings.violetColor;
            }
        }
        else
        {
            ParticleFlowManagerOfInput_02.active = false;
        }

        if (resourceProducedAtBuilding == ResourceType.whiteSoul)
        {
            ParticleToKnowWhatWeProduce.ChangeColor(_aiManager.GameSettings.whiteSoulColor);

            if (BrokenSoulOnTableExist)
            {
                SoulsInWhell_01.SetActive(true);
                SoulsInWhell_01.GetComponent<SoulColor>().ChangeColor(_aiManager.GameSettings.brokenSoulColor);
            }
            else
            {
                SoulsInWhell_01.SetActive(false);
            }

            //color of the stockpile
            for (int i = 0; i < StockPileVisuals.Count; i++)
            {
                StockPileVisuals[i].GetComponent<SoulColor>().ChangeColor(_aiManager.GameSettings.whiteSoulColor);
            }
        }

        if (resourceProducedAtBuilding == ResourceType.blueVioletSoul)
        {
            ParticleToKnowWhatWeProduce.ChangeColor(_aiManager.GameSettings.blueVioletColor);

            if (WhiteSoulOnTableExist)
            {
                SoulsInWhell_01.SetActive(true);
                SoulsInWhell_01.GetComponent<SoulColor>().ChangeColor(_aiManager.GameSettings.whiteSoulColor);
            }
            else
            {
                SoulsInWhell_01.SetActive(false);
            }

            //color of the stockpile
            for (int i = 0; i < StockPileVisuals.Count; i++)
            {
                StockPileVisuals[i].GetComponent<SoulColor>().ChangeColor(_aiManager.GameSettings.blueVioletColor);
            }
        }

        if (resourceProducedAtBuilding == ResourceType.violetSoul)
        {
            ParticleToKnowWhatWeProduce.ChangeColor(_aiManager.GameSettings.violetColor);

            if (WhiteSoulOnTableExist)
            {
                SoulsInWhell_01.SetActive(true);
                SoulsInWhell_01.GetComponent<SoulColor>().ChangeColor(_aiManager.GameSettings.whiteSoulColor);
            }
            else
            {
                SoulsInWhell_01.SetActive(false);
            }
            if (BlueVioletSoulOnTableExist)
            {
                SoulsInWhell_02.SetActive(true);
                SoulsInWhell_02.GetComponent<SoulColor>().ChangeColor(_aiManager.GameSettings.blueVioletColor);
            }
            else
            {
                SoulsInWhell_02.SetActive(false);
            }

            //color of the stockpile
            for (int i = 0; i < StockPileVisuals.Count; i++)
            {
                StockPileVisuals[i].GetComponent<SoulColor>().ChangeColor(_aiManager.GameSettings.violetColor);
            }
        }

        if (resourceProducedAtBuilding == ResourceType.blueSoul)
        {
            ParticleToKnowWhatWeProduce.ChangeColor(_aiManager.GameSettings.blueColor);

            if (WhiteSoulOnTableExist)
            {
                SoulsInWhell_01.SetActive(true);
                SoulsInWhell_01.GetComponent<SoulColor>().ChangeColor(_aiManager.GameSettings.whiteSoulColor);
            }
            else
            {
                SoulsInWhell_01.SetActive(false);
            }
            if (VioletSoulOnTableExist)
            {
                SoulsInWhell_02.SetActive(true);
                SoulsInWhell_02.GetComponent<SoulColor>().ChangeColor(_aiManager.GameSettings.violetColor);
            }
            else
            {
                SoulsInWhell_02.SetActive(false);
            }

            //color of the stockpile
            for (int i = 0; i < StockPileVisuals.Count; i++)
            {
                StockPileVisuals[i].GetComponent<SoulColor>().ChangeColor(_aiManager.GameSettings.blueColor);
            }
        }

        if (resourceProducedAtBuilding == ResourceType.redSoul)
        {
            ParticleToKnowWhatWeProduce.ChangeColor(_aiManager.GameSettings.redColor);

            if (BlueVioletSoulOnTableExist)
            {
                SoulsInWhell_01.SetActive(true);
                SoulsInWhell_01.GetComponent<SoulColor>().ChangeColor(_aiManager.GameSettings.blueVioletColor);
            }
            else
            {
                SoulsInWhell_01.SetActive(false);
            }
            if (VioletSoulOnTableExist)
            {
                SoulsInWhell_02.SetActive(true);
                SoulsInWhell_02.GetComponent<SoulColor>().ChangeColor(_aiManager.GameSettings.violetColor);
            }
            else
            {
                SoulsInWhell_02.SetActive(false);
            }

            //color of the stockpile
            for (int i = 0; i < StockPileVisuals.Count; i++)
            {
                StockPileVisuals[i].GetComponent<SoulColor>().ChangeColor(_aiManager.GameSettings.redColor);
            }
        }
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
        ParticleFlowManagerOfInput_01.EndingPoint = null;
        ParticleFlowManagerOfInput_01.StartingPoint = null;
        ParticleFlowManagerOfInput_01.active = false;
        ParticleFlowManagerOfInput_02.EndingPoint = null;
        ParticleFlowManagerOfInput_02.StartingPoint = null;
        ParticleFlowManagerOfInput_02.active = false;
    }

    public void UpdateVisuParticleWell(ResourceType recipeBeingProduced)
    {
        switch (recipeBeingProduced)
        {
            case ResourceType.whiteSoul:
                ParticleWell_01.ChangeColor(_aiManager.GameSettings.brokenSoulColor);
                ParticleWell_02.gameObject.SetActive(false);
                break;
            case ResourceType.blueVioletSoul:
                ParticleWell_01.ChangeColor(_aiManager.GameSettings.whiteSoulColor);
                ParticleWell_02.gameObject.SetActive(false);
                break;
            case ResourceType.violetSoul:
                ParticleWell_01.ChangeColor(_aiManager.GameSettings.whiteSoulColor);
                ParticleWell_02.ChangeColor(_aiManager.GameSettings.blueVioletColor);
                ParticleWell_02.gameObject.SetActive(true);
                break;
            case ResourceType.blueSoul:
                ParticleWell_01.ChangeColor(_aiManager.GameSettings.whiteSoulColor);
                ParticleWell_02.ChangeColor(_aiManager.GameSettings.violetColor);
                ParticleWell_02.gameObject.SetActive(true);
                break;
            case ResourceType.redSoul:
                ParticleWell_01.ChangeColor(_aiManager.GameSettings.blueVioletColor);
                ParticleWell_02.ChangeColor(_aiManager.GameSettings.violetColor);
                ParticleWell_02.gameObject.SetActive(true);
                break;
        }
    }

    IEnumerator feedToNotLooseGame()
    {
        yield return new WaitForSeconds(3);
        
        if (Health < 0)
        {
            DestroyBuilding();
        }

        StartCoroutine(feedToNotLooseGame());
    }
}
