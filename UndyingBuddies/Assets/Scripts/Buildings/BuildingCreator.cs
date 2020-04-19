using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingCreator : MonoBehaviour
{
    BuildingType buildingType;

    int valueForName;

    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private GameSettings gameSettings;
    
    public void CreateBuilding(int building)
    {
        if (!GameObject.Find("Main Camera").GetComponent<Grab>().grabbing && !GameObject.Find("Main Camera").GetComponent<Grab>().notUsingSpell)
        {
            switch (building)
            {
                case (int)BuildingType.Processor: //0
                    if (resourceManager.amountOfEnergy >= gameSettings.CostOfNewBuilding)
                    {
                        InstantiateBuilding(gameSettings.processorBuilding.PrefabBuilding);
                    }
                    else
                    {
                        Debug.Log("not enought resources");
                    }
                    break;
            }
        }
        else
        {
            Debug.Log("place what you have in hand first");
        }
    }

    void InstantiateBuilding(GameObject gameObject)
    {
        GameObject newObj = Instantiate(gameObject);

        valueForName++;

        newObj.name = valueForName.ToString();

        StartCoroutine(waitForFeedback(newObj));        
    }

    IEnumerator waitForFeedback(GameObject newObj)
    {
        yield return new WaitForSeconds(0.05f);

        GameObject.Find("Main Camera").GetComponent<Grab>().grabbedItem = newObj;
        GameObject.Find("Main Camera").GetComponent<Grab>().grabbing = true;
    }
}
