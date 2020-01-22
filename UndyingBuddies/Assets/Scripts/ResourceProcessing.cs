using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceProcessing : MonoBehaviour
{
    private Building building;

    private bool CanProcess;
    private int amountOfAiAttributed;

    private int timeToWait;

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

        amountOfAiAttributed = building.AiAttributedToBuilding.Count;

        StartCoroutine(waitToProcess());
    }

    void Update()
    {
        //verification to not divide time by 0
        if (amountOfAiAttributed == 0)
        {
            timeToWait = 2;
        }
        else
        {
            timeToWait = 2 / amountOfAiAttributed;
        }

        if (building.StockPile.Count > 0 && building.AiAttributedToBuilding.Count > 0)
        {
            CanProcess = true;
        }
        else
        {
            CanProcess = false;
        }
    }

    IEnumerator waitToProcess()
    {
        yield return new WaitForSeconds(timeToWait);

        if (CanProcess)
        {
            building.StockPile[0].GetComponent<TransformIntoResource>().TransformIntoOtherResource();
            building.StockPile.Remove(building.StockPile[0]);
        }

        StartCoroutine(waitToProcess());
    }
}
