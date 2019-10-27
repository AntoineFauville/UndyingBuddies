using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reproduction : MonoBehaviour
{
    //look at the boys, if two boys are "good with everything" state, and are close enought to each other, they can reproduce.

    //get a random boy
    //look if he has a friend neirby
    //if that friend is goodeverything like him
    //reproduce!

    [SerializeField] private BoyFactory _boyFactory;

    void Start()
    {
        StartCoroutine(Reproduce());
    }

    IEnumerator Reproduce()
    {
        yield return new WaitForSeconds(Random.Range(5, 10));

        GameObject randomBoy = _boyFactory.Boys[Random.Range(0, _boyFactory.Boys.Count)].gameObject;

        if (randomBoy.GetComponent<Survive>().LookAroundForOtherBoys() == true)
        {
            _boyFactory.CreateBoy(randomBoy);
        }
        
        StartCoroutine(Reproduce());
    }
}
