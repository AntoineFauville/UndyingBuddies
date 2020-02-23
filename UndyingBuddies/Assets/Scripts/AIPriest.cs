using System.Collections;
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

    public bool Stun;
    public bool isAttacked;
    public GameObject buildingToWalkTo;

    public int MaxPositionCamper = 10;
    public GameObject Camp;
    public bool CanLookAround;
    public bool preparationForAttack;
    public bool canOnlyDoOnceCoroutine;
    public GameObject TargeForRandom;

    void Start()
    {
        aiManager = GameObject.Find("Main Camera").GetComponent<AiManager>();
        _gameSettings = aiManager.GameSettings;

        if (!aiManager.Priest.Contains(this.gameObject))
        {
            aiManager.Priest.Add(this.gameObject);
        }
        
        CheckClosestDemonToAttack();
    }

    void Update()
    {
        if (Stun)
        {
            if (!AmIBuilding && CanAttackBack)
            {
                NavMeshAgent.isStopped = true;

                animatorPriest.Play("Stun");

                StartCoroutine(UnStun());
            }
        }
        else
        {
            if (isAttacked)
            {
                switch (PriestAttackerType)
                {
                    case PriestAttackerType.defender:
                        if (!AmIBuilding && CanAttackBack)
                        {
                            CheckClosestDemonToAttack();

                            if (Target != null)
                            {
                                if (Vector3.Distance(this.transform.position, Target.transform.position) <= _gameSettings.demonRangeOfCloseBy && !CanAttackAgain)
                                {
                                    NavMeshAgent.isStopped = true;

                                    animatorPriest.Play("Attack");

                                    CanAttackAgain = true;

                                    if (Target.GetComponent<AIDemons>() != null)
                                    {
                                        Target.GetComponent<AIDemons>().life -= _gameSettings.PriestAttackAmount;
                                    }
                                    else if (Target.GetComponent<Building>() != null)
                                    {
                                        Target.GetComponent<Building>().GetAttack(_gameSettings.PriestAttackAmount);
                                    }

                                    StartCoroutine(waitToReAttack());
                                }
                                else if (Vector3.Distance(this.transform.position, Target.transform.position) <= _gameSettings.demonRangeOfDetection)
                                {
                                    NavMeshAgent.isStopped = false;

                                    NavMeshAgent.destination = Target.transform.position;

                                    animatorPriest.Play("Walk");
                                }
                                else
                                {
                                    animatorPriest.Play("Idle");

                                    NavMeshAgent.isStopped = true;
                                }
                            }
                        }
                        break;
                    case PriestAttackerType.rusherFromCity:
                        if (!AmIBuilding && CanAttackBack)
                        {
                            CheckClosestDemonToAttack();

                            if (Target != null)
                            {
                                if (Vector3.Distance(this.transform.position, Target.transform.position) <= _gameSettings.demonRangeOfCloseBy && !CanAttackAgain)
                                {
                                    NavMeshAgent.isStopped = true;

                                    animatorPriest.Play("Attack");

                                    CanAttackAgain = true;

                                    if (Target.GetComponent<AIDemons>() != null)
                                    {
                                        Target.GetComponent<AIDemons>().life -= _gameSettings.PriestAttackAmount;
                                    }
                                    else if (Target.GetComponent<Building>() != null)
                                    {
                                        Target.GetComponent<Building>().GetAttack(_gameSettings.PriestAttackAmount);
                                    }

                                    StartCoroutine(waitToReAttack());
                                }
                                else
                                {
                                    NavMeshAgent.isStopped = false;

                                    NavMeshAgent.destination = Target.transform.position;

                                    animatorPriest.Play("Walk");
                                }
                            }
                        }
                        break;
                    case PriestAttackerType.followFormation:
                        if (!AmIBuilding && CanAttackBack)
                        {
                            CheckClosestDemonToAttack();

                            if (Target != null)
                            {
                                if (Vector3.Distance(this.transform.position, Target.transform.position) <= _gameSettings.demonRangeOfCloseBy && !CanAttackAgain)//if i'm close to an enemy i attack
                                {
                                    NavMeshAgent.isStopped = true;

                                    animatorPriest.Play("Attack");

                                    CanAttackAgain = true;

                                    if (Target.GetComponent<AIDemons>() != null)
                                    {
                                        Target.GetComponent<AIDemons>().life -= _gameSettings.PriestAttackAmount;
                                    }
                                    else if (Target.GetComponent<Building>() != null)
                                    {
                                        Target.GetComponent<Building>().GetAttack(_gameSettings.PriestAttackAmount);
                                    }

                                    StartCoroutine(waitToReAttack());
                                }
                                else if (Vector3.Distance(this.transform.position, Target.transform.position) <= _gameSettings.demonRangeOfDetection)//if i've in my detection range an enemy target becomes the enemy
                                {
                                    NavMeshAgent.isStopped = false;

                                    NavMeshAgent.destination = Target.transform.position;

                                    animatorPriest.Play("Walk");
                                }
                                else //if i'm away from that tell me what i need to follow
                                {
                                    Target = aiFormationFollowPoint;

                                    NavMeshAgent.isStopped = false;

                                    NavMeshAgent.destination = aiFormationFollowPoint.transform.position;

                                    animatorPriest.Play("Walk");
                                }
                            }
                            else //if i'm away from that tell me what i need to follow
                            {
                                Target = aiFormationFollowPoint;

                                NavMeshAgent.isStopped = false;

                                NavMeshAgent.destination = aiFormationFollowPoint.transform.position;

                                animatorPriest.Play("Walk");
                            }
                        }
                        break;
                }
            }
            else
            {
                switch (PriestAttackerType)
                {
                    case PriestAttackerType.defender:
                        //while i'm not attacked
                        if (!AmIBuilding && CanAttackBack)
                        {
                            if (Vector3.Distance(this.transform.position, buildingToWalkTo.transform.position) <= _gameSettings.demonRangeOfDetection)
                            {
                                NavMeshAgent.isStopped = false;

                                NavMeshAgent.destination = buildingToWalkTo.transform.position;

                                animatorPriest.Play("Walk");
                            }
                            else
                            {
                                animatorPriest.Play("Idle");

                                NavMeshAgent.isStopped = true;
                            }
                        }
                        break;
                    case PriestAttackerType.rusherFromCity:
                        Debug.Log("mehh");
                        break;
                    case PriestAttackerType.followFormation:
                        break;
                    case PriestAttackerType.camper:
                        if (!preparationForAttack)
                        {
                            if (Target == null)
                            {
                                if (Camp == null)
                                {
                                    Debug.Log("please make sure " + this.gameObject.name + " is assigned to the camp");
                                }
                                else
                                {
                                    Vector3 RandInAreaToGoTo = new Vector3(Random.Range(Camp.transform.position.x - MaxPositionCamper, Camp.transform.position.x + MaxPositionCamper),
                                        Camp.transform.position.y,
                                        Random.Range(Camp.transform.position.z - MaxPositionCamper, Camp.transform.position.z + MaxPositionCamper));

                                    if (TargeForRandom == null)
                                    {
                                        TargeForRandom = new GameObject();
                                        TargeForRandom.name = this.name + " TargetForRandom";

                                        TargeForRandom.transform.position = RandInAreaToGoTo;

                                        Target = TargeForRandom;
                                    }
                                    else
                                    {
                                        TargeForRandom.transform.position = RandInAreaToGoTo;

                                        Target = TargeForRandom;
                                    }
                                }
                            }

                            if (Vector3.Distance(this.transform.position, Target.transform.position) > 2)
                            {
                                //walk;
                                animatorPriest.Play("Walk");

                                NavMeshAgent.isStopped = false;
                                NavMeshAgent.destination = Target.transform.position;
                            }
                            else
                            {
                                if (!canOnlyDoOnceCoroutine)
                                {
                                    StartCoroutine(WaitAndLook());
                                }
                            }

                            if (CanLookAround && !preparationForAttack)
                            {
                                LookIfFindAnyEnemy();
                            }
                            if (preparationForAttack)
                            {
                                Debug.Log("Found something sir, lets rpepare to attack these demons");
                            }
                        }
                        if(preparationForAttack)
                        {
                            Target = Camp;
                            if (Vector3.Distance(this.transform.position, Target.transform.position) > 2)
                            {
                                NavMeshAgent.isStopped = false;
                                NavMeshAgent.destination = Target.transform.position;
                                animatorPriest.Play("Walk");
                            }
                            else
                            {
                                animatorPriest.Play("Idle");
                                NavMeshAgent.isStopped = true;
                            }

                            StartCoroutine(waitForRaid());
                        }
                        break;
                }

               
            }
        }
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
        aiManager.Priest.Remove(this.gameObject);

        StartCoroutine(waitToDie(diedByWhat));
    }

    public void CheckClosestDemonToAttack()
    {
        GameObject bestDemon = null;

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

    IEnumerator waitToReAttack()
    {
        yield return new WaitForSeconds(1);
        CanAttackAgain = false;
    }

    IEnumerator UnStun()
    {
        yield return new WaitForSeconds(5);

        Stun = false;
    }

    IEnumerator WaitAndLook()
    {
        canOnlyDoOnceCoroutine = true;
        NavMeshAgent.isStopped = true;

        animatorPriest.Play("Observe");

        CanLookAround = true;

        yield return new WaitForSeconds(5);

        CanLookAround = false;
        canOnlyDoOnceCoroutine = false;
        Target = null;
    }

    IEnumerator waitForRaid()
    {
        yield return new WaitForSeconds(_gameSettings.timeToPrepareWithACamp);
        Camp.GetComponent<AITown>().RevengeCamp = true;
    }
}
