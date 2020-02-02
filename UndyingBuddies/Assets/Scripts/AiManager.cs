using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiManager : MonoBehaviour
{
    public GameSettings GameSettings;

    //all the AI
    public List<GameObject> Demons = new List<GameObject>();

    //All the AI and buildings!!
    public List<GameObject> Priest = new List<GameObject>();

    //all the wood supply around
    public List<GameObject> WoodToProcess = new List<GameObject>();
    public List<GameObject> FoodToProcess = new List<GameObject>();

    public List<GameObject> FoodStockageBuilding = new List<GameObject>();
    public List<GameObject> WoodStockageBuilding = new List<GameObject>();

    public List<GameObject> Buildings = new List<GameObject>();
    public bool AreBuildingShowned;

    public GameObject FlagToFollow;
    int nameID;

    //enemy
    public List<GameObject> AIPriest = new List<GameObject>();
    public List<GameObject> AIPriestBuildings = new List<GameObject>();

    void Start()
    {
        if (FlagToFollow == null)
        {
            GameObject.Find("FlagToFollow");
        }

        AddDemons();
        AddPriest();
        AddAllTheInitialResource();
        StartCoroutine(SlowUpdate());
    }

    public void AddAllTheInitialResource()
    {
        foreach (var wood in GameObject.FindGameObjectsWithTag("wood"))
        {
            if (!wood.GetComponent<Resource>().processedResource)
            {
                WoodToProcess.Add(wood);
            }
        }

        foreach (var food in GameObject.FindGameObjectsWithTag("food"))
        {
            if (!food.GetComponent<Resource>().processedResource)
            {
                FoodToProcess.Add(food);

            }
        }
    }

    public void AddResource(GameObject resourceGameObject)
    {
        if (resourceGameObject.GetComponent<Resource>() == null)
        {
            Debug.Log("can't do anything about this resource");
        }

        if (resourceGameObject.GetComponent<Resource>().resourceType == ResourceType.wood)
        {
            WoodToProcess.Add(resourceGameObject);
        }
        else if (resourceGameObject.GetComponent<Resource>().resourceType == ResourceType.food)
        {
            FoodToProcess.Add(resourceGameObject);
        }
    }

    void RemoveResource(GameObject resourceGameObject)
    {
        if (resourceGameObject.GetComponent<Resource>() == null)
        {
            Debug.Log("can't do anything about this resource");
        }

        if (resourceGameObject.GetComponent<Resource>().resourceType == ResourceType.wood && WoodToProcess.Contains(resourceGameObject))
        {
            WoodToProcess.Remove(resourceGameObject);
        }
        else if (resourceGameObject.GetComponent<Resource>().resourceType == ResourceType.food && FoodToProcess.Contains(resourceGameObject))
        {
            FoodToProcess.Remove(resourceGameObject);
        }
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

    void AddPriest()
    {
        //foreach (var priest in GameObject.FindGameObjectsWithTag("priest"))
        //{
        //    Priest.Add(priest);
        //    priest.name = "priest_" + nameID;
        //    nameID++;
        //}
    }

    void CleanTheListFromEmptyObjects()
    {
        for (int i = 0; i < FoodStockageBuilding.Count; i++)
        {
            if (FoodStockageBuilding[i] == null)
            {
                FoodStockageBuilding.Remove(FoodStockageBuilding[i]);
            }
        }

        for (int i = 0; i < WoodStockageBuilding.Count; i++)
        {
            if (WoodStockageBuilding[i] == null)
            {
                WoodStockageBuilding.Remove(WoodStockageBuilding[i]);
            }
        }

        for (int i = 0; i < FoodToProcess.Count; i++)
        {
            if (FoodToProcess[i] == null)
            {
                FoodToProcess.Remove(FoodToProcess[i]);
            }
        }

        for (int i = 0; i < WoodToProcess.Count; i++)
        {
            if (WoodToProcess[i] == null)
            {
                WoodToProcess.Remove(WoodToProcess[i]);
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
                Priest.Remove(Demons[i]);
            }
        }
    }

    IEnumerator SlowUpdate()
    {
        CleanTheListFromEmptyObjects();

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
                    case JobType.followFlag:
                        if (Priest.Count <= 0)
                        {
                            if (FlagToFollow == null)
                            {
                                FlagToFollow = GameObject.FindGameObjectWithTag("flag");
                            }

                            if (currentAiDemon.checkIfGivenObjectIscloseBy(FlagToFollow))
                            {
                                currentAiDemon.Idle();
                            }
                            else
                            {
                                currentAiDemon.Walk();
                            }
                        }
                        else
                        {
                            if (currentAiDemon.CheckIfAnythingWithPriestNearBy())
                            {
                                if (currentAiDemon.CheckIfPriestIsCloseToAttack())
                                {
                                    currentAiDemon.Attack();
                                }
                                else
                                {
                                    currentAiDemon.Walk();
                                }
                            }
                            else
                            {
                                if (FlagToFollow == null)
                                {
                                    FlagToFollow = GameObject.FindGameObjectWithTag("flag");
                                }

                                if (currentAiDemon.checkIfGivenObjectIscloseBy(FlagToFollow))
                                {
                                    currentAiDemon.Idle();
                                }
                                else
                                {
                                    currentAiDemon.Walk();
                                }
                            }
                        }
                        break;

                    case JobType.foodProcessor:
                        if (currentAiDemon.AssignedBuilding.GetComponent<jobSwitcher>().Building.WhatsBeenWorkedOnTheTableExist)
                        {
                            if (currentAiDemon.AssignedBuilding.GetComponent<jobSwitcher>().Building.WorkedOnTableBeenProcessed)
                            {
                                if (currentAiDemon.BerryBasketAmount > 0)
                                {
                                    if (currentAiDemon.CheckIfThereIsStockageAvailable())
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
                                        Debug.Log("No More Stockage Units");
                                        currentAiDemon.Idle();
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
                                    currentAiDemon.AssignedBuilding.GetComponent<jobSwitcher>().Building.WorkedOnTableBeenProcessed = true;
                                }
                                else
                                {
                                    currentAiDemon.Walk();
                                }
                            }
                        }
                        else
                        {
                            if (currentAiDemon.BushAmount >= 3)
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
                                if (FoodToProcess.Count > 0)
                                {
                                    GameObject food = currentAiDemon.FindClosestResourceSupply(ResourceType.food);
                                    if (currentAiDemon.checkIfGivenObjectIscloseBy(food))
                                    {
                                        currentAiDemon.Gather(ResourceType.food);
                                    }
                                    else
                                    {
                                        currentAiDemon.Walk();
                                    }
                                }
                                else
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
                        break;

                    case JobType.woodProcessor:
                        if (currentAiDemon.AssignedBuilding.GetComponent<jobSwitcher>().Building.WhatsBeenWorkedOnTheTableExist)
                        {
                            if (currentAiDemon.AssignedBuilding.GetComponent<jobSwitcher>().Building.WorkedOnTableBeenProcessed)
                            {
                                if (currentAiDemon.PlankAmount > 0)
                                {
                                    if (currentAiDemon.CheckIfThereIsStockageAvailable())
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
                                        Debug.Log("No More Stockage Units");
                                        currentAiDemon.Idle();
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
                                    currentAiDemon.AssignedBuilding.GetComponent<jobSwitcher>().Building.WorkedOnTableBeenProcessed = true;
                                }
                                else
                                {
                                    currentAiDemon.Walk();
                                }
                            }
                        }
                        else
                        {
                            if (currentAiDemon.LogAmount >= 3)
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
                                if (WoodToProcess.Count > 0)
                                {
                                    GameObject wood = currentAiDemon.FindClosestResourceSupply(ResourceType.wood);
                                    if (currentAiDemon.checkIfGivenObjectIscloseBy(wood))
                                    {
                                        currentAiDemon.Gather(ResourceType.wood);
                                    }
                                    else
                                    {
                                        currentAiDemon.Walk();
                                    }
                                }
                                else
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
                        break;

                    case JobType.energyProcessor:
                        if (currentAiDemon.checkIfGivenObjectIscloseBy(currentAiDemon.AssignedBuilding))
                        {
                            currentAiDemon.Prey();
                        }
                        else
                        {
                            currentAiDemon.Walk();
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
