using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIDemons : MonoBehaviour
{
    public string myName;
    public JobType JobType;
    public int life;
    public UiHealth UiHealth;
    private int _demonRangeOfDetection;
    private int _demonRangeOfCloseBy;

    public NavMeshAgent NavMeshAgent;

    public GameObject TargetToGoTo;

    public int LogAmount;
    public int MaxLog = 9;
    public int BushAmount;
    public int MaxBush = 3;
    public int PlankAmount;
    public int BerryBasketAmount;
    public GameObject AssignedBuilding;

    bool AbleToPerformAction;

    Animator animatorDemon;

    public bool amIInFire;

    //visuals
    public GameObject Wagon;
    public GameObject[] BushObject;
    public GameObject[] BerryContainerObject;
    public GameObject[] LogObject;
    public GameObject[] PlankObject;
    public bool runCoroutineBerryOnce;

    public void Setup(string name, JobType initialJobtype, int initiallife, int demonRangeOfDetection, int demonRangeOfAttack)
    {
        myName = name;
        SwitchJob(initialJobtype);
        life = initiallife;
        _demonRangeOfDetection = demonRangeOfDetection;
        _demonRangeOfCloseBy = demonRangeOfAttack;

        animatorDemon = this.GetComponent<Animator>();

        if (Wagon != null)
        {
            Wagon.SetActive(false);
        }
    }

    public void SwitchJob(JobType newJobtype)
    {
        JobType = newJobtype;
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.tag == "FireZone")
        {
            amIInFire = true;
        }
        else
        {
            amIInFire = false;
        }
    }

    void OnTriggerExit()
    {
        amIInFire = false;
    }

    public void ResetVisuals()
    {
        for (int i = 0; i < BushObject.Length; i++)
        {
            BushObject[i].SetActive(false);
        }
        for (int i = 0; i < LogObject.Length; i++)
        {
            LogObject[i].SetActive(false);
        }
        if (Wagon != null)
        {
            Wagon.SetActive(false);
        }
        for (int i = 0; i < BerryContainerObject.Length; i++)
        {
            BerryContainerObject[i].SetActive(false);
        }
        for (int i = PlankObject.Length - 1; i > -1; i--)
        {
            PlankObject[i].SetActive(false);
        }
    }

    //actions
    public void Attack()
    {
        //Debug.Log("attack");
        animatorDemon.Play("Attack");
        NavMeshAgent.isStopped = true;

        if (!AbleToPerformAction)
        {
            AbleToPerformAction = true;

            if (TargetToGoTo.GetComponent<AIPriest>() != null)
            {
                TargetToGoTo.GetComponent<AIPriest>().healthAmount -= GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.damageDemons;
            }

            StartCoroutine(TransferingTime());
        }
    }

    public void Die()
    {
        //Debug.Log("die");
        animatorDemon.Play("Die");
        NavMeshAgent.isStopped = true;
        NavMeshAgent.enabled = false;
        StartCoroutine(WaitToDie());
    }

    public void Walk()
    {
        //Debug.Log("walk");
        animatorDemon.Play("Walk");

        if (Vector3.Distance(TargetToGoTo.transform.position, this.transform.position) > 4)
        {
            NavMeshAgent.isStopped = false;
            NavMeshAgent.SetDestination(TargetToGoTo.transform.position);
        }
        else
        {
            NavMeshAgent.isStopped = true;
        }
    }

    public void Process()
    {
        NavMeshAgent.isStopped = true;
        animatorDemon.Play("Gather");

        if (AssignedBuilding.GetComponentInParent<Building>().BuildingType == BuildingType.WoodProcessor || AssignedBuilding.GetComponentInParent<Building>().BuildingType == BuildingType.FoodProcessor)
        {
            this.gameObject.transform.position = AssignedBuilding.GetComponentInParent<Building>().EmplacementWorker.transform.position;
            this.gameObject.transform.rotation = AssignedBuilding.GetComponentInParent<Building>().EmplacementWorker.transform.rotation;

            AssignedBuilding.GetComponentInParent<Building>().visualsOnTable.SetActive(true);
        }
    }

    public void Gather(ResourceType resourceToGather)
    {   
        //Debug.Log("gather");
        animatorDemon.Play("Gather");

        NavMeshAgent.isStopped = true;

        if (Wagon != null)
        {
            Wagon.SetActive(true);
        }

        if (!AbleToPerformAction && !TargetToGoTo.GetComponent<Resource>().processedResource)
        {
            AbleToPerformAction = true;
            
            if (resourceToGather == ResourceType.wood)
            {
                if (TargetToGoTo.GetComponent<Resource>().amountOfResourceAvailable > 0)
                {
                    if (LogAmount < MaxLog)
                    {
                        TargetToGoTo.GetComponent<Resource>().amountOfResourceAvailable -= 1;
                        LogAmount += 1;
                        LogObject[LogAmount - 1].SetActive(true);
                    }
                }
            }
            else if (resourceToGather == ResourceType.food)
            {
                if (TargetToGoTo.GetComponent<Resource>().amountOfResourceAvailable > 0)
                {
                    if (BushAmount < MaxBush)
                    {
                        TargetToGoTo.GetComponent<Resource>().amountOfResourceAvailable -= 1;
                        BushAmount += 1;
                        BushObject[BushAmount-1].SetActive(true);
                    }
                }
            }

            StartCoroutine(TransferingTime());
        }
    }

    public void Idle()
    {
        //Debug.Log("idle");
        animatorDemon.Play("Idle");
        NavMeshAgent.isStopped = true;
    }

    public void PlaceOnTable()
    {
        animatorDemon.Play("Place");
        NavMeshAgent.isStopped = true;

        StartCoroutine(PlaceOnTableWait());
    }

    public void TakeFromTable()
    {
        animatorDemon.Play("Place");
        NavMeshAgent.isStopped = true;
        
        if (Wagon != null)
        {
            Wagon.SetActive(true);
        }

        if (JobType == JobType.foodProcessor)
        {
            BerryBasketAmount = 1;

            for (int i = 0; i < BerryContainerObject.Length; i++)
            {
                BerryContainerObject[i].SetActive(true);
            }
        }
        else if (JobType == JobType.woodProcessor)
        {
            PlankAmount = 1;

            for (int i = 0; i < PlankObject.Length; i++)
            {
                PlankObject[i].SetActive(true);
            }
        }

        if (AssignedBuilding.GetComponentInParent<Building>().visualsOnTable != null)
        {
            AssignedBuilding.GetComponentInParent<Building>().visualsOnTable.SetActive(false);
        }
    }
    
    public void PlaceInStockpile()
    {
        //Debug.Log("place");
        animatorDemon.Play("Place");
        NavMeshAgent.isStopped = true;

        if(!runCoroutineBerryOnce)
            StartCoroutine(PlaceInStockpileWait());
    }
    
    public bool CheckIfAnythingWithPriestNearBy()
    {
        bool check = false;

        GameObject bestPriest = null;

        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        List<GameObject> listToCheck;

        listToCheck = GameObject.Find("Main Camera").GetComponent<AiManager>().Priest;

        foreach (GameObject potentialTarget in listToCheck)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestPriest = potentialTarget;
            }
        }

        TargetToGoTo = bestPriest;

        if (Vector3.Distance(this.transform.position, TargetToGoTo.transform.position) < _demonRangeOfDetection)
        {
            check = true;
        }
        else
        {
            check = false;
        }
        
        return check;
    } //check if there is an enemy in the vision range

    public bool CheckIfPriestIsCloseToAttack()
    {
        bool check = false;

        GameObject bestPriest = null;

        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        List<GameObject> listToCheck;

        listToCheck = GameObject.Find("Main Camera").GetComponent<AiManager>().Priest;

        foreach (GameObject potentialTarget in listToCheck)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestPriest = potentialTarget;
            }
        }

        TargetToGoTo = bestPriest;

        if (Vector3.Distance(this.transform.position, TargetToGoTo.transform.position) < _demonRangeOfCloseBy)
        {
            check = true;
        }
        else
        {
            check = false;
        }

        return check;
    } // check if there is an enemy to attack close up

    public bool checkIfGivenObjectIscloseBy(GameObject gameObject)
    {
        bool check = false;

        TargetToGoTo = gameObject;

        //Debug.Log(Vector3.Distance(this.transform.position, gameObject.transform.position));

        if (Vector3.Distance(this.transform.position, gameObject.transform.position) < _demonRangeOfCloseBy)
        {
            check = true;
        }
        else
        {
            check = false;
        }

        return check;
    } // check if there is an enemy to attack close up

    public GameObject FindClosestResourceSupply(ResourceType resourceType)
    {
        GameObject closestWoodSupply = null;

        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        List<GameObject> listToCheck;

        listToCheck = GameObject.Find("Main Camera").GetComponent<AiManager>().WoodToProcess;

        if (resourceType == ResourceType.wood)
            listToCheck = GameObject.Find("Main Camera").GetComponent<AiManager>().WoodToProcess;
        else if (resourceType == ResourceType.food)
            listToCheck = GameObject.Find("Main Camera").GetComponent<AiManager>().FoodToProcess;

        foreach (GameObject potentialTarget in listToCheck)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                closestWoodSupply = potentialTarget;
            }
        }

        TargetToGoTo = closestWoodSupply;

        return closestWoodSupply;
    } // closest resource

    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    public bool CheckForClosestBuildingToPlaceStockage()
    {
        bool check = false;

        GameObject closestBuilding = null;

        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        List<GameObject> listToCheck;

        if(JobType == JobType.foodProcessor)
            listToCheck = GameObject.Find("Main Camera").GetComponent<AiManager>().FoodStockageBuilding;
        else //wood
            listToCheck = GameObject.Find("Main Camera").GetComponent<AiManager>().WoodStockageBuilding;

        for (int i = 0; i < listToCheck.Count; i++)
        {
            if (listToCheck[i].GetComponent<Building>().currentStockage >= listToCheck[i].GetComponent<Building>().maxStockage)
            {
                listToCheck.Remove(listToCheck[i]);
            }
        }

        foreach (GameObject potentialTarget in listToCheck)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                closestBuilding = potentialTarget;
            }
        }

        TargetToGoTo = closestBuilding;

        if (Vector3.Distance(this.transform.position, TargetToGoTo.transform.position) < _demonRangeOfCloseBy)
        {
            check = true;
        }
        else
        {
            check = false;
        }

        return check;
    } // check if there is an enemy to attack close up

    public bool CheckIfThereIsStockageAvailable()
    {
        bool check = false;

        int stockage = 0;

        List<GameObject> listToCheck;

        if (JobType == JobType.foodProcessor)
            listToCheck = GameObject.Find("Main Camera").GetComponent<AiManager>().FoodStockageBuilding;
        else //wood
            listToCheck = GameObject.Find("Main Camera").GetComponent<AiManager>().WoodStockageBuilding;

        for (int i = 0; i < listToCheck.Count; i++)
        {
            int stock;

            stock = listToCheck[i].GetComponent<Building>().maxStockage - listToCheck[i].GetComponent<Building>().currentStockage;

            stockage += stock;
        }

        if (stockage > 0)
        {
            check = true;
        }
        else
        {
            check = false;
        }

        return check;
    } // check if there is an enemy to attack close up

    IEnumerator TransferingTime()
    {
        yield return new WaitForSeconds(1);

        AbleToPerformAction = false;
    }

    IEnumerator WaitToDie()
    {
        yield return new WaitForSeconds(2);

        DestroyImmediate(this.gameObject);
    }

    IEnumerator PlaceInStockpileWait()
    {
        runCoroutineBerryOnce = true;

        yield return new WaitForSeconds(0.4f);

        if (JobType == JobType.foodProcessor)
        {
            for (int i = 0; i < BerryContainerObject.Length; i++)
            {
                BerryContainerObject[i].SetActive(false);

                if (TargetToGoTo.GetComponent<Building>().currentStockage < TargetToGoTo.GetComponent<Building>().maxStockage)
                {
                    TargetToGoTo.GetComponent<Building>().AddToStockage();
                }
                yield return new WaitForSeconds(0.4f);
            }
        }
        else if(JobType == JobType.woodProcessor)
        {

            for(int i = PlankObject.Length-1; i > -1; i--)
            {
                PlankObject[i].SetActive(false);

                if (TargetToGoTo.GetComponent<Building>().currentStockage < TargetToGoTo.GetComponent<Building>().maxStockage)
                {
                    TargetToGoTo.GetComponent<Building>().AddToStockage();
                }
                yield return new WaitForSeconds(0.4f);
            }
        }
       
        if (Wagon != null)
        {
            Wagon.SetActive(false);
        }

        yield return new WaitForSeconds(0.5f);

        AssignedBuilding.GetComponentInParent<Building>().WhatsBeenWorkedOnTheTableExist = false;
        BerryBasketAmount = 0;
        PlankAmount = 0;

        runCoroutineBerryOnce = false;
    }

    IEnumerator PlaceOnTableWait()
    {
        yield return new WaitForSeconds(0.3f);

        if (JobType == JobType.foodProcessor)
        {
            for (int i = 0; i < BushObject.Length; i++)
            {
                BushObject[i].SetActive(false);
                yield return new WaitForSeconds(0.4f);
            }
        }
        else if (JobType == JobType.woodProcessor)
        {
            for (int i = 0; i < LogObject.Length; i++)
            {
                LogObject[i].SetActive(false);
                yield return new WaitForSeconds(0.4f);
            }
        }

        if (Wagon != null)
        {
            Wagon.SetActive(false);
        }

        yield return new WaitForSeconds(0.5f);

        BushAmount = 0;
        LogAmount = 0;
        AssignedBuilding.GetComponentInParent<Building>().WhatsBeenWorkedOnTheTableExist = true; //there is smthg on the table
        AssignedBuilding.GetComponentInParent<Building>().WorkedOnTableBeenProcessed = false; //but it hasn't been processed yet
    }
}
