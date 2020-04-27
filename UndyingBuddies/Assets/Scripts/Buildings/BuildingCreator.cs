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

    public bool creationMode;
    
    public void CreateBuilding()
    {
        creationMode = !creationMode;

        Debug.Log("Select a place to build it" + " creation mode " + creationMode);

        ManageCreationMode();
    }

    void ManageCreationMode()
    {
        for (int i = 0; i < resourceManager.GetComponent<AiManager>().Buildings.Count; i++)
        {
            for (int y = 0; y < resourceManager.GetComponent<AiManager>().Buildings[i].GetComponent<Building>().buildingCommunicators.Count; y++)
            {
                if (creationMode)
                {
                    resourceManager.GetComponent<AiManager>().Buildings[i].GetComponent<Building>().buildingCommunicators[y].FindConnection();
                    resourceManager.GetComponent<AiManager>().Buildings[i].GetComponent<Building>().buildingCommunicators[y].artShow = true;
                    resourceManager.GetComponent<AiManager>().Buildings[i].GetComponent<Building>().buildingCommunicators[y].ManageArt();
                }
                else
                {
                    resourceManager.GetComponent<AiManager>().Buildings[i].GetComponent<Building>().buildingCommunicators[y].FindConnection();
                    resourceManager.GetComponent<AiManager>().Buildings[i].GetComponent<Building>().buildingCommunicators[y].artShow = false;
                    resourceManager.GetComponent<AiManager>().Buildings[i].GetComponent<Building>().buildingCommunicators[y].ManageArt();
                }
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            creationMode = false;

            ManageCreationMode();
        }
    }

    public void CreateBuildingHere(Transform transformPosition)
    {
        if (resourceManager.amountOfEnergy >= gameSettings.CostOfNewBuilding)
        {
            InstantiateBuilding(gameSettings.processorBuilding.PrefabBuilding, transformPosition);
        }
        else
        {
            Debug.Log("not enought resources");
        }

        ManageCreationMode();
    }

    void InstantiateBuilding(GameObject gameObject, Transform transformPosition)
    {
        GameObject newObj = Instantiate(gameObject, transformPosition.position, new Quaternion());

        valueForName++;

        newObj.name = valueForName.ToString();

        resourceManager.ManageCostOfPurchaseForBuilding();

        if (!resourceManager.GetComponent<AiManager>().Buildings.Contains(newObj))
        {
            resourceManager.GetComponent<AiManager>().Buildings.Add(newObj);
        }

        if (newObj.transform.GetComponent<Building>().amountOfActiveWorker < newObj.transform.GetComponent<Building>().amountOfWorkerAllowed
            && !resourceManager.GetComponent<AiManager>().BuildingWithJobs.Contains(newObj))
        {
            resourceManager.GetComponent<AiManager>().BuildingWithJobs.Add(newObj);
        }

        if (newObj.transform.GetComponent<Building>().resourceProducedAtBuilding == ResourceType.whiteSoul)
        {
            resourceManager.GetComponent<AiManager>().WhiteSoulStockage.Add(newObj);
        }

        if (newObj.transform.GetComponent<Building>().resourceProducedAtBuilding == ResourceType.blueVioletSoul)
        {
            resourceManager.GetComponent<AiManager>().BlueVioletSoulStockage.Add(newObj);
        }
    }
}
