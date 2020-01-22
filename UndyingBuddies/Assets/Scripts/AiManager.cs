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
    public List<GameObject> Woods = new List<GameObject>();
    public List<GameObject> Foods = new List<GameObject>();

    public List<GameObject> Buildables = new List<GameObject>();
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
        AddAllTheBuildable();
        StartCoroutine(SlowUpdate());
    }

    public void AddAllTheBuildable()
    {
        foreach (var buildable in GameObject.FindGameObjectsWithTag("buildable"))
        {
            Buildables.Add(buildable);
        }
    }

    public void AddAllTheInitialResource()
    {
        foreach (var wood in GameObject.FindGameObjectsWithTag("wood"))
        {
            Woods.Add(wood);
        }

        foreach (var food in GameObject.FindGameObjectsWithTag("food"))
        {
            Foods.Add(food);
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
            Woods.Add(resourceGameObject);
        }
        else if (resourceGameObject.GetComponent<Resource>().resourceType == ResourceType.food)
        {
            Foods.Add(resourceGameObject);
        }
    }

    void RemoveResource(GameObject resourceGameObject)
    {
        if (resourceGameObject.GetComponent<Resource>() == null)
        {
            Debug.Log("can't do anything about this resource");
        }

        if (resourceGameObject.GetComponent<Resource>().resourceType == ResourceType.wood && Woods.Contains(resourceGameObject))
        {
            Woods.Remove(resourceGameObject);
        }
        else if (resourceGameObject.GetComponent<Resource>().resourceType == ResourceType.food && Foods.Contains(resourceGameObject))
        {
            Foods.Remove(resourceGameObject);
        }
    }

    void AddDemons()
    {
        foreach (var demon in GameObject.FindGameObjectsWithTag("demon"))
        {
            Demons.Add(demon);
            demon.name = "demon_" + nameID;
            nameID++;

            demon.GetComponent<AIDemons>().Setup(demon.name, JobType.builder, GameSettings.demonLife, GameSettings.demonRangeOfDetection, GameSettings.demonRangeOfCloseBy);
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

    IEnumerator SlowUpdate()
    {
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
                    case JobType.builder:
                        if (Buildables.Count == 0)
                        {
                            if (currentAiDemon.AssignedBuilding != null) //in case you don't have a city hall, this would be null since you haven't assigned the demons to a building yet
                            {
                                if (currentAiDemon.checkIfGivenObjectIscloseBy(currentAiDemon.AssignedBuilding)) // if nothing is to be build check after the city hall and idle there
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
                                currentAiDemon.Idle();
                            }
                        }
                        else
                        {
                            if (currentAiDemon.CheckForClosestBuildingToBuild())
                            {
                                currentAiDemon.Build();
                            }
                            else
                            {
                                currentAiDemon.Walk();
                            }
                        }
                        break;

                    case JobType.collectFood:
                        if (Foods.Count == 0)
                        {
                            currentAiDemon.Idle();
                        }
                        else
                        {
                            if (currentAiDemon.foodAmount <= GameSettings.maxFoodCanCarry) //do i have food on me //no
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
                                    currentAiDemon.Place();
                                }
                                else
                                {
                                    currentAiDemon.Walk();
                                }
                            }
                        }
                        break;

                    case JobType.collectWood:
                        if (Woods.Count == 0)
                        {
                            currentAiDemon.Idle();
                        }
                        else
                        {
                            if (currentAiDemon.woodAmount <= GameSettings.maxWoodCanCarry) //do i have wood on me //no
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
                            else // yes
                            {
                                if (currentAiDemon.checkIfGivenObjectIscloseBy(currentAiDemon.AssignedBuilding))
                                {
                                    currentAiDemon.Place();
                                }
                                else
                                {
                                    currentAiDemon.Walk();
                                }
                            }
                        }
                        break;

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
                        if (currentAiDemon.AssignedBuilding.GetComponent<jobSwitcher>().Building.StockPile.Count > 0)
                        {
                            Debug.Log("Stockpile : " + currentAiDemon.AssignedBuilding.GetComponent<jobSwitcher>().Building.StockPile.Count);
                            currentAiDemon.Process();
                        }
                        else
                        {
                            currentAiDemon.Idle();
                        }
                        break;

                    case JobType.woodCutter:
                        if (currentAiDemon.AssignedBuilding.GetComponent<jobSwitcher>().Building.StockPile.Count > 0)
                        {
                            Debug.Log("Stockpile : " + currentAiDemon.AssignedBuilding.GetComponent<jobSwitcher>().Building.StockPile.Count);
                            currentAiDemon.Process();
                        }
                        else
                        {
                            currentAiDemon.Idle();
                        }
                        break;
                }
            }
            else
            {
                currentAiDemon.Die();
                Demons.Remove(Demons[i]);
            }
        }

        yield return new WaitForSeconds(0.7f);
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
