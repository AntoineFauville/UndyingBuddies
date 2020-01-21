using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiManager : MonoBehaviour
{
    [SerializeField] private GameSettings _gameSettings;

    //all the AI
    public List<GameObject> Demons = new List<GameObject>();

    //All the AI and buildings!!
    public List<GameObject> Priest = new List<GameObject>();

    //all the wood supply around
    public List<GameObject> Woods = new List<GameObject>();
    public List<GameObject> Foods = new List<GameObject>();

    public GameObject FlagToFollow;

    int nameID;

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

            demon.GetComponent<AIDemons>().Setup(demon.name, JobType.builder, _gameSettings.demonLife, _gameSettings.demonRangeOfDetection, _gameSettings.demonRangeOfCloseBy);
        }
    }

    void AddPriest()
    {
        foreach (var priest in GameObject.FindGameObjectsWithTag("priest"))
        {
            Priest.Add(priest);
            priest.name = "priest_" + nameID;
            nameID++;
        }
    }

    IEnumerator SlowUpdate()
    {
        foreach (var demon in Demons)
        {
            JobType demonJobType;
            AIDemons currentAiDemon;

            currentAiDemon = demon.GetComponent<AIDemons>();
            demonJobType = currentAiDemon.JobType;

            switch (demonJobType)
            {
                case JobType.builder:
                    break;

                case JobType.collectFood:
                    if (Foods.Count == 0)
                    {
                        currentAiDemon.Idle();
                    }
                    else
                    {
                        if (currentAiDemon.foodAmount <= _gameSettings.maxFoodCanCarry) //do i have food on me //no
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
                        if (currentAiDemon.woodAmount <= _gameSettings.maxWoodCanCarry) //do i have wood on me //no
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
                        if (currentAiDemon.checkIfGivenObjectIscloseBy(FlagToFollow))
                        {
                            currentAiDemon.Idle();
                        }
                        else
                        {
                            currentAiDemon.Walk();
                        }
                    }
                    break;
            }
        }

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(SlowUpdate());
    }
}
