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
    
    [SerializeField] SettingsData _settingsData;

    [SerializeField] private GameObject[] _houses;

    void Start()
    {
        StartCoroutine(Reproduce());
        StartCoroutine(DefaultGeneration());

        _houses = GameObject.FindGameObjectsWithTag("House");
    }

    IEnumerator Reproduce()
    {
        yield return new WaitForSeconds(Random.Range(_settingsData.ReproductionSpeedMin, _settingsData.ReproductionSpeedMax));

        GameObject randomBoy = _boyFactory.TotalOfTheBoys[Random.Range(0, _boyFactory.TotalOfTheBoys.Count)].gameObject;

        if (randomBoy.GetComponent<Survive>().LookAroundForOtherBoys() == true)
        {
            _boyFactory.CreateBoy(randomBoy, 2);
        }
        
        StartCoroutine(Reproduce());
    }

    IEnumerator DefaultGeneration()
    {
        yield return new WaitForSeconds(_settingsData.ReproductionByDefaultTime);
        
        _boyFactory.CreateBoy(_houses[Random.Range(0,_houses.Length)], 10);

        StartCoroutine(DefaultGeneration());
    }
}
