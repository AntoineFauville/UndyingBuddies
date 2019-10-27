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

    public GameObject destinationToObjectif;

    void Start()
    {
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

    public void FindClosestUsable()
    {
        GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (GameObject potentialTarget in GameObject.Find("GameController").GetComponent<Usables>().Bush)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

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

            switch (BoyState)
            {
                case BoyState.Idle:
                    NavMeshAgent.SetDestination(RandomNavmeshLocation(6f));
                    anim.Play("ArmsWalking");
                    break;

                case BoyState.FindingFood:
                    anim.Play("Arms");
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
        FindClosestUsable();
        yield return new WaitForSeconds(1f);
        StartCoroutine(CheckWhatTheClosestAroundYouIs());
    }
}
