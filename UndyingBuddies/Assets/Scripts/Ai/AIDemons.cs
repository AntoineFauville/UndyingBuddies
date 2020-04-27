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
    public int MaxSouls = 1;
    public int SoulBasketAmount;
    public GameObject AssignedBuilding;

    bool AbleToPerformAction;

    Animator animatorDemon;

    public bool amIInFire;

    //visuals
    public GameObject Wagon;
    public GameObject SoulObjects;
    public bool runCoroutineSoulsOnce;

    public ResourceType resourceImCurrentlyTransporting;

    public bool OnlyProcessOnce;

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
        SoulObjects.SetActive(false);

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
        NavMeshAgent.enabled = true;

        //Debug.Log("walk");
        animatorDemon.Play("Walk");

        if (Vector3.Distance(TargetToGoTo.transform.position, this.transform.position) > 4)
        {
            NavMeshAgent.isStopped = false;
            NavMeshAgent.SetDestination(TargetToGoTo.transform.position);

            Debug.DrawLine(this.transform.position, TargetToGoTo.transform.position, Color.green, 0.4f);
        }
        else
        {
            NavMeshAgent.isStopped = true;
        }
    }

    public void Process()
    {
        if (NavMeshAgent.enabled)
        {
            NavMeshAgent.isStopped = true;
        }
        
        animatorDemon.Play("Prey");

        if (!OnlyProcessOnce)
        {
            OnlyProcessOnce = true;
            StartCoroutine(processing());

            NavMeshAgent.enabled = false;

            this.gameObject.transform.position = AssignedBuilding.GetComponentInParent<Building>().EmplacementWorker.transform.position;
            this.gameObject.transform.rotation = AssignedBuilding.GetComponentInParent<Building>().EmplacementWorker.transform.rotation;

            AssignedBuilding.GetComponentInParent<Building>().visualsOnTable.SetActive(true);
            if (AssignedBuilding.GetComponentInParent<Building>().resourceProducedAtBuilding == ResourceType.whiteSoul)
                AssignedBuilding.GetComponentInParent<Building>().visualsOnTable.GetComponent<SoulColor>().ChangeColor(GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.whiteSoulColor);
            else if (AssignedBuilding.GetComponentInParent<Building>().resourceProducedAtBuilding == ResourceType.blueVioletSoul)
                AssignedBuilding.GetComponentInParent<Building>().visualsOnTable.GetComponent<SoulColor>().ChangeColor(GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.blueVioletColor);
            else if (AssignedBuilding.GetComponentInParent<Building>().resourceProducedAtBuilding == ResourceType.violetSoul)
                AssignedBuilding.GetComponentInParent<Building>().visualsOnTable.GetComponent<SoulColor>().ChangeColor(GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.violetColor);
            else if (AssignedBuilding.GetComponentInParent<Building>().resourceProducedAtBuilding == ResourceType.blueSoul)
                AssignedBuilding.GetComponentInParent<Building>().visualsOnTable.GetComponent<SoulColor>().ChangeColor(GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.blueColor);
            else if (AssignedBuilding.GetComponentInParent<Building>().resourceProducedAtBuilding == ResourceType.redSoul)
                AssignedBuilding.GetComponentInParent<Building>().visualsOnTable.GetComponent<SoulColor>().ChangeColor(GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.redColor);
        }
    }

    public void Gather(ResourceType resourceToGather)
    {   
        //Debug.Log("gather");
        animatorDemon.Play("Gather");
        NavMeshAgent.enabled = true;
        NavMeshAgent.isStopped = true;

        if (Wagon != null)
        {
            Wagon.SetActive(true);
        }

        if (!AbleToPerformAction && !TargetToGoTo.GetComponent<Resource>().processedResource)
        {
            AbleToPerformAction = true;
            
            if (resourceToGather == ResourceType.brokenSoul)
            {
                resourceImCurrentlyTransporting = ResourceType.brokenSoul;

                SoulObjects.GetComponent<SoulColor>().ChangeColor(GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.brokenSoulColor);

                if (TargetToGoTo.GetComponent<Resource>().amountOfResourceAvailable > 0)
                {
                    if (SoulAmount < MaxSouls)
                    {
                        TargetToGoTo.GetComponent<Resource>().amountOfResourceAvailable -= 1;
                        SoulAmount += 1;
                        SoulObjects.SetActive(true);
                    }
                }
            }

            StartCoroutine(TransferingTime());
        }
    }

    public void Idle()
    {
        NavMeshAgent.enabled = true;
        //Debug.Log("idle");
        animatorDemon.Play("Idle");
        NavMeshAgent.isStopped = true;
    }

    public void PlaceOnTable()
    {
        animatorDemon.Play("Place");
        NavMeshAgent.enabled = true;
        NavMeshAgent.isStopped = true;

        StartCoroutine(PlaceOnTableWait());
    }

    public void TakeFromTable()
    {
        animatorDemon.Play("Place");
        NavMeshAgent.enabled = true;
        NavMeshAgent.isStopped = true;
        
        if (Wagon != null)
        {
            Wagon.SetActive(true);
        }

        SoulBasketAmount = 1;
        SoulObjects.SetActive(true);

        if (AssignedBuilding.GetComponentInParent<Building>().visualsOnTable != null)
        {
            AssignedBuilding.GetComponentInParent<Building>().visualsOnTable.SetActive(false);
        }
    }
    
    public void PlaceInStockpile()
    {
        //Debug.Log("place");
        animatorDemon.Play("Place");
        NavMeshAgent.enabled = true;
        NavMeshAgent.isStopped = true;

        if(!runCoroutineSoulsOnce)
            StartCoroutine(PlaceInStockpileWait());
    }

    public void TakeFromStockpile()
    {
        animatorDemon.Play("Place");
        NavMeshAgent.enabled = true;
        NavMeshAgent.isStopped = true;

        if (!runCoroutineSoulsOnce)
            StartCoroutine(TakeFromStockpileWait());
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

        if (gameObject == null)
        {
            check = false;
        }
        else
        {
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
        }

        return check;
    } // check if there is an enemy to attack close up

    public GameObject FindClosestResourceSupply(ResourceType resourceType)
    {
        GameObject closestWoodSupply = null;

        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        List<GameObject> listToCheck = new List<GameObject>();
        
        if (resourceType == ResourceType.brokenSoul)
        {
            for (int i = 0; i < GameObject.Find("Main Camera").GetComponent<AiManager>().ResourceToProcess.Count; i++)
            {
                listToCheck.Add(GameObject.Find("Main Camera").GetComponent<AiManager>().ResourceToProcess[i]);
            }
        } 
        else if (resourceType == ResourceType.whiteSoul) // check for a stock
        {
            for (int i = 0; i < GameObject.Find("Main Camera").GetComponent<AiManager>().WhiteSoulStockage.Count; i++)
            {
                if (GameObject.Find("Main Camera").GetComponent<AiManager>().WhiteSoulStockage[i].GetComponent<Building>().currentStockage > 0)
                {
                    listToCheck.Add(GameObject.Find("Main Camera").GetComponent<AiManager>().WhiteSoulStockage[i]);
                }
            }
        }
        else if (resourceType == ResourceType.blueVioletSoul) // check for a stock
        {
            for (int i = 0; i < GameObject.Find("Main Camera").GetComponent<AiManager>().BlueVioletSoulStockage.Count; i++)
            {
                if (GameObject.Find("Main Camera").GetComponent<AiManager>().BlueVioletSoulStockage[i].GetComponent<Building>().currentStockage > 0)
                {
                    listToCheck.Add(GameObject.Find("Main Camera").GetComponent<AiManager>().BlueVioletSoulStockage[i]);
                }
            }
        }
        else if (resourceType == ResourceType.violetSoul) // check for a stock
        {
            for (int i = 0; i < GameObject.Find("Main Camera").GetComponent<AiManager>().VioletSoulStorage.Count; i++)
            {
                if (GameObject.Find("Main Camera").GetComponent<AiManager>().VioletSoulStorage[i].GetComponent<Building>().currentStockage > 0)
                {
                    listToCheck.Add(GameObject.Find("Main Camera").GetComponent<AiManager>().VioletSoulStorage[i]);
                }
            }
        }

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

        if (AssignedBuilding.GetComponent<Building>().resourceProducedAtBuilding == ResourceType.whiteSoul)
        {
            if (GameObject.Find("Main Camera").GetComponent<AiManager>().WhiteSoulStockage.Count > 0)
            {
                for (int i = 0; i < GameObject.Find("Main Camera").GetComponent<AiManager>().WhiteSoulStockage.Count; i++)
                {
                    listToCheck.Add(GameObject.Find("Main Camera").GetComponent<AiManager>().WhiteSoulStockage[i]);
                }
            }
        }
        else if (AssignedBuilding.GetComponent<Building>().resourceProducedAtBuilding == ResourceType.blueVioletSoul)
        {
            if (GameObject.Find("Main Camera").GetComponent<AiManager>().BlueVioletSoulStockage.Count > 0)
            {
                for (int i = 0; i < GameObject.Find("Main Camera").GetComponent<AiManager>().BlueVioletSoulStockage.Count; i++)
                {
                    listToCheck.Add(GameObject.Find("Main Camera").GetComponent<AiManager>().BlueVioletSoulStockage[i]);
                }
            }
        }
        else if (AssignedBuilding.GetComponent<Building>().resourceProducedAtBuilding == ResourceType.violetSoul)
        {
            if (GameObject.Find("Main Camera").GetComponent<AiManager>().VioletSoulStorage.Count > 0)
            {
                for (int i = 0; i < GameObject.Find("Main Camera").GetComponent<AiManager>().VioletSoulStorage.Count; i++)
                {
                    listToCheck.Add(GameObject.Find("Main Camera").GetComponent<AiManager>().VioletSoulStorage[i]);
                }
            }
        }
        else if (AssignedBuilding.GetComponent<Building>().resourceProducedAtBuilding == ResourceType.blueSoul)
        {
            if (GameObject.Find("Main Camera").GetComponent<AiManager>().BlueSoulStockage.Count > 0)
            {
                for (int i = 0; i < GameObject.Find("Main Camera").GetComponent<AiManager>().BlueSoulStockage.Count; i++)
                {
                    listToCheck.Add(GameObject.Find("Main Camera").GetComponent<AiManager>().BlueSoulStockage[i]);
                }
            }
        }
        else if (AssignedBuilding.GetComponent<Building>().resourceProducedAtBuilding == ResourceType.redSoul)
        {
            if (GameObject.Find("Main Camera").GetComponent<AiManager>().RedSoulStorage.Count > 0)
            {
                for (int i = 0; i < GameObject.Find("Main Camera").GetComponent<AiManager>().RedSoulStorage.Count; i++)
                {
                    listToCheck.Add(GameObject.Find("Main Camera").GetComponent<AiManager>().RedSoulStorage[i]);
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

        Debug.Log(check + "CheckForClosestBuildingToPlaceStockage");

        return check;
    } // check if there is an enemy to attack close up

    public bool CheckIfThereIsAWayToPlaceStockage(ResourceType resourceType)
    {
        bool check;

        check = false;

        int freeStock = 0;

        List<GameObject> listToCheck = new List<GameObject>();

        if (resourceType == ResourceType.whiteSoul)
        {
            for (int i = 0; i < GameObject.Find("Main Camera").GetComponent<AiManager>().WhiteSoulStockage.Count; i++)
            {
                listToCheck.Add(GameObject.Find("Main Camera").GetComponent<AiManager>().WhiteSoulStockage[i]);
            }
        }
        else if (resourceType == ResourceType.blueVioletSoul)
        {
            for (int i = 0; i < GameObject.Find("Main Camera").GetComponent<AiManager>().BlueVioletSoulStockage.Count; i++)
            {
                listToCheck.Add(GameObject.Find("Main Camera").GetComponent<AiManager>().BlueVioletSoulStockage[i]);
            }
        }
        else if (resourceType == ResourceType.violetSoul)
        {
            for (int i = 0; i < GameObject.Find("Main Camera").GetComponent<AiManager>().VioletSoulStorage.Count; i++)
            {
                listToCheck.Add(GameObject.Find("Main Camera").GetComponent<AiManager>().VioletSoulStorage[i]);
            }
        }
        else if (resourceType == ResourceType.blueSoul)
        {
            for (int i = 0; i < GameObject.Find("Main Camera").GetComponent<AiManager>().BlueSoulStockage.Count; i++)
            {
                listToCheck.Add(GameObject.Find("Main Camera").GetComponent<AiManager>().BlueSoulStockage[i]);
            }
        }
        else if (resourceType == ResourceType.redSoul)
        {
            for (int i = 0; i < GameObject.Find("Main Camera").GetComponent<AiManager>().RedSoulStorage.Count; i++)
            {
                listToCheck.Add(GameObject.Find("Main Camera").GetComponent<AiManager>().RedSoulStorage[i]);
            }
        }

        if (listToCheck.Count > 0)
        {
            for (int i = 0; i < listToCheck.Count; i++)
            {
                freeStock += (listToCheck[i].GetComponent<Building>().maxStockage - listToCheck[i].GetComponent<Building>().currentStockage);
            }
        }

        if (freeStock > 0)
        {
            check = true;
        }
        else
        {
            check = false;
        }

        Debug.Log(check + "CheckIfThereIsAWayToPlaceStockage");

        return check;
    }

    public bool CheckIfCurrentStockInStockIsMoreThanZero(ResourceType resourceType)
    {
        bool check = false;

        int stockage = 0;

        List<GameObject> listToCheck = new List<GameObject>();
        
        if (resourceType == ResourceType.whiteSoul)
        {
            for (int i = 0; i < GameObject.Find("Main Camera").GetComponent<AiManager>().WhiteSoulStockage.Count; i++)
            {
                if (GameObject.Find("Main Camera").GetComponent<AiManager>().WhiteSoulStockage[i].GetComponent<Building>().currentStockage > 0)
                {
                    listToCheck.Add(GameObject.Find("Main Camera").GetComponent<AiManager>().WhiteSoulStockage[i]);
                }
            }
        }
        else if (resourceType == ResourceType.blueVioletSoul)
        {
            for (int i = 0; i < GameObject.Find("Main Camera").GetComponent<AiManager>().BlueVioletSoulStockage.Count; i++)
            {
                if (GameObject.Find("Main Camera").GetComponent<AiManager>().BlueVioletSoulStockage[i].GetComponent<Building>().currentStockage > 0)
                {
                    listToCheck.Add(GameObject.Find("Main Camera").GetComponent<AiManager>().BlueVioletSoulStockage[i]);
                }
            }
        }
        else if (resourceType == ResourceType.violetSoul)
        {
            for (int i = 0; i < GameObject.Find("Main Camera").GetComponent<AiManager>().VioletSoulStorage.Count; i++)
            {
                if (GameObject.Find("Main Camera").GetComponent<AiManager>().VioletSoulStorage[i].GetComponent<Building>().currentStockage > 0)
                {
                    listToCheck.Add(GameObject.Find("Main Camera").GetComponent<AiManager>().VioletSoulStorage[i]);
                }
            }
        }
        else if (resourceType == ResourceType.blueSoul)
        {
            for (int i = 0; i < GameObject.Find("Main Camera").GetComponent<AiManager>().BlueSoulStockage.Count; i++)
            {
                if (GameObject.Find("Main Camera").GetComponent<AiManager>().BlueSoulStockage[i].GetComponent<Building>().currentStockage > 0)
                {
                    listToCheck.Add(GameObject.Find("Main Camera").GetComponent<AiManager>().BlueSoulStockage[i]);
                }
            }
        }
        else if (resourceType == ResourceType.redSoul)
        {
            for (int i = 0; i < GameObject.Find("Main Camera").GetComponent<AiManager>().RedSoulStorage.Count; i++)
            {
                if (GameObject.Find("Main Camera").GetComponent<AiManager>().RedSoulStorage[i].GetComponent<Building>().currentStockage > 0)
                {
                    listToCheck.Add(GameObject.Find("Main Camera").GetComponent<AiManager>().RedSoulStorage[i]);
                }
            }
        }

        if (listToCheck.Count > 0)
        {
            int stock = 0;

            for (int i = 0; i < listToCheck.Count; i++)
            {
                stock += listToCheck[i].GetComponent<Building>().currentStockage;

                stockage += stock;
            }
        }
        else
        {
            stockage = 0;
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

        SoulObjects.SetActive(false);

        if (TargetToGoTo.GetComponent<Building>().currentStockage < TargetToGoTo.GetComponent<Building>().maxStockage)
        {
            TargetToGoTo.GetComponent<Building>().AddToStockage();
        }
        yield return new WaitForSeconds(0.4f);

        if (Wagon != null)
        {
            Wagon.SetActive(false);
        }

        yield return new WaitForSeconds(0.5f);

        AssignedBuilding.GetComponentInParent<Building>().BrokenSoulOnTableExist = false;
        AssignedBuilding.GetComponentInParent<Building>().WhiteSoulOnTableExist = false;
        AssignedBuilding.GetComponentInParent<Building>().BlueVioletSoulOnTableExist = false;
        AssignedBuilding.GetComponentInParent<Building>().VioletSoulOnTableExist = false;
        SoulBasketAmount = 0;

        runCoroutineSoulsOnce = false;
    }

    IEnumerator TakeFromStockpileWait()
    {
        runCoroutineSoulsOnce = true;

        if (TargetToGoTo.GetComponent<Building>().resourceProducedAtBuilding == ResourceType.whiteSoul)
        {
            resourceImCurrentlyTransporting = ResourceType.whiteSoul;
            SoulObjects.GetComponent<SoulColor>().ChangeColor(GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.whiteSoulColor);
        }
        else if (TargetToGoTo.GetComponent<Building>().resourceProducedAtBuilding == ResourceType.blueVioletSoul)
        {
            resourceImCurrentlyTransporting = ResourceType.blueVioletSoul;
            SoulObjects.GetComponent<SoulColor>().ChangeColor(GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.blueVioletColor);
        }
        else if (TargetToGoTo.GetComponent<Building>().resourceProducedAtBuilding == ResourceType.violetSoul)
        {
            resourceImCurrentlyTransporting = ResourceType.violetSoul;
            SoulObjects.GetComponent<SoulColor>().ChangeColor(GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.violetColor);
        }

        yield return new WaitForSeconds(0.4f);

        if (Wagon != null)
        {
            Wagon.SetActive(true);

            SoulObjects.SetActive(false);
        }

        SoulObjects.SetActive(true);

        if (TargetToGoTo.GetComponent<Building>().currentStockage > 0)
        {
            TargetToGoTo.GetComponent<Building>().currentStockage -= 1;
            SoulAmount += 1;
            TargetToGoTo.GetComponent<Building>().UpdateStockVisu();
        }
        yield return new WaitForSeconds(0.4f);

        yield return new WaitForSeconds(0.5f);
        runCoroutineSoulsOnce = false;
    }

    IEnumerator PlaceOnTableWait()
    {
        yield return new WaitForSeconds(0.3f);

        SoulObjects.SetActive(false);
        yield return new WaitForSeconds(0.4f);

        if (Wagon != null)
        {
            Wagon.SetActive(false);
        }

        yield return new WaitForSeconds(0.5f);

        SoulAmount = 0;

        if (resourceImCurrentlyTransporting == ResourceType.brokenSoul)
        {
            AssignedBuilding.GetComponentInParent<Building>().BrokenSoulOnTableExist = true; //there is smthg on the table
        }
        else if (resourceImCurrentlyTransporting == ResourceType.whiteSoul)
        {
            AssignedBuilding.GetComponentInParent<Building>().WhiteSoulOnTableExist = true;
        }
        else if (resourceImCurrentlyTransporting == ResourceType.blueVioletSoul)
        {
            AssignedBuilding.GetComponentInParent<Building>().BlueVioletSoulOnTableExist = true;
        }
        else if (resourceImCurrentlyTransporting == ResourceType.violetSoul)
        {
            AssignedBuilding.GetComponentInParent<Building>().VioletSoulOnTableExist = true;
        }

        AssignedBuilding.GetComponentInParent<Building>().WorkedOnTableBeenProcessed = false; //but it hasn't been processed yet
    }

    IEnumerator processing()
    {
        AssignedBuilding.GetComponent<Building>().LoadingBarForProcessing.SetActive(true);
        AssignedBuilding.GetComponent<Building>().imageFillingProcessing.fillAmount = 0;

        for (float i = 0; i < 10; i++)
        {
            AssignedBuilding.GetComponent<Building>().imageFillingProcessing.fillAmount = i / 10;
            yield return new WaitForSeconds(0.3f);
        }

        AssignedBuilding.GetComponentInParent<Building>().WorkedOnTableBeenProcessed = true;
        AssignedBuilding.GetComponent<Building>().LoadingBarForProcessing.SetActive(false);

        OnlyProcessOnce = false;
    }
}
