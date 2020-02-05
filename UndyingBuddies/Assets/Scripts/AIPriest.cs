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
    //Lonelyness
    //public int LonelynessAmount;
    //public int LonelynessMaxAmount;
    ////Intestine Status
    //public int IntestineStatusAmount;
    //public int IntestineStatusMaxAmount;

    public bool CanAttackBack;

    public bool AmIBuilding;

    private GameSettings _gameSettings;
    private AiManager aiManager;

    public PriestType PriestType;
    public bool CanAttackAgain;

    public GameObject Target;

    public NavMeshAgent NavMeshAgent;

    public bool amIInFire;

    public UiHealth UiHealth;

    public PriestAttackerType PriestAttackerType;

    public GameObject aiFormationFollowPoint;

    public Animator animatorPriest;

    void Start()
    {
        aiManager = GameObject.Find("Main Camera").GetComponent<AiManager>();
        _gameSettings = aiManager.GameSettings;

        if (!aiManager.Priest.Contains(this.gameObject))
        {
            aiManager.Priest.Add(this.gameObject);
        }

        if (PriestType == PriestType.soldier)
        {
            healthAmount = _gameSettings.PriestHealth;
        }
        else if (PriestType == PriestType.building)
        {
            healthAmount = _gameSettings.PriestBuildingHealth;
        }

        CheckClosestDemonToAttack();

        maxHealth = healthAmount;
    }

    void Update()
    {
        if (healthAmount <= 0)
        {
            Die();
        }
        else if (MentalHealthAmount <= 0)
        {
            Die();
        }

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

            case PriestAttackerType.rusher:
                if (!AmIBuilding && CanAttackBack)
                {
                    CheckClosestDemonToAttack();

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

    void Die()
    {
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("Floor").Length; i++)
        {
            if (GameObject.FindGameObjectsWithTag("Floor")[i].GetComponent<TerrainManagerLock>() != null)
            {
                if (GameObject.FindGameObjectsWithTag("Floor")[i].GetComponent<TerrainManagerLock>().AIOnMe.Contains(this.gameObject))
                {
                    GameObject.FindGameObjectsWithTag("Floor")[i].GetComponent<TerrainManagerLock>().AIOnMe.Remove(this.gameObject);
                }
            }
        }

        aiManager.Priest.Remove(this.gameObject);

        StartCoroutine(waitToDie());
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

        foreach (GameObject potentialTarget in listToCheck)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestDemon = potentialTarget;
            }
        }

        Target = bestDemon;
    } // check if there is an enemy to attack close up
    
    IEnumerator waitToDie()
    {
        yield return new WaitForSeconds(0.05f);

        DestroyImmediate(this.gameObject);
    }

    IEnumerator waitToReAttack()
    {
        yield return new WaitForSeconds(1);
        CanAttackAgain = false;
    }
}
