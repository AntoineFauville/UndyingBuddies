﻿using System.Collections;
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
                        }
                        else
                        {
                            if (currentAiDemon.AssignedBuilding.GetComponentInParent<Building>().WhatsBeenWorkedOnTheTableExist)
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
                                        else if (currentAiDemon.AssignedBuilding.GetComponentInParent<Building>().resourceProducedAtBuilding == ResourceType.blueVioletSoul)
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
                                    }
                                }
                            }
                            else
                            {
                                if (currentAiDemon.SoulAmount >= 3)
                                {
                                    if (currentAiDemon.checkIfGivenObjectIscloseBy(currentAiDemon.AssignedBuilding))
                                    {
                                        currentAiDemon.PlaceOnTable();
                                    }
                                    else
                                    {
                                        currentAiDemon.Walk();
                                    }
                                }
                                else
                                {
                                    //define in two part one for taking in storage one for taking in the wild
                                    if (currentAiDemon.AssignedBuilding.GetComponent<Building>().resourceProducedAtBuilding == ResourceType.whiteSoul)
                                    {
                                        if (ResourceToProcess.Count > 0)//if they are resource in the nature
                                        {
                                            GameObject whiteSoul = currentAiDemon.FindClosestResourceSupply(ResourceType.whiteSoul);
                                            if (currentAiDemon.checkIfGivenObjectIscloseBy(whiteSoul))
                                            {
                                                currentAiDemon.Gather(ResourceType.whiteSoul);
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
                                            }
                                            else
                                            {
                                                currentAiDemon.Walk();
                                            }
                                        }
                                    }
                                    else if(currentAiDemon.AssignedBuilding.GetComponent<Building>().resourceProducedAtBuilding == ResourceType.blueVioletSoul)
                                    {
                                        if (WhiteSoulStockage.Count > 0)//if they are actual stock in the game, if no building around then idle at home
                                        {
                                            if (currentAiDemon.CheckIfCurrentStockInStockIsMoreThanZero(ResourceType.whiteSoul)) //is there a stock to take from ? more than 0 
                                            {
                                                GameObject blueVioletSoulStockage = currentAiDemon.FindClosestResourceSupply(ResourceType.blueVioletSoul); //check for stockage
                                                //activate to show where you are taking it from

                                                if (blueVioletSoulStockage.GetComponent<Building>().currentStockage > 0)
                                                {
                                                    blueVioletSoulStockage.GetComponent<Building>().visuFlowParticle.EndingPoint = currentAiDemon.AssignedBuilding.GetComponent<Building>().EndVisuStockPileFlow;
                                                    blueVioletSoulStockage.GetComponent<Building>().visuFlowParticle.active = true;
                                                }
                                                else
                                                {
                                                    blueVioletSoulStockage.GetComponent<Building>().visuFlowParticle.active = false;
                                                    blueVioletSoulStockage.GetComponent<Building>().visuFlowParticle.EndingPoint = null;
                                                }

                                                if (currentAiDemon.checkIfGivenObjectIscloseBy(blueVioletSoulStockage))//am i close to closest stockage 
                                                {
                                                    currentAiDemon.TakeFromStockpile();
                                                }
                                                else
                                                {
                                                    currentAiDemon.Walk();
                                                }
                                            }
                                            else //otherwise idle
                                            {
                                                //deactivate all to make sure none are going in case
                                                for (int a = 0; a < Buildings.Count; a++)
                                                {
                                                    Buildings[a].GetComponent<Building>().visuFlowParticle.active = false;
                                                    Buildings[a].GetComponent<Building>().visuFlowParticle.EndingPoint = null;
                                                }

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
