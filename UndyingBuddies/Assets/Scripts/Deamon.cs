using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Deamon : MonoBehaviour
{
    //live for 15 ssec
    //run toward a random boy
    //grab him and eat him
    //fly away

    [SerializeField] private int seconds = 15;

    private int state;

    [SerializeField] private GameObject destinationToObjectif;

    [SerializeField] private GameObject PlaceBoyHere;

    public NavMeshAgent NavMeshAgent;

    public bool CanIStart;
    public Animator anim;

    void Start()
    {
        anim.Play("deamonApparition");

        StartCoroutine(waittoDie());
        StartCoroutine(waitToStart());
    }

    void Update()
    {
        if (CanIStart)
        {
            switch (state)
            {
                case 0: //inital find a boy
                    findBoy();

                    if (Vector3.Distance(destinationToObjectif.transform.position, this.transform.position) <= 4f)
                    {
                        state = 1;
                    }
                    else
                    {
                        NavMeshAgent.destination = destinationToObjectif.transform.position;
                    }
                    anim.Play("deamon");


                    break;
                case 1: //grab
                    destinationToObjectif.GetComponent<NavMeshAgent>().enabled = false;
                    destinationToObjectif.GetComponent<MovingBoy>().anim.Play("Arms");
                    destinationToObjectif.GetComponent<Survive>().WoodAmount = 0;
                    destinationToObjectif.GetComponent<MovingBoy>().enabled = false;
                    destinationToObjectif.transform.position = PlaceBoyHere.transform.position;

                    anim.Play("deamonGrabing");

                    StartCoroutine(waitToKillBoy());
                    break;
                case 2: //kill boy
                    destinationToObjectif.GetComponent<Survive>().food = 0;
                    destinationToObjectif.GetComponent<Survive>().dieded = true;
                    state = 0;
                    break;
            }
        }
    }

    void findBoy()
    {
        GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject potentialTarget in GameObject.Find("GameController").GetComponent<BoyFactory>().TotalOfTheBoys)
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

    IEnumerator waittoDie()
    {
        yield return new WaitForSeconds(seconds - 0.2f);
        anim.Play("deamonDiseapear");
        yield return new WaitForSeconds(0.3f);
        DestroyImmediate(this.gameObject);
    }

    IEnumerator waitToKillBoy()
    {
        yield return new WaitForSeconds(2);
        state = 2;
    }


    IEnumerator waitToStart()
    {
        yield return new WaitForSeconds(0.2f);
        CanIStart = true;
    }
}
