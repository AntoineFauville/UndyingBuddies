﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EyeOfDoom : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    private GameObject Target;

    public int EyeState;

    [SerializeField] List<GameObject> listToCheck = new List<GameObject>();

    [SerializeField] private GameSettings gameSettings;
    private bool canDoDamage;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("Main Camera").GetComponent<AiManager>().Priest.Count > 0) // if i have a building to attack make sure the priest can attack the building
        {
            for (int i = 0; i < GameObject.Find("Main Camera").GetComponent<AiManager>().Priest.Count; i++)
            {
                if (!listToCheck.Contains(GameObject.Find("Main Camera").GetComponent<AiManager>().Priest[i]))
                {
                    listToCheck.Add(GameObject.Find("Main Camera").GetComponent<AiManager>().Priest[i]);
                }
            }
        }

        StartCoroutine(EyeOfDooomTimers());
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

        Target = bestPriest;
    } // check if there is an enemy to attack close up

    void Update()
    {
        if (Target == null)
        {
            CheckClosestPriestToAttack();
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

        EyeState = 0;

        yield return new WaitForSeconds(3f);

        Target.GetComponent<AIPriest>().Stun = false;

        if (listToCheck.Contains(Target))
        {
            listToCheck.Remove(Target);
        }

        StartCoroutine(EyeOfDooomTimers());
    }

    IEnumerator EyeOfDoomDamage()
    {
        this.transform.LookAt(Target.transform);

        Target.GetComponent<AIStatController>().TakeDamage(AiStatus.MentalHealth, gameSettings.eyeSpell);
        Target.GetComponent<AIPriest>().Stun = true;
        yield return new WaitForSeconds(0.4f);
        canDoDamage = false;
    }
}
