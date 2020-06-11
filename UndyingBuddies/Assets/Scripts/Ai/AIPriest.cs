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

    public AITown myAiTown;

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
    
    public bool onlyStopFireOnce;
    public bool onlyFearStopOnce;
    public bool onlyPoisonOnce;
    public bool onlySlowOnce;
    public GameObject FearIndicator;
    public GameObject Flames;
    public GameObject PoisonIndicator;

    [SerializeField] private GameObject Hoe;
    [SerializeField] private GameObject Axe;
    [SerializeField] private GameObject Hammer;

    void Start()
    {
        if (Hoe != null)
        {
            Hoe.SetActive(false);
        }

        if (Axe != null)
        {
            Axe.SetActive(false);
        }

        if (Hammer != null)
        {
            Hammer.SetActive(false);
        }

        aiManager = GameObject.Find("Main Camera").GetComponent<AiManager>();
        _gameSettings = aiManager.GameSettings;

        if (!aiManager.Priest.Contains(this.gameObject))
        {
            aiManager.Priest.Add(this.gameObject);
        }

        if (!AmIBuilding)
        {
            Flames.SetActive(false);
            FearIndicator.SetActive(false);
            PoisonIndicator.SetActive(false);
        }
    }

    public void Stun(float stunDuration)
    {
        if (!stunOnce)
        {
            NavMeshAgent.isStopped = true;
            animatorPriest.Play("Stun");

            _stun = true;
            AmUnderEffect = true;
            currentAiPriestEffects = AiPriestEffects.Stun;
            StartCoroutine(UnStun(stunDuration));
        }
    }

    public void Question()
    {
        NavMeshAgent.isStopped = false;
        animatorPriest.Play("Question");
    }


    public void Walk()
    {
        NavMeshAgent.isStopped = false;
        animatorPriest.Play("Walk");

        NavMeshAgent.destination = Target.transform.position;

        Debug.DrawLine(this.transform.position, Target.transform.position, Color.red, 0.4f);

        animatorPriest.speed = NavMeshAgent.speed;

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

    public void Farm()
    {
        NavMeshAgent.isStopped = true;
        animatorPriest.Play("Farm");

        animatorPriest.speed = Random.Range(0.2f,1.5f);

        //this.transform.rotation = Quaternion.Euler(0,Random.Range(0,100), 0);

        Hoe.SetActive(true);
    }

    public void Build()
    {
        NavMeshAgent.isStopped = true;
        animatorPriest.Play("Build");

        animatorPriest.speed = Random.Range(0.2f, 1.5f);

        //this.transform.rotation = Quaternion.Euler(0,Random.Range(0,100), 0);

        Hammer.SetActive(true);
    }

    public void Lumber()
    {
        NavMeshAgent.isStopped = true;
        animatorPriest.Play("Lumber");

        animatorPriest.speed = Random.Range(0.2f, 1.5f);

        //this.transform.rotation = Quaternion.Euler(0,Random.Range(0,100), 0);

        Axe.SetActive(true);
    }

    public void Idle()
    {
        NavMeshAgent.isStopped = true;
        animatorPriest.Play("Idle");
    }

    public void OnFire()
    {
        this.GetComponent<AIStatController>().TakeDamage(AiStatus.Physical, 1);
        NavMeshAgent.SetDestination(FindRandomPositionNearMe(this.gameObject, 5));
        NavMeshAgent.isStopped = false;

        if (!onlyStopFireOnce)
        {
            Debug.Log("shit fire yo");

            animatorPriest.Play("OnFire");

            Flames.SetActive(true);
            NavMeshAgent.speed = 4f;
            StartCoroutine(SetFireOff());
        }
    }

    public void Poisoned()
    {
        this.GetComponent<AIStatController>().TakeDamage(AiStatus.Physical, 3);

        if (!onlyPoisonOnce)
        {
            Debug.Log("PoisonedYo");

            PoisonIndicator.SetActive(true);
            NavMeshAgent.speed = 0.75f;
            StartCoroutine(SetPoisonOff());
        }
    }

    public void Fear()
    {
        if (!onlyFearStopOnce)
        {
            Debug.Log("fear chutlulululu");

            animatorPriest.Play("Feared");

            NavMeshAgent.isStopped = false;

            NavMeshAgent.SetDestination(FindRandomPositionNearMe(this.gameObject, 30));
            NavMeshAgent.speed = 3f;
            FearIndicator.SetActive(true);
            StartCoroutine(SetFearOff());
        }
    }

    public void Slow()
    {
        if (!onlySlowOnce)
        {
            Debug.Log("being slowed");

            NavMeshAgent.isStopped = false;
            NavMeshAgent.speed = 0.5f;
            
            StartCoroutine(SetSlowOff());
        }
        
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

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = new Color(1,0,0,0.1f);
        Gizmos.DrawSphere(transform.position, MaxPositionCamper);
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

    public Vector3 FindRandomPositionNearMe(GameObject objectToCompareTo, int Range) //for the others to run around in a set range when on fire or so
    {
        Vector3 RandInAreaToGoTo = new Vector3(Random.Range(objectToCompareTo.transform.position.x - Range, objectToCompareTo.transform.position.x + Range),
                                        objectToCompareTo.transform.position.y,
                                        Random.Range(objectToCompareTo.transform.position.z - Range, objectToCompareTo.transform.position.z + Range));

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
            int rand;

            rand = Random.Range(0, 100);

            if (rand > 80)
            {
                preparationForAttack = true;
                Camp.GetComponent<AITown>().weNeedToPrepare = true;
            }
            else
            {
                Question();
                UiHealth.questionMark.enabled = true;
            }
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
                Instantiate(_gameSettings.DeathPriest, this.transform.position, new Quaternion());
                break;
            case 1: // by physical damage
                Instantiate(_gameSettings.DeathPriest, this.transform.position, new Quaternion());
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

    IEnumerator SetFireOff()
    {
        onlyStopFireOnce = true;
        yield return new WaitForSeconds(2);
        NavMeshAgent.speed = 1.5f;
        Flames.SetActive(false);
        onlyStopFireOnce = false;
        AmUnderEffect = false;
    }

    IEnumerator SetSlowOff()
    {
        onlySlowOnce = true;
        yield return new WaitForSeconds(2);
        NavMeshAgent.speed = 1.5f;
        onlySlowOnce = false;
    }

    IEnumerator SetFearOff()
    {
        onlyFearStopOnce = true;
        yield return new WaitForSeconds(5);
        NavMeshAgent.speed = 1.5f;
        FearIndicator.SetActive(false);
        onlyFearStopOnce = false;
        AmUnderEffect = false;
        FearAmount = 0;
    }

    IEnumerator SetPoisonOff()
    {
        onlyPoisonOnce = true;
        yield return new WaitForSeconds(7);
        NavMeshAgent.speed = 1.5f;
        PoisonIndicator.SetActive(false);
        onlyPoisonOnce = false;
        AmUnderEffect = false;
    }
}
