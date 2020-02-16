using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiManager : MonoBehaviour
{
    public GameSettings GameSettings;

    //all the AI
    public List<GameObject> Demons = new List<GameObject>();
    public List<GameObject> IdlingDemons = new List<GameObject>();

    //All the AI and buildings!!
    public List<GameObject> Priest = new List<GameObject>();

    //all the wood supply around
    public List<GameObject> ResourceToProcess = new List<GameObject>();

    public List<GameObject> WhiteSoulStockage = new List<GameObject>();
    public List<GameObject> BlueVioletSoulStockage = new List<GameObject>();
    public List<GameObject> VioletSoulStorage = new List<GameObject>();
    public List<GameObject> BlueSoulStockage = new List<GameObject>();
    public List<GameObject> RedSoulStorage = new List<GameObject>();

    public List<GameObject> Buildings = new List<GameObject>();
    public List<GameObject> BuildingWithJobs = new List<GameObject>();
    public bool AreBuildingShowned;
    
    int nameID;

    //enemy
    public List<GameObject> AIPriest = new List<GameObject>();
    public List<GameObject> AIPriestBuildings = new List<GameObject>();

    void Start()
    {
        AddDemons();
        AddBuilding();
        AddAllTheInitialResource();
        StartCoroutine(SlowUpdate());
    }

    public void AddAllTheInitialResource()
    {
        foreach (var resource in GameObject.FindGameObjectsWithTag("Resource"))
        {
            if (!resource.GetComponent<Resource>().processedResource)
            {
                ResourceToProcess.Add(resource);
            }
        }
    }

    public void AddBuilding()
    {
        foreach (var building in GameObject.FindObjectsOfType<Building>())
        {
            if (!Buildings.Contains(building.gameObject))
            {
                Buildings.Add(building.gameObject);
            }

            if (building.amountOfWorkerAllowed > building.amountOfActiveWorker && !BuildingWithJobs.Contains(building.gameObject))
            {
                BuildingWithJobs.Add(building.gameObject);
            }

            if (building.resourceProducedAtBuilding == ResourceType.whiteSoul && !WhiteSoulStockage.Contains(building.gameObject))
            {
                WhiteSoulStockage.Add(building.gameObject);
            }

            if (building.resourceProducedAtBuilding == ResourceType.blueVioletSoul && !BlueVioletSoulStockage.Contains(building.gameObject))
            {
                BlueVioletSoulStockage.Add(building.gameObject);
            }

            if (building.resourceProducedAtBuilding == ResourceType.violetSoul && !VioletSoulStorage.Contains(building.gameObject))
            {
                VioletSoulStorage.Add(building.gameObject);
            }

            if (building.resourceProducedAtBuilding == ResourceType.blueSoul && !BlueSoulStockage.Contains(building.gameObject))
            {
                BlueSoulStockage.Add(building.gameObject);
            }

            if (building.resourceProducedAtBuilding == ResourceType.redSoul && !RedSoulStorage.Contains(building.gameObject))
            {
                RedSoulStorage.Add(building.gameObject);
            }
        }
    }

    public void AddResource(GameObject resourceGameObject)
    {
        if (resourceGameObject.GetComponent<Resource>() == null)
        {
            Debug.Log("can't do anything about this resource");
        }

        ResourceToProcess.Add(resourceGameObject);
    }

    void RemoveResource(GameObject resourceGameObject)
    {
        if (resourceGameObject.GetComponent<Resource>() == null)
        {
            Debug.Log("can't do anything about this resource");
        }

        ResourceToProcess.Remove(resourceGameObject);
    }

    void AddDemons()
    {
        foreach (var demon in GameObject.FindGameObjectsWithTag("demon"))
        {
            Demons.Add(demon);
            demon.name = "demon_" + nameID;
            nameID++;

            demon.GetComponent<AIDemons>().Setup(demon.name, JobType.IdleVillager, GameSettings.demonLife, GameSettings.demonRangeOfDetection, GameSettings.demonRangeOfCloseBy);
        }
    }

    void CheckForJobLessBuilding()
    {
        for (int i = 0; i < Buildings.Count; i++)
        {
            if (Buildings[i].GetComponent<Building>().amountOfWorkerAllowed > Buildings[i].GetComponent<Building>().amountOfActiveWorker && !BuildingWithJobs.Contains(Buildings[i]))
            {
                BuildingWithJobs.Add(Buildings[i]);
            }
        }
    }

    void CheckForJobLessDemon()
    {
        for (int i = 0; i < Demons.Count; i++)
        {
            if (Demons[i].GetComponent<AIDemons>().JobType == JobType.IdleVillager)
            {
                if (!IdlingDemons.Contains(Demons[i]))
                {
                    IdlingDemons.Add(Demons[i]);
                }
            }
        }
    }

    void AssignJobLessDemons()
    {
        if (BuildingWithJobs.Count > 0 && IdlingDemons.Count > 0)
        {
            AIDemons demon;
            GameObject building;

            demon = IdlingDemons[0].GetComponent<AIDemons>();
            building = BuildingWithJobs[0];

            demon.AssignedBuilding = BuildingWithJobs[0];

            if (building.GetComponent<Building>().BuildingType == BuildingType.Processor)
            {
                demon.JobType = JobType.processor;
            }
            else
            {
                Debug.Log("you're trying to assign a jobless demon to a uncapacitated building");
            }

            BuildingWithJobs[0].GetComponent<Building>().amountOfActiveWorker++;

            IdlingDemons.Remove(IdlingDemons[0]);
            BuildingWithJobs.Remove(BuildingWithJobs[0]);
        }
    }
    
    void CleanTheListFromEmptyObjects()
    {
        for (int i = 0; i < WhiteSoulStockage.Count; i++)
        {
            if (WhiteSoulStockage[i] == null)
            {
                WhiteSoulStockage.Remove(WhiteSoulStockage[i]);
            }
        }

        for (int i = 0; i < BlueVioletSoulStockage.Count; i++)
        {
            if (BlueVioletSoulStockage[i] == null)
            {
                BlueVioletSoulStockage.Remove(BlueVioletSoulStockage[i]);
            }
        }

        for (int i = 0; i < VioletSoulStorage.Count; i++)
        {
            if (VioletSoulStorage[i] == null)
            {
                VioletSoulStorage.Remove(VioletSoulStorage[i]);
            }
        }

        for (int i = 0; i < BlueSoulStockage.Count; i++)
        {
            if (BlueSoulStockage[i] == null)
            {
                BlueSoulStockage.Remove(BlueSoulStockage[i]);
            }
        }

        for (int i = 0; i < RedSoulStorage.Count; i++)
        {
            if (RedSoulStorage[i] == null)
            {
                RedSoulStorage.Remove(RedSoulStorage[i]);
            }
        }

        for (int i = 0; i < ResourceToProcess.Count; i++)
        {
            if (ResourceToProcess[i] == null)
            {
                ResourceToProcess.Remove(ResourceToProcess[i]);
            }
        }

        for (int i = 0; i < Demons.Count; i++)
        {
            if (Demons[i] == null)
            {
                Demons.Remove(Demons[i]);
            }
        }

        for (int i = 0; i < Priest.Count; i++)
        {
            if (Priest[i] == null)
            {
                Priest.Remove(Priest[i]);
            }
        }

        for (int i = 0; i < Buildings.Count; i++)
        {
            if (Buildings[i] == null)
            {
                Buildings.Remove(Buildings[i]);
            }
        }

        for (int i = 0; i < BuildingWithJobs.Count; i++)
        {
            if (BuildingWithJobs[i] == null)
            {
                BuildingWithJobs.Remove(BuildingWithJobs[i]);
            }
        }
    }

    IEnumerator SlowUpdate()
    {
        CleanTheListFromEmptyObjects();

        CheckForJobLessDemon();

        CheckForJobLessBuilding();

        AssignJobLessDemons();

        //check if all buildings are in list, they disapear sometimes..
        //AddBuilding();

        for (int i = 0; i < Demons.Count; i++)
        {
            JobType demonJobType;
            AIDemons currentAiDemon;

            currentAiDemon = Demons[i].GetComponent<AIDemons>();
            demonJobType = currentAiDemon.JobType;

            if (currentAiDemon.life > 0)
            {
                switch (demonJobType)
                {
                    case JobType.processor:
                        if (currentAiDemon.AssignedBuilding == null)
                        {
                            currentAiDemon.JobType = JobType.IdleVillager;
                            Debug.Log("Building might be destroyed");
                        }
                        else
                        {
                            if (currentAiDemon.AssignedBuilding.GetComponentInParent<Building>().BrokenSoulOnTableExist && 
                                currentAiDemon.AssignedBuilding.GetComponentInParent<Building>().resourceProducedAtBuilding == ResourceType.whiteSoul)
                            {
                                if (currentAiDemon.AssignedBuilding.GetComponentInParent<Building>().WorkedOnTableBeenProcessed)
                                {
                                    if (currentAiDemon.SoulBasketAmount > 0)
                                    {
                                        //once produced you need to place these somewhere check if the correct stock exists somewhere
                                        if (currentAiDemon.AssignedBuilding.GetComponentInParent<Building>().resourceProducedAtBuilding == ResourceType.whiteSoul)
                                        {
                                            if (currentAiDemon.CheckIfThereIsAWayToPlaceStockage(ResourceType.whiteSoul))
                                            {
                                                if (currentAiDemon.CheckForClosestBuildingToPlaceStockage())//check the closest and if i'm nearby returns true
                                                {
                                                    currentAiDemon.PlaceInStockpile();
                                                }
                                                else
                                                {
                                                    currentAiDemon.Walk();
                                                }
                                            }
                                            else
                                            {
                                                Debug.Log("No More Stockage Units for " + ResourceType.whiteSoul);
                                                currentAiDemon.Idle();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        currentAiDemon.TakeFromTable();
                                    }
                                }
                                else
                                {
                                    if (currentAiDemon.checkIfGivenObjectIscloseBy(currentAiDemon.AssignedBuilding))
                                    {
                                        currentAiDemon.Process(); // change process to transform the item on table into processed
                                    }
                                    else
                                    {
                                        currentAiDemon.Walk();
                                    }
                                }
                            }
                            else if (currentAiDemon.AssignedBuilding.GetComponentInParent<Building>().WhiteSoulOnTableExist &&
                                currentAiDemon.AssignedBuilding.GetComponentInParent<Building>().resourceProducedAtBuilding == ResourceType.blueVioletSoul)
                            {
                                if (currentAiDemon.AssignedBuilding.GetComponentInParent<Building>().WorkedOnTableBeenProcessed)
                                {
                                    if (currentAiDemon.SoulBasketAmount > 0)
                                    {
                                        //once produced you need to place these somewhere check if the correct stock exists somewhere
                                        if (currentAiDemon.AssignedBuilding.GetComponentInParent<Building>().resourceProducedAtBuilding == ResourceType.blueVioletSoul)
                                        {
                                            if (currentAiDemon.CheckIfThereIsAWayToPlaceStockage(ResourceType.blueVioletSoul))
                                            {
                                                if (currentAiDemon.CheckForClosestBuildingToPlaceStockage())//check the closest and if i'm nearby returns true
                                                {
                                                    currentAiDemon.PlaceInStockpile();
                                                }
                                                else
                                                {
                                                    currentAiDemon.Walk();
                                                    Debug.Log("Not close enought from stockpile of " + ResourceType.blueVioletSoul);
                                                }
                                            }
                                            else
                                            {
                                                Debug.Log("No More Stockage Units for " + ResourceType.blueVioletSoul);
                                                currentAiDemon.Idle();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        currentAiDemon.TakeFromTable();
                                    }
                                }
                                else
                                {
                                    if (currentAiDemon.checkIfGivenObjectIscloseBy(currentAiDemon.AssignedBuilding))
                                    {
                                        currentAiDemon.Process(); // change process to transform the item on table into processed
                                    }
                                    else
                                    {
                                        currentAiDemon.Walk();
                                        Debug.Log("Not close enought to my assigned building");
                                    }
                                }
                            }
                            else if (currentAiDemon.AssignedBuilding.GetComponentInParent<Building>().WhiteSoulOnTableExist && 
                                currentAiDemon.AssignedBuilding.GetComponentInParent<Building>().BlueVioletSoulOnTableExist &&
                                currentAiDemon.AssignedBuilding.GetComponentInParent<Building>().resourceProducedAtBuilding == ResourceType.violetSoul)
                            {
                                if (currentAiDemon.AssignedBuilding.GetComponentInParent<Building>().WorkedOnTableBeenProcessed)
                                {
                                    if (currentAiDemon.SoulBasketAmount > 0)
                                    {
                                        //once produced you need to place these somewhere check if the correct stock exists somewhere
                                        if (currentAiDemon.AssignedBuilding.GetComponentInParent<Building>().resourceProducedAtBuilding == ResourceType.violetSoul)
                                        {
                                            if (currentAiDemon.CheckIfThereIsAWayToPlaceStockage(ResourceType.violetSoul))
                                            {
                                                if (currentAiDemon.CheckForClosestBuildingToPlaceStockage())//check the closest and if i'm nearby returns true
                                                {
                                                    currentAiDemon.PlaceInStockpile();
                                                }
                                                else
                                                {
                                                    currentAiDemon.Walk();
                                                    Debug.Log("Not close enought from stockpile of " + ResourceType.violetSoul);
                                                }
                                            }
                                            else
                                            {
                                                Debug.Log("No More Stockage Units for " + ResourceType.violetSoul);
                                                currentAiDemon.Idle();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        currentAiDemon.TakeFromTable();
                                    }
                                }
                                else
                                {
                                    if (currentAiDemon.checkIfGivenObjectIscloseBy(currentAiDemon.AssignedBuilding))
                                    {
                                        currentAiDemon.Process(); // change process to transform the item on table into processed
                                    }
                                    else
                                    {
                                        currentAiDemon.Walk();
                                        Debug.Log("Not close enought to my assigned building");
                                    }
                                }
                            }
                            else if (currentAiDemon.AssignedBuilding.GetComponentInParent<Building>().WhiteSoulOnTableExist &&
                               currentAiDemon.AssignedBuilding.GetComponentInParent<Building>().VioletSoulOnTableExist &&
                               currentAiDemon.AssignedBuilding.GetComponentInParent<Building>().resourceProducedAtBuilding == ResourceType.blueSoul)
                            {
                                if (currentAiDemon.AssignedBuilding.GetComponentInParent<Building>().WorkedOnTableBeenProcessed)
                                {
                                    if (currentAiDemon.SoulBasketAmount > 0)
                                    {
                                        //once produced you need to place these somewhere check if the correct stock exists somewhere
                                        if (currentAiDemon.AssignedBuilding.GetComponentInParent<Building>().resourceProducedAtBuilding == ResourceType.blueSoul)
                                        {
                                            if (currentAiDemon.CheckIfThereIsAWayToPlaceStockage(ResourceType.blueSoul))
                                            {
                                                if (currentAiDemon.CheckForClosestBuildingToPlaceStockage())//check the closest and if i'm nearby returns true
                                                {
                                                    currentAiDemon.PlaceInStockpile();
                                                }
                                                else
                                                {
                                                    currentAiDemon.Walk();
                                                    Debug.Log("Not close enought from stockpile of " + ResourceType.blueSoul);
                                                }
                                            }
                                            else
                                            {
                                                Debug.Log("No More Stockage Units for " + ResourceType.blueSoul);
                                                currentAiDemon.Idle();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        currentAiDemon.TakeFromTable();
                                    }
                                }
                                else
                                {
                                    if (currentAiDemon.checkIfGivenObjectIscloseBy(currentAiDemon.AssignedBuilding))
                                    {
                                        currentAiDemon.Process(); // change process to transform the item on table into processed
                                    }
                                    else
                                    {
                                        currentAiDemon.Walk();
                                        Debug.Log("Not close enought to my assigned building");
                                    }
                                }
                            }
                            else if (currentAiDemon.AssignedBuilding.GetComponentInParent<Building>().BlueVioletSoulOnTableExist &&
                              currentAiDemon.AssignedBuilding.GetComponentInParent<Building>().VioletSoulOnTableExist &&
                              currentAiDemon.AssignedBuilding.GetComponentInParent<Building>().resourceProducedAtBuilding == ResourceType.redSoul)
                            {
                                if (currentAiDemon.AssignedBuilding.GetComponentInParent<Building>().WorkedOnTableBeenProcessed)
                                {
                                    if (currentAiDemon.SoulBasketAmount > 0)
                                    {
                                        //once produced you need to place these somewhere check if the correct stock exists somewhere
                                        if (currentAiDemon.AssignedBuilding.GetComponentInParent<Building>().resourceProducedAtBuilding == ResourceType.redSoul)
                                        {
                                            if (currentAiDemon.CheckIfThereIsAWayToPlaceStockage(ResourceType.redSoul))
                                            {
                                                if (currentAiDemon.CheckForClosestBuildingToPlaceStockage())//check the closest and if i'm nearby returns true
                                                {
                                                    currentAiDemon.PlaceInStockpile();
                                                }
                                                else
                                                {
                                                    currentAiDemon.Walk();
                                                    Debug.Log("Not close enought from stockpile of " + ResourceType.redSoul);
                                                }
                                            }
                                            else
                                            {
                                                Debug.Log("No More Stockage Units for " + ResourceType.redSoul);
                                                currentAiDemon.Idle();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        currentAiDemon.TakeFromTable();
                                    }
                                }
                                else
                                {
                                    if (currentAiDemon.checkIfGivenObjectIscloseBy(currentAiDemon.AssignedBuilding))
                                    {
                                        currentAiDemon.Process(); // change process to transform the item on table into processed
                                    }
                                    else
                                    {
                                        currentAiDemon.Walk();
                                        Debug.Log("Not close enought to my assigned building");
                                    }
                                }
                            }
                            else
                            {
                                if (currentAiDemon.SoulAmount >= currentAiDemon.MaxSouls)
                                {
                                    if (currentAiDemon.checkIfGivenObjectIscloseBy(currentAiDemon.AssignedBuilding))
                                    {
                                        currentAiDemon.PlaceOnTable();
                                    }
                                    else
                                    {
                                        currentAiDemon.Walk();
                                        Debug.Log("Not close enought to my assigned building to place on table");
                                    }
                                }
                                else
                                {
                                    //define in two part one for taking in storage one for taking in the wild
                                    if (currentAiDemon.AssignedBuilding.GetComponent<Building>().resourceProducedAtBuilding == ResourceType.whiteSoul)
                                    {
                                        if (!currentAiDemon.AssignedBuilding.GetComponent<Building>().BrokenSoulOnTableExist)
                                        {
                                            if (ResourceToProcess.Count > 0)//if they are resource in the nature
                                            {
                                                GameObject whiteSoul = currentAiDemon.FindClosestResourceSupply(ResourceType.brokenSoul);
                                                if (currentAiDemon.checkIfGivenObjectIscloseBy(whiteSoul))
                                                {
                                                    currentAiDemon.Gather(ResourceType.brokenSoul);
                                                    Debug.Log("Gathering " + ResourceType.brokenSoul);
                                                }
                                                else
                                                {
                                                    currentAiDemon.Walk();
                                                }
                                            }
                                            else//otherwise idle
                                            {
                                                if (currentAiDemon.checkIfGivenObjectIscloseBy(currentAiDemon.AssignedBuilding))
                                                {
                                                    currentAiDemon.Idle();
                                                    Debug.Log("No more to gather around me");
                                                }
                                                else
                                                {
                                                    currentAiDemon.Walk();
                                                }
                                            }
                                        }
                                    }
                                    else if (currentAiDemon.AssignedBuilding.GetComponent<Building>().resourceProducedAtBuilding == ResourceType.blueVioletSoul)
                                    {
                                        if (!currentAiDemon.AssignedBuilding.GetComponent<Building>().WhiteSoulOnTableExist)
                                        {
                                            if (WhiteSoulStockage.Count > 0)//if they are actual stock in the game, if no building around then idle at home
                                            {
                                                if (currentAiDemon.CheckIfCurrentStockInStockIsMoreThanZero(ResourceType.whiteSoul)) //is there a stock to take from ? more than 0 
                                                {
                                                    GameObject whiteSoulStockage = currentAiDemon.FindClosestResourceSupply(ResourceType.whiteSoul); //check for stockage //activate to show where you are taking it from

                                                    currentAiDemon.AssignedBuilding.GetComponent<Building>().BuildingLinkedToGenerateFlow_Input01 = whiteSoulStockage;

                                                    if (currentAiDemon.checkIfGivenObjectIscloseBy(whiteSoulStockage))//am i close to closest stockage 
                                                    {
                                                        currentAiDemon.TakeFromStockpile();
                                                        Debug.Log("Taking from stockpile " + ResourceType.whiteSoul);
                                                    }
                                                    else
                                                    {
                                                        currentAiDemon.Walk();
                                                    }
                                                }
                                                else //otherwise idle
                                                {
                                                    if (currentAiDemon.checkIfGivenObjectIscloseBy(currentAiDemon.AssignedBuilding))
                                                    {
                                                        currentAiDemon.Idle();
                                                    }
                                                    else
                                                    {
                                                        currentAiDemon.Walk();
                                                    }
                                                }
                                            }
                                            else //otherwise idle
                                            {
                                                if (currentAiDemon.checkIfGivenObjectIscloseBy(currentAiDemon.AssignedBuilding))
                                                {
                                                    currentAiDemon.Idle();
                                                }
                                                else
                                                {
                                                    currentAiDemon.Walk();
                                                }
                                            }
                                        }
                                    }
                                    else if(currentAiDemon.AssignedBuilding.GetComponent<Building>().resourceProducedAtBuilding == ResourceType.violetSoul)
                                    {
                                        if (!currentAiDemon.AssignedBuilding.GetComponent<Building>().WhiteSoulOnTableExist)
                                        {
                                            if (WhiteSoulStockage.Count > 0)//if they are actual stock in the game, if no building around then idle at home
                                            {
                                                if (currentAiDemon.CheckIfCurrentStockInStockIsMoreThanZero(ResourceType.whiteSoul)) //is there a stock to take from ? more than 0 
                                                {
                                                    GameObject whiteSoulStockage = currentAiDemon.FindClosestResourceSupply(ResourceType.whiteSoul); //check for stockage
                                                                                                                                                     //activate to show where you are taking it from
                                                    currentAiDemon.AssignedBuilding.GetComponent<Building>().BuildingLinkedToGenerateFlow_Input01 = whiteSoulStockage;
                                                    
                                                    if (currentAiDemon.checkIfGivenObjectIscloseBy(whiteSoulStockage))//am i close to closest stockage 
                                                    {
                                                        currentAiDemon.TakeFromStockpile();
                                                        Debug.Log("Taking from stockpile " + ResourceType.whiteSoul);
                                                    }
                                                    else
                                                    {
                                                        currentAiDemon.Walk();
                                                    }
                                                }
                                                else //otherwise idle
                                                {
                                                    if (currentAiDemon.checkIfGivenObjectIscloseBy(currentAiDemon.AssignedBuilding))
                                                    {
                                                        currentAiDemon.Idle();
                                                    }
                                                    else
                                                    {
                                                        currentAiDemon.Walk();
                                                    }
                                                }
                                            }
                                            else //otherwise idle
                                            {
                                                Debug.Log("No more " + ResourceType.whiteSoul + " in stockpile");

                                                if (currentAiDemon.checkIfGivenObjectIscloseBy(currentAiDemon.AssignedBuilding))
                                                {
                                                    currentAiDemon.Idle();
                                                }
                                                else
                                                {
                                                    currentAiDemon.Walk();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Debug.Log(ResourceType.whiteSoul + " Already exist on table");
                                        }
                                        if (!currentAiDemon.AssignedBuilding.GetComponent<Building>().BlueVioletSoulOnTableExist)
                                        {
                                            if (BlueVioletSoulStockage.Count > 0)//if they are actual stock in the game, if no building around then idle at home
                                            {
                                                if (currentAiDemon.CheckIfCurrentStockInStockIsMoreThanZero(ResourceType.blueVioletSoul)) //is there a stock to take from ? more than 0 
                                                {
                                                    GameObject blueVioletSoulStockage = currentAiDemon.FindClosestResourceSupply(ResourceType.blueVioletSoul); //check for stockage
                                                                                                                                                               //activate to show where you are taking it from

                                                    currentAiDemon.AssignedBuilding.GetComponent<Building>().BuildingLinkedToGenerateFlow_Input02 = blueVioletSoulStockage;

                                                    if (currentAiDemon.checkIfGivenObjectIscloseBy(blueVioletSoulStockage))//am i close to closest stockage 
                                                    {
                                                        currentAiDemon.TakeFromStockpile();
                                                        Debug.Log("Taking from stockpile " + ResourceType.blueVioletSoul);
                                                    }
                                                    else
                                                    {
                                                        currentAiDemon.Walk();
                                                    }
                                                }
                                                else //otherwise idle
                                                {
                                                     if (currentAiDemon.checkIfGivenObjectIscloseBy(currentAiDemon.AssignedBuilding))
                                                    {
                                                        currentAiDemon.Idle();
                                                    }
                                                    else
                                                    {
                                                        currentAiDemon.Walk();
                                                    }
                                                }
                                            }
                                            else //otherwise idle
                                            {
                                                Debug.Log("No more " + ResourceType.blueVioletSoul + " in stockpile");

                                                if (currentAiDemon.checkIfGivenObjectIscloseBy(currentAiDemon.AssignedBuilding))
                                                {
                                                    currentAiDemon.Idle();
                                                }
                                                else
                                                {
                                                    currentAiDemon.Walk();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Debug.Log(ResourceType.blueVioletSoul + " Already exist on table");
                                        }
                                    }
                                    else if (currentAiDemon.AssignedBuilding.GetComponent<Building>().resourceProducedAtBuilding == ResourceType.blueSoul)
                                    {
                                        if (!currentAiDemon.AssignedBuilding.GetComponent<Building>().WhiteSoulOnTableExist)
                                        {
                                            if (WhiteSoulStockage.Count > 0)//if they are actual stock in the game, if no building around then idle at home
                                            {
                                                if (currentAiDemon.CheckIfCurrentStockInStockIsMoreThanZero(ResourceType.whiteSoul)) //is there a stock to take from ? more than 0 
                                                {
                                                    GameObject whiteSoulStockage = currentAiDemon.FindClosestResourceSupply(ResourceType.whiteSoul); //check for stockage
                                                                                                                                                     //activate to show where you are taking it from
                                                    currentAiDemon.AssignedBuilding.GetComponent<Building>().BuildingLinkedToGenerateFlow_Input01 = whiteSoulStockage;

                                                    if (currentAiDemon.checkIfGivenObjectIscloseBy(whiteSoulStockage))//am i close to closest stockage 
                                                    {
                                                        currentAiDemon.TakeFromStockpile();
                                                        Debug.Log("Taking from stockpile " + ResourceType.whiteSoul);
                                                    }
                                                    else
                                                    {
                                                        currentAiDemon.Walk();
                                                    }
                                                }
                                                else //otherwise idle
                                                {
                                                    if (currentAiDemon.checkIfGivenObjectIscloseBy(currentAiDemon.AssignedBuilding))
                                                    {
                                                        currentAiDemon.Idle();
                                                    }
                                                    else
                                                    {
                                                        currentAiDemon.Walk();
                                                    }
                                                }
                                            }
                                            else //otherwise idle
                                            {
                                                Debug.Log("No more " + ResourceType.whiteSoul + " in stockpile");

                                                if (currentAiDemon.checkIfGivenObjectIscloseBy(currentAiDemon.AssignedBuilding))
                                                {
                                                    currentAiDemon.Idle();
                                                }
                                                else
                                                {
                                                    currentAiDemon.Walk();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Debug.Log(ResourceType.whiteSoul + " Already exist on table");
                                        }
                                        if (!currentAiDemon.AssignedBuilding.GetComponent<Building>().VioletSoulOnTableExist)
                                        {
                                            if (VioletSoulStorage.Count > 0)//if they are actual stock in the game, if no building around then idle at home
                                            {
                                                if (currentAiDemon.CheckIfCurrentStockInStockIsMoreThanZero(ResourceType.violetSoul)) //is there a stock to take from ? more than 0 
                                                {
                                                    GameObject VioletSoulStockage = currentAiDemon.FindClosestResourceSupply(ResourceType.violetSoul); //check for stockage
                                                                                                                                                               //activate to show where you are taking it from

                                                    currentAiDemon.AssignedBuilding.GetComponent<Building>().BuildingLinkedToGenerateFlow_Input02 = VioletSoulStockage;

                                                    if (currentAiDemon.checkIfGivenObjectIscloseBy(VioletSoulStockage))//am i close to closest stockage 
                                                    {
                                                        currentAiDemon.TakeFromStockpile();
                                                        Debug.Log("Taking from stockpile " + ResourceType.violetSoul);
                                                    }
                                                    else
                                                    {
                                                        currentAiDemon.Walk();
                                                    }
                                                }
                                                else //otherwise idle
                                                {
                                                    if (currentAiDemon.checkIfGivenObjectIscloseBy(currentAiDemon.AssignedBuilding))
                                                    {
                                                        currentAiDemon.Idle();
                                                    }
                                                    else
                                                    {
                                                        currentAiDemon.Walk();
                                                    }
                                                }
                                            }
                                            else //otherwise idle
                                            {
                                                Debug.Log("No more " + ResourceType.violetSoul + " in stockpile");

                                                if (currentAiDemon.checkIfGivenObjectIscloseBy(currentAiDemon.AssignedBuilding))
                                                {
                                                    currentAiDemon.Idle();
                                                }
                                                else
                                                {
                                                    currentAiDemon.Walk();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Debug.Log(ResourceType.violetSoul + " Already exist on table");
                                        }
                                    }
                                    else if (currentAiDemon.AssignedBuilding.GetComponent<Building>().resourceProducedAtBuilding == ResourceType.redSoul)
                                    {
                                        if (!currentAiDemon.AssignedBuilding.GetComponent<Building>().BlueVioletSoulOnTableExist)
                                        {
                                            if (BlueVioletSoulStockage.Count > 0)//if they are actual stock in the game, if no building around then idle at home
                                            {
                                                if (currentAiDemon.CheckIfCurrentStockInStockIsMoreThanZero(ResourceType.blueVioletSoul)) //is there a stock to take from ? more than 0 
                                                {
                                                    GameObject blueVioletSoulStockage = currentAiDemon.FindClosestResourceSupply(ResourceType.blueVioletSoul); //check for stockage
                                                                                                                                                     //activate to show where you are taking it from
                                                    currentAiDemon.AssignedBuilding.GetComponent<Building>().BuildingLinkedToGenerateFlow_Input01 = blueVioletSoulStockage;

                                                    if (currentAiDemon.checkIfGivenObjectIscloseBy(blueVioletSoulStockage))//am i close to closest stockage 
                                                    {
                                                        currentAiDemon.TakeFromStockpile();
                                                        Debug.Log("Taking from stockpile " + ResourceType.blueVioletSoul);
                                                    }
                                                    else
                                                    {
                                                        currentAiDemon.Walk();
                                                    }
                                                }
                                                else //otherwise idle
                                                {
                                                    if (currentAiDemon.checkIfGivenObjectIscloseBy(currentAiDemon.AssignedBuilding))
                                                    {
                                                        currentAiDemon.Idle();
                                                    }
                                                    else
                                                    {
                                                        currentAiDemon.Walk();
                                                    }
                                                }
                                            }
                                            else //otherwise idle
                                            {
                                                Debug.Log("No more " + ResourceType.blueVioletSoul + " in stockpile");

                                                if (currentAiDemon.checkIfGivenObjectIscloseBy(currentAiDemon.AssignedBuilding))
                                                {
                                                    currentAiDemon.Idle();
                                                }
                                                else
                                                {
                                                    currentAiDemon.Walk();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Debug.Log(ResourceType.blueVioletSoul + " Already exist on table");
                                        }
                                        if (!currentAiDemon.AssignedBuilding.GetComponent<Building>().VioletSoulOnTableExist)
                                        {
                                            if (BlueVioletSoulStockage.Count > 0)//if they are actual stock in the game, if no building around then idle at home
                                            {
                                                if (currentAiDemon.CheckIfCurrentStockInStockIsMoreThanZero(ResourceType.violetSoul)) //is there a stock to take from ? more than 0 
                                                {
                                                    GameObject VioletSoulStockage = currentAiDemon.FindClosestResourceSupply(ResourceType.violetSoul); //check for stockage
                                                                                                                                                       //activate to show where you are taking it from

                                                    currentAiDemon.AssignedBuilding.GetComponent<Building>().BuildingLinkedToGenerateFlow_Input02 = VioletSoulStockage;

                                                    if (currentAiDemon.checkIfGivenObjectIscloseBy(VioletSoulStockage))//am i close to closest stockage 
                                                    {
                                                        currentAiDemon.TakeFromStockpile();
                                                        Debug.Log("Taking from stockpile " + ResourceType.violetSoul);
                                                    }
                                                    else
                                                    {
                                                        currentAiDemon.Walk();
                                                    }
                                                }
                                                else //otherwise idle
                                                {
                                                    if (currentAiDemon.checkIfGivenObjectIscloseBy(currentAiDemon.AssignedBuilding))
                                                    {
                                                        currentAiDemon.Idle();
                                                    }
                                                    else
                                                    {
                                                        currentAiDemon.Walk();
                                                    }
                                                }
                                            }
                                            else //otherwise idle
                                            {
                                                Debug.Log("No more " + ResourceType.violetSoul + " in stockpile");

                                                if (currentAiDemon.checkIfGivenObjectIscloseBy(currentAiDemon.AssignedBuilding))
                                                {
                                                    currentAiDemon.Idle();
                                                }
                                                else
                                                {
                                                    currentAiDemon.Walk();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Debug.Log(ResourceType.violetSoul + " Already exist on table");
                                        }
                                    }
                                }
                            }
                        }
                        break;
                }

                if (currentAiDemon.amIInFire)
                {
                    currentAiDemon.life -= GameSettings.fireSpell.DamageToDemons;
                }

                currentAiDemon.UiHealth.life = currentAiDemon.life;
                currentAiDemon.UiHealth.maxLife = GameSettings.demonLife;
            }
            else
            {
                currentAiDemon.Die();
                Demons.Remove(Demons[i]);
            }
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine(SlowUpdate());
    }

    //debug
    public void HideBuildings()
    {
        AreBuildingShowned = !AreBuildingShowned;

        for (int i = 0; i < Buildings.Count; i++)
        {
            Buildings[i].SetActive(AreBuildingShowned);
        }
    }
}
