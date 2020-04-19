using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EyeOfDoom : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private GameObject Target;

    public int EyeState;

    [SerializeField] List<GameObject> listToCheck = new List<GameObject>();
    [SerializeField] List<GameObject> listOfAiFromTown = new List<GameObject>();

    [SerializeField] private GameSettings gameSettings;
    private bool canDoDamage;

    private bool OnlyOneCoroutineAtTime;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("Main Camera").GetComponent<AiManager>().Priest.Count > 0) // if i have a building to attack make sure the priest can attack the building
        {
            for (int i = 0; i < GameObject.Find("Main Camera").GetComponent<AiManager>().Priest.Count; i++)
            {
                if (!listToCheck.Contains(GameObject.Find("Main Camera").GetComponent<AiManager>().Priest[i]) 
                    && !GameObject.Find("Main Camera").GetComponent<AiManager>().Priest[i].GetComponent<AIPriest>().AmIBuilding)
                {
                    listToCheck.Add(GameObject.Find("Main Camera").GetComponent<AiManager>().Priest[i]);
                }
            }
        }

        if (!OnlyOneCoroutineAtTime)
        {
            StartCoroutine(EyeOfDooomTimers());
        }
    }

    public void CheckClosestPriestToAttack()
    {
        for (int i = 0; i < listToCheck.Count; i++)
        {
            if (listToCheck[i] == null)
            {
                listToCheck.Remove(listToCheck[i]);
            }
        }

        GameObject bestPriest = null;

        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        for (int i = 0; i < listToCheck.Count; i++)
        {
            if (listToCheck[i] == null)
            {
                listToCheck.Remove(listToCheck[i]);
            }
            else
            {
                Vector3 directionToTarget = listToCheck[i].transform.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestPriest = listToCheck[i];
                }
            }
        }

        Target = bestPriest;

        //after find closest target, we can get all the priest from that city
        if (listOfAiFromTown.Count <= 0)
        {
            for (int i = 0; i < bestPriest.GetComponent<AIPriest>().myAiTown.AllPriestUnit.Count; i++)
            {
                listOfAiFromTown.Add(bestPriest.GetComponent<AIPriest>().myAiTown.AllPriestUnit[i].gameObject);
            }

            //now listOfAiFromTown will contains only the ai from the city itself

            //then we can stipulate that the list now must look like the otherone so when we remove target we're removing from town etc 
            //since it's only in the start that it gets all the priest possible this should be fine
            // never do this because otherwise you confond the two and it crashes listToCheck = listOfAiFromTown;

            listToCheck.Clear();
            for (int i = 0; i < listOfAiFromTown.Count; i++)
            {
                listToCheck.Add(listOfAiFromTown[i]);
            }
        }
    } 

    void Update()
    {
        if (Target == null)
        {
            if (!OnlyOneCoroutineAtTime)
            {
                CheckClosestPriestToAttack();
            }
        }
        else
        {
            switch (EyeState)
            {
                case 0: // find the dude
                    if (Vector3.Distance(this.transform.position, Target.transform.position) < 4)
                    {
                        EyeState = 1;
                    }
                    else
                    {
                        navMeshAgent.destination = Target.transform.position;
                    }
                    break;

                case 1: // attack the dude
                    if (Target.GetComponent<AIStatController>() != null)
                    {
                        if (!canDoDamage)
                        {
                            canDoDamage = true;
                            StartCoroutine(EyeOfDoomDamage());
                        }
                    }
                    break;
            }
        }
    }

    IEnumerator EyeOfDooomTimers()
    {
        CheckClosestPriestToAttack();

        OnlyOneCoroutineAtTime = true;

        EyeState = 0;

        yield return new WaitForSeconds(3f);
        
        if (listToCheck.Contains(Target))
        {
            listToCheck.Remove(Target);
            Target = null;
        }

        OnlyOneCoroutineAtTime = false;

        StartCoroutine(EyeOfDooomTimers());
    }

    IEnumerator EyeOfDoomDamage()
    {
        this.transform.LookAt(Target.transform);
        if (!Target.GetComponent<AIPriest>().AmUnderEffect)
        {
            Target.GetComponent<AIPriest>().Stun(3);
        }
        Target.GetComponent<AIStatController>().TakeDamage(AiStatus.MentalHealth, gameSettings.eyeSpell);
        Target.GetComponent<AIPriest>().FearAmount += gameSettings.eyeSpell.FearAmount;
        yield return new WaitForSeconds(0.4f);
        canDoDamage = false;
    }
}
