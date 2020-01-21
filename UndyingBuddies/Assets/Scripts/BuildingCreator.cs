using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingCreator : MonoBehaviour
{
    BuildingType buildingType;

    private ResourceManager resourceManager;
    private GameSettings gameSettings;

    void Start()
    {
        resourceManager = GameObject.Find("Main Camera").GetComponent<ResourceManager>();
        gameSettings = GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings;
    }

    public void CreateBuilding(int building)
    {
        switch (building)
        {
            case (int)BuildingType.CityHall: //0
                if (resourceManager.amountOfWood >= gameSettings.cityhall.BuildingCostInWood && resourceManager.amountOfFood >= gameSettings.cityhall.BuildingCostInFood)
                {
                    resourceManager.amountOfWood -= gameSettings.cityhall.BuildingCostInWood;
                    resourceManager.amountOfFood -= gameSettings.cityhall.BuildingCostInFood;
                    InstantiateBuilding(gameSettings.cityhall.PrefabBuilding);
                }
                else
                {
                    Debug.Log("not enought resources");
                }
                break;

            case (int)BuildingType.FoodHouse: //1
                if (resourceManager.amountOfWood >= gameSettings.foodHouse.BuildingCostInWood && resourceManager.amountOfFood >= gameSettings.foodHouse.BuildingCostInFood)
                {
                    resourceManager.amountOfWood -= gameSettings.foodHouse.BuildingCostInWood;
                    resourceManager.amountOfFood -= gameSettings.foodHouse.BuildingCostInFood;
                    InstantiateBuilding(gameSettings.foodHouse.PrefabBuilding);
                }
                else
                {
                    Debug.Log("not enought resources");
                }
                break;

            case (int)BuildingType.WoodHouse: //2
                if (resourceManager.amountOfWood >= gameSettings.woodHouse.BuildingCostInWood && resourceManager.amountOfFood >= gameSettings.woodHouse.BuildingCostInFood)
                {
                    resourceManager.amountOfWood -= gameSettings.woodHouse.BuildingCostInWood;
                    resourceManager.amountOfFood -= gameSettings.woodHouse.BuildingCostInFood;
                    InstantiateBuilding(gameSettings.woodHouse.PrefabBuilding);
                }
                else
                {
                    Debug.Log("not enought resources");
                }
                break;

            case (int)BuildingType.Barrack: //3
                if (resourceManager.amountOfWood >= gameSettings.Barrack.BuildingCostInWood && resourceManager.amountOfFood >= gameSettings.Barrack.BuildingCostInFood)
                {
                    resourceManager.amountOfWood -= gameSettings.Barrack.BuildingCostInWood;
                    resourceManager.amountOfFood -= gameSettings.Barrack.BuildingCostInFood;
                    InstantiateBuilding(gameSettings.Barrack.PrefabBuilding);
                }
                else
                {
                    Debug.Log("not enought resources");
                }
                break;
        }
    }

    void InstantiateBuilding(GameObject gameObject)
    {
        GameObject newObj = Instantiate(gameObject);

        newObj.GetComponent<Building>().PreGround.SetActive(true);
        newObj.GetComponent<Building>().Ground.SetActive(false);

        GameObject.Find("Main Camera").GetComponent<AiManager>().Buildables.Add(newObj);

        StartCoroutine(waitForFeedback(newObj));        
    }

    IEnumerator waitForFeedback(GameObject newObj)
    {
        yield return new WaitForSeconds(0.5f);

        GameObject.Find("Main Camera").GetComponent<Grab>().grabbedItem = newObj;
        GameObject.Find("Main Camera").GetComponent<Grab>().grabbing = true;
    }
}
