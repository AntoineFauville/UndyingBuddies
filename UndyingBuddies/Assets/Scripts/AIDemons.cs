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
    
    public int SoulAmount;
    public int MaxSouls = 3;
    public int SoulBasketAmount;
    public GameObject AssignedBuilding;

    bool AbleToPerformAction;

    Animator animatorDemon;

    public bool amIInFire;

    //visuals
    public GameObject Wagon;
    public GameObject[] SoulObjects;
    public bool runCoroutineSoulsOnce;

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
        for (int i = 0; i < SoulObjects.Length; i++)
        {
            SoulObjects[i].SetActive(false);
        }
        if (Wagon != null)
        {
            Wagon.SetActive(false);
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
        animatorDemon.Play("Prey");

        StartCoroutine(processing());

        if (AssignedBuilding.GetComponentInParent<Building>().BuildingType == BuildingType.Processor)
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
            
            if (resourceToGather == ResourceType.whiteSoul)
            {
                if (TargetToGoTo.GetComponent<Resource>().amountOfResourceAvailable > 0)
                {
                    if (SoulAmount < MaxSouls)
                    {
                        TargetToGoTo.GetComponent<Resource>().amountOfResourceAvailable -= 1;
                        SoulAmount += 1;
                        SoulObjects[SoulAmount-1].SetActive(true);
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

        if (JobType == JobType.processor)
        {
            SoulBasketAmount = 1;

            for (int i = 0; i < SoulObjects.Length; i++)
            {
                SoulObjects[i].SetActive(true);
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

        if(!runCoroutineSoulsOnce)
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
        
        listToCheck = GameObject.Find("Main Camera").GetComponent<AiManager>().ResourceToProcess;

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

        List<GameObject> listToCheck = new List<GameObject>();

        if (JobType == JobType.processor)
        {
            if (GameObject.Find("Main Camera").GetComponent<AiManager>().WhiteSoulStockage.Count > 0)
            {
                for (int i = 0; i < GameObject.Find("Main Camera").GetComponent<AiManager>().WhiteSoulStockage.Count; i++)
                {
                    listToCheck.Add(GameObject.Find("Main Camera").GetComponent<AiManager>().WhiteSoulStockage[i]);
                }
            }
        }

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

       
        listToCheck = GameObject.Find("Main Camera").GetComponent<AiManager>().WhiteSoulStockage;

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
        runCoroutineSoulsOnce = true;

        yield return new WaitForSeconds(0.4f);

        if (JobType == JobType.processor)
        {
            for (int i = 0; i < SoulObjects.Length; i++)
            {
                SoulObjects[i].SetActive(false);

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
        SoulBasketAmount = 0;

        runCoroutineSoulsOnce = false;
    }

    IEnumerator PlaceOnTableWait()
    {
        yield return new WaitForSeconds(0.3f);

        if (JobType == JobType.foodProcessor)
        {
            for (int i = 0; i < SoulObjects.Length; i++)
            {
                SoulObjects[i].SetActive(false);
                yield return new WaitForSeconds(0.4f);
            }
        }

        if (Wagon != null)
        {
            Wagon.SetActive(false);
        }

        yield return new WaitForSeconds(0.5f);

        SoulAmount = 0;
        AssignedBuilding.GetComponentInParent<Building>().WhatsBeenWorkedOnTheTableExist = true; //there is smthg on the table
        AssignedBuilding.GetComponentInParent<Building>().WorkedOnTableBeenProcessed = false; //but it hasn't been processed yet
    }

    IEnumerator processing()
    {
        yield return new WaitForSeconds(3f);

        AssignedBuilding.GetComponentInParent<Building>().WorkedOnTableBeenProcessed = true;
    }
}
