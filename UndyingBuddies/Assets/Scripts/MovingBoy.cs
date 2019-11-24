using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovingBoy : MonoBehaviour
{
    public Survive Survive;

    public bool Dead;
    public NavMeshAgent NavMeshAgent;

    public Animator anim;

    public BoyState BoyState;

    [SerializeField] public GameObject destinationToObjectif;
    GameObject destinationToRunAwayFromFireTo;

    public bool RunAwayFromFire;

    public UsableType typeOfUsableImLookingFor;

    void Start()
    {
        FindClosestUsable(UsableType.Bush);
        FindClosestUsable(UsableType.House);
        FindClosestUsable(UsableType.Tree);

        StartCoroutine(CheckWhatTheClosestAroundYouIs());
    }

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

    public void FindClosestUsable(UsableType usableType)
    {
        GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        
        List<GameObject> listToCheck;

        if (usableType == UsableType.Bush)
            listToCheck = GameObject.Find("GameController").GetComponent<Usables>().Bush;
        else if (usableType == UsableType.House)
            listToCheck = GameObject.Find("GameController").GetComponent<Usables>().House;
        else 
            listToCheck = GameObject.Find("GameController").GetComponent<Usables>().Tree;
        

        foreach (GameObject potentialTarget in listToCheck)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        if (usableType == UsableType.Bush)
            Survive.NeirbyBush = bestTarget;
        else if (usableType == UsableType.House)
            Survive.NeirbyHouse = bestTarget;
        else
            Survive.NeirbyTree = bestTarget;

        destinationToObjectif = bestTarget;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Dead)
        {
            if (GameObject.Find("GameController").GetComponent<Usables>().Bush.Count <= 0)
            {
                BoyState = BoyState.Idle;
            }

            if (GameObject.Find("GameController").GetComponent<Usables>().Tree.Count <= 0)
            {
                BoyState = BoyState.Idle;
            }

            if (GameObject.Find("GameController").GetComponent<Usables>().House.Count <= 0)
            {
                BoyState = BoyState.Idle;
            }

            switch (BoyState)
            {
                case BoyState.Idle:
                    NavMeshAgent.SetDestination(RandomNavmeshLocation(6f));
                    anim.Play("ArmsWalking");
                    break;

                case BoyState.FindingFood:
                    anim.Play("ArmsFeed");
                    BoyState = BoyState.WalkingToObjectif;
                    break;

                case BoyState.WalkingToObjectif:
                    if (destinationToObjectif != null)
                    {
                        if (Vector3.Distance(destinationToObjectif.transform.position, this.transform.position) > 2)
                        {
                            NavMeshAgent.SetDestination(destinationToObjectif.transform.position);
                        }
                        else
                        {
                            BoyState = BoyState.StandingStill;
                        }
                    }
                    break;

                case BoyState.StandingStill:
                    anim.Play("ArmsNotMoving");
                    break;

                case BoyState.RunAway:
                    //find a random spot 30 min max away



                    if (!RunAwayFromFire)
                    {
                        RunAwayFromFire = true;

                        GameObject newPosGameObject = new GameObject();

                        newPosGameObject.transform.position = RandomNavmeshLocation(30);

                        destinationToRunAwayFromFireTo = newPosGameObject;
                    }

                    destinationToObjectif = destinationToRunAwayFromFireTo;

                    anim.Play("Arms");


                    if (destinationToObjectif != null)
                    {
                        if (Vector3.Distance(destinationToObjectif.transform.position, this.transform.position) > 2)
                        {
                            NavMeshAgent.SetDestination(destinationToObjectif.transform.position);
                        }
                        else
                        {
                            RunAwayFromFire = false;
                        }
                    }
                    break;

                default:
                    break;
            }
        }
        else
        {
            NavMeshAgent.isStopped = true;
        }
    }

    IEnumerator CheckWhatTheClosestAroundYouIs()
    {
        FindClosestUsable(UsableType.Bush);
        FindClosestUsable(UsableType.House);
        FindClosestUsable(UsableType.Tree);
        yield return new WaitForSeconds(1f);
        StartCoroutine(CheckWhatTheClosestAroundYouIs());
    }
}
