using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIPriest : MonoBehaviour
{
    public int Health;

    public bool CanAttackBack;

    public bool AmIBuilding;

    private GameSettings _gameSettings;
    private AiManager aiManager;

    public PriestType PriestType;
    public bool CanAttackAgain;

    public GameObject Target;

    public NavMeshAgent NavMeshAgent;

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
            Health = _gameSettings.PriestHealth;
        }
        else if (PriestType == PriestType.building)
        {
            Health = _gameSettings.PriestBuildingHealth;
        }

        CheckClosestDemonToAttack();
    }

    void Update()
    {
        if (Health <= 0)
        {
            Die();
        }

        //if see a demon nearby
        if (!AmIBuilding && CanAttackBack)
        {
            CheckClosestDemonToAttack();

            if (Vector3.Distance(this.transform.position, Target.transform.position) <= _gameSettings.demonRangeOfCloseBy && !CanAttackAgain)
            {
                NavMeshAgent.isStopped = true;

                CanAttackAgain = true;

                if (Target.GetComponent<AIDemons>() != null)
                {
                    Target.GetComponent<AIDemons>().life -= _gameSettings.PriestAttackAmount;
                }

                StartCoroutine(waitToReAttack());
            }
            else if (Vector3.Distance(this.transform.position, Target.transform.position) <= _gameSettings.demonRangeOfDetection)
            {
                NavMeshAgent.isStopped = false;

                NavMeshAgent.destination = Target.transform.position;
            }
            else
            {
                NavMeshAgent.isStopped = true;
            }
        }
    }

    void Die()
    {
        aiManager.Priest.Remove(this.gameObject);

        StartCoroutine(waitToDie());
    }

    public void CheckClosestDemonToAttack()
    {
        GameObject bestDemon = null;

        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        List<GameObject> listToCheck;

        listToCheck = GameObject.Find("Main Camera").GetComponent<AiManager>().Demons;

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
