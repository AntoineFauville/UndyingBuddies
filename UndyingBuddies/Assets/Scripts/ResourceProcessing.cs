using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceProcessing : MonoBehaviour
{
    [SerializeField] private Building building;

    [SerializeField] private bool CanProcess;
    [SerializeField] private float amountOfAiAttributed;

    private float timeToWait;

    void Start()
    {
        if (this.GetComponent<Building>() != null)
        {
            building = this.GetComponent<Building>();
        }
        else
        {
            Debug.Log("Can't process if there isn't any stockpile to refer to");
        }

        StartCoroutine(waitToProcess());
    }

    void Update()
    {
        amountOfAiAttributed = building.AiAttributedToBuilding.Count;

        //verification to not divide time by 0
        if (amountOfAiAttributed <= 0)
        {
            timeToWait = 2;
        }
        else
        {
            timeToWait = 2 / amountOfAiAttributed;
        }

        if (building.Stokpile.Count > 0 && building.AiAttributedToBuilding.Count > 0)
        {
            CanProcess = true;
        }
        else
        {
            CanProcess = false;
        }

        //Debug.Log(timeToWait);    
    }

    IEnumerator waitToProcess()
    {
        yield return new WaitForSeconds(timeToWait);

        if (CanProcess)
        {
            building.Stokpile[0].GetComponent<TransformIntoResource>().TransformIntoOtherResource();

            building.Stokpile.Remove(building.Stokpile[0]);
        }

        StartCoroutine(waitToProcess());
    }
}
