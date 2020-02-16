using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleMover : MonoBehaviour
{
    private int Statuses;

    public List<GameObject> travelPoints = new List<GameObject>();
    public int travelIndex;

    private float startTime;

    private float journeyLength;

    public float speed = 1.0F;

    private float distCovered;
    private float fractionOfJourney;

    private bool die;

    void Start()
    {
        startTime = Time.time;

        journeyLength = Vector3.Distance(travelPoints[0].transform.position, travelPoints[travelPoints.Count -1].transform.position);

        StartCoroutine(calculateSlowly());
    }

    IEnumerator calculateSlowly()
    {
        if (!die)
        {
            distCovered = (Time.time - startTime) * speed;
            fractionOfJourney = distCovered / journeyLength;

            if (Vector3.Distance(this.transform.position, travelPoints[travelIndex].transform.position) > 0.1)
            {
                Statuses = 0;
            }
            else
            {
                Statuses = 1;
            }

            switch (Statuses)
            {
                case 0: //travel
                    this.transform.position = Vector3.MoveTowards(this.transform.position, travelPoints[travelIndex].transform.position, fractionOfJourney);
                    break;

                case 1: //switch target
                    if (travelIndex >= travelPoints.Count - 1)
                    {
                        StartCoroutine(waitForFadeThenDie());
                    }
                    else
                    {
                        travelIndex++;
                        Statuses = 0;
                    }
                    break;
            }
        }

        yield return new WaitForSeconds(0.08f);
        StartCoroutine(calculateSlowly());
    }

    IEnumerator waitForFadeThenDie()
    {
        die = true;
        yield return new WaitForSeconds(2);
        DestroyImmediate(this.gameObject);
    }
}
