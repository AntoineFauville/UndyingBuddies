using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIDemons : MonoBehaviour
{
    public string myName;
    public JobType JobType;
    public int life;
    private int _demonRangeOfDetection;
    private int _demonRangeOfCloseBy;

    public NavMeshAgent NavMeshAgent;

    public GameObject TargetToGoTo;

    public int woodAmount;
    public int foodAmount;
    public GameObject AssignedBuilding;

    bool AbleToPerformAction;

    public void Setup(string name, JobType initialJobtype, int initiallife, int demonRangeOfDetection, int demonRangeOfAttack)
    {
        myName = name;
        SwitchJob(initialJobtype);
        life = initiallife;
        _demonRangeOfDetection = demonRangeOfDetection;
        _demonRangeOfCloseBy = demonRangeOfAttack;
    }

    public void SwitchJob(JobType newJobtype)
    {
        JobType = newJobtype;
    }

    //actions
    public void Attack()
    {
        Debug.Log("attack");
        NavMeshAgent.isStopped = true;
    }
    public void Die()
    {
        Debug.Log("die");
        NavMeshAgent.isStopped = true;
    }
    public void Walk()
    {
        Debug.Log("walk");
        
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
    public void Gather(ResourceType resourceToGather)
    {
        Debug.Log("gather");
        NavMeshAgent.isStopped = true;

        if (!AbleToPerformAction)
        {
            AbleToPerformAction = true;
            
            if (resourceToGather == ResourceType.wood)
            {
                woodAmount += 1;
            }
            else if (resourceToGather == ResourceType.food)
            {
                foodAmount += 1;
            }

            StartCoroutine(TransferingTime());
        }
    }
    public void Idle()
    {
        Debug.Log("idle");
        NavMeshAgent.isStopped = true;
    }
    public void Place()
    {
        Debug.Log("place");
        NavMeshAgent.isStopped = true;
        woodAmount = 0;
        foodAmount = 0;
    }
    public void Build()
    {
        Debug.Log("build");
        NavMeshAgent.isStopped = true;
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

        listToCheck = GameObject.Find("Main Camera").GetComponent<AiManager>().Woods;

        if (resourceType == ResourceType.wood)
            listToCheck = GameObject.Find("Main Camera").GetComponent<AiManager>().Woods;
        else if (resourceType == ResourceType.food)
            listToCheck = GameObject.Find("Main Camera").GetComponent<AiManager>().Foods;

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

    IEnumerator TransferingTime()
    {
        yield return new WaitForSeconds(1);

        AbleToPerformAction = false;
    }
}
