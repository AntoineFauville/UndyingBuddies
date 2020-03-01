﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIPriest : MonoBehaviour
{
    //physical
    public int healthAmount;
    public int maxHealth;
    //fear
    public int FearAmount;
    public int fearMaxAmount;
    //Mental Health
    public int MentalHealthAmount;
    public int MentalHealthMaxAmount;

    public bool CanAttackBack;

    public bool AmIBuilding;

    private GameSettings _gameSettings;
    private AiManager aiManager;

    public PriestType PriestType;
    public bool CanAttackAgain;

    public GameObject Target;

    public NavMeshAgent NavMeshAgent;

    public bool amIInFire;
    public bool attackedBySpike;
    public bool attackedByEye;
    public bool attackedByTentacle;

    public UiHealth UiHealth;

    public PriestAttackerType PriestAttackerType;

    public GameObject aiFormationFollowPoint;

    public Animator animatorPriest;

    public bool _stun;
    public bool isAttacked;
    public GameObject buildingToWalkTo;

    public int MaxPositionCamper = 10;
    public GameObject Camp;
    public bool CanLookAround;
    public bool preparationForAttack;
    public bool canOnlyDoOnceCoroutine;
    public GameObject TargeForRandom;

    public AIPriestType _myAIPriestType;
    public bool AmUnderEffect;
    public AiPriestEffects currentAiPriestEffects;
    public bool stunOnce;

    void Start()
    {
        aiManager = GameObject.Find("Main Camera").GetComponent<AiManager>();
        _gameSettings = aiManager.GameSettings;

        if (!aiManager.Priest.Contains(this.gameObject))
        {
            aiManager.Priest.Add(this.gameObject);
        }
    }

    public void Stun(float stunDuration)
    {
        NavMeshAgent.isStopped = true;
        animatorPriest.Play("Stun");

        _stun = true;
        AmUnderEffect = true;

        if (!stunOnce)
        {
            StartCoroutine(UnStun(stunDuration));
        }
    }

    public void Walk()
    {
        NavMeshAgent.isStopped = false;
        animatorPriest.Play("Walk");

        NavMeshAgent.destination = Target.transform.position;

        if (Vector3.Distance(Target.transform.position, this.transform.position) > 2)
        {
            NavMeshAgent.isStopped = false;
            NavMeshAgent.SetDestination(Target.transform.position);
        }
        else
        {
            NavMeshAgent.isStopped = true;
        }
    }

    public void Idle()
    {
        NavMeshAgent.isStopped = true;
        animatorPriest.Play("Idle");
    }

    public void OnFire()
    {
    }

    public void Fear()
    {
        NavMeshAgent.isStopped = true;
        animatorPriest.Play("Feared");
    }

    public void Observe()
    {
        NavMeshAgent.isStopped = true;
        animatorPriest.Play("Observe");

        LookIfFindAnyEnemy();

        if (!canOnlyDoOnceCoroutine)
        {
            StartCoroutine(WaitAndLook());
        }
    }

    public void Attack()
    {
        if (!CanAttackAgain)
        {
            StartCoroutine(WaitToAttack());
        }

        NavMeshAgent.isStopped = true;
        animatorPriest.Play("Attack");
    }

    public Vector3 FindRandomPositionNearMe(GameObject objectToCompareTo) // for the camp use this to walk around the camp, for the fear, use myself
    {
        Vector3 RandInAreaToGoTo = new Vector3(Random.Range(objectToCompareTo.transform.position.x - MaxPositionCamper, objectToCompareTo.transform.position.x + MaxPositionCamper),
                                        objectToCompareTo.transform.position.y,
                                        Random.Range(objectToCompareTo.transform.position.z - MaxPositionCamper, objectToCompareTo.transform.position.z + MaxPositionCamper));

        return RandInAreaToGoTo;
    }

    public void LookIfFindAnyEnemy()
    {
        List<GameObject> demonAroundMe = new List<GameObject>();

        Collider[] hitCollider = Physics.OverlapSphere(this.transform.position, MaxPositionCamper);
        for (int i = 0; i < hitCollider.Length; i++)
        {
            if (hitCollider[i].GetComponent<AIDemons>() != null)
            {
                demonAroundMe.Add(hitCollider[i].gameObject);
            }
        }

        if (demonAroundMe.Count > 0)
        {
            preparationForAttack = true;
            Camp.GetComponent<AITown>().weNeedToPrepare = true;
        }
    }

    public void Die(int diedByWhat)
    {
        aiManager
            .Priest
            .Remove(
            this.gameObject);

        StartCoroutine(waitToDie(diedByWhat));
    }

    public void CheckClosestDemonToAttack()
    {
        GameObject bestDemon = null;

        Debug.Log("checking for an enemy");

        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        List<GameObject> listToCheck = new List<GameObject>();
        
        if (GameObject.Find("Main Camera").GetComponent<AiManager>().Demons.Count > 0) // if i have a building to attack make sure the priest can attack the building
        {
            for (int i = 0; i < GameObject.Find("Main Camera").GetComponent<AiManager>().Demons.Count; i++)
            {
                if (!listToCheck.Contains(GameObject.Find("Main Camera").GetComponent<AiManager>().Demons[i]))
                {
                    listToCheck.Add(GameObject.Find("Main Camera").GetComponent<AiManager>().Demons[i]);
                }
            }
        }

        if (GameObject.Find("Main Camera").GetComponent<AiManager>().Buildings.Count > 0) // if i have a building to attack make sure the priest can attack the building
        {
            for (int i = 0; i < GameObject.Find("Main Camera").GetComponent<AiManager>().Buildings.Count; i++)
            {
                if (!listToCheck.Contains(GameObject.Find("Main Camera").GetComponent<AiManager>().Buildings[i]))
                {
                    listToCheck.Add(GameObject.Find("Main Camera").GetComponent<AiManager>().Buildings[i]);
                }
            }
        }

        for (int i = 0; i < listToCheck.Count; i++)
        {
            if (listToCheck[i] == null)
            {
                listToCheck.Remove(listToCheck[i]);
            }

            Vector3 directionToTarget = listToCheck[i].transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestDemon = listToCheck[i];
            }
        } 
        
        Target = bestDemon;
    } // check if there is an enemy to attack close up
    
    IEnumerator waitToDie(int diedByWhat)
    {
        yield return new WaitForSeconds(0.05f);

        switch (diedByWhat)
        {
            case 0: // by mental health

                break;
            case 1: // by physical damage

                break;
        }

        DestroyImmediate(this.gameObject);
    }

    IEnumerator WaitToAttack()
    {
        CanAttackAgain = true;

        if (Target.GetComponent<AIDemons>() != null)
        {
            Target.GetComponent<AIDemons>().life -= _gameSettings.PriestAttackAmount;
        }
        else if (Target.GetComponent<Building>() != null)
        {
            Target.GetComponent<Building>().GetAttack(_gameSettings.PriestAttackAmount);
        }

        yield return new WaitForSeconds(1);
        CanAttackAgain = false;
    }

    IEnumerator UnStun(float stunDuration)
    {
        stunOnce = true;
        animatorPriest.Play("Stun");
        yield return new WaitForSeconds(stunDuration);
        animatorPriest.Play("Stun");
        stunOnce = false;
        AmUnderEffect = false;
        _stun = false;
    }

    public IEnumerator WaitAndLook()
    {
        canOnlyDoOnceCoroutine = true;
        
        yield return new WaitForSeconds(5);

        CanLookAround = false;
        canOnlyDoOnceCoroutine = false;
        TargeForRandom.transform.position = FindRandomPositionNearMe(Camp); //this is the camp

        Target = TargeForRandom;
    }
}
