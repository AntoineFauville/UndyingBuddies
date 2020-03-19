using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatSpell : MonoBehaviour
{
    [SerializeField] private GameObject Target;

    [SerializeField] private List<GameObject> Rats = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SlowUpdate());
    }

    public GameObject CheckIfPriestIsCloseToAttack()
    {
        GameObject bestPriest = null;

        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        List<GameObject> listToCheck;

        listToCheck = GameObject.Find("Main Camera").GetComponent<AiManager>().Priest;

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

        return bestPriest;
    }

    void CleanupRats()
    {
        for (int i = 0; i < Rats.Count; i++)
        {
            if (Rats[i] == null)
            {
                Rats.Remove(Rats[i]);
            }
        }
    }

    // Update is called once per frame
    IEnumerator SlowUpdate()
    {
        yield return new WaitForSeconds(2f);

        CleanupRats();

        if (Rats.Count == 0)
        {
            Destroy(this);
        }

        Target = CheckIfPriestIsCloseToAttack();

        CleanupRats();

        for (int i = 0; i < Rats.Count; i++)
        {
            CleanupRats();
            Rats[i].GetComponent<Rats>().Target = Target;
        }

        StartCoroutine(SlowUpdate());
    }
}
