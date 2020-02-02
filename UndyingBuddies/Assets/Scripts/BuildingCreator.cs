using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingCreator : MonoBehaviour
{
    BuildingType buildingType;

    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private GameSettings gameSettings;
    
    public void CreateBuilding(int building)
    {
        switch (building)
        {
            case (int)BuildingType.CityHall: //0
                if (resourceManager.amountOfWood >= gameSettings.cityhall.BuildingCostInWood && resourceManager.amountOfFood >= gameSettings.cityhall.BuildingCostInFood)
                {
                    InstantiateBuilding(gameSettings.cityhall.PrefabBuilding, gameSettings.cityhall);
                }
                else
                {
                    Debug.Log("not enought resources");
                }
                break;

            case (int)BuildingType.FoodStock: //1
                if (resourceManager.amountOfWood >= gameSettings.foodHouse.BuildingCostInWood && resourceManager.amountOfFood >= gameSettings.foodHouse.BuildingCostInFood)
                {
                    InstantiateBuilding(gameSettings.foodHouse.PrefabBuilding, gameSettings.foodHouse);
                }
                else
                {
                    Debug.Log("not enought resources");
                }
                break;

            case (int)BuildingType.WoodStock: //2
                if (resourceManager.amountOfWood >= gameSettings.woodHouse.BuildingCostInWood && resourceManager.amountOfFood >= gameSettings.woodHouse.BuildingCostInFood)
                {
                    InstantiateBuilding(gameSettings.woodHouse.PrefabBuilding, gameSettings.woodHouse);
                }
                else
                {
                    Debug.Log("not enought resources");
                }
                break;

            case (int)BuildingType.Barrack: //3
                if (resourceManager.amountOfWood >= gameSettings.Barrack.BuildingCostInWood && resourceManager.amountOfFood >= gameSettings.Barrack.BuildingCostInFood)
                {
                    InstantiateBuilding(gameSettings.Barrack.PrefabBuilding, gameSettings.Barrack);
                }
                else
                {
                    Debug.Log("not enought resources");
                }
                break;

            case (int)BuildingType.WoodProcessor: //4
                if (resourceManager.amountOfWood >= gameSettings.woodCutter.BuildingCostInWood && resourceManager.amountOfFood >= gameSettings.woodCutter.BuildingCostInFood)
                {
                    InstantiateBuilding(gameSettings.woodCutter.PrefabBuilding, gameSettings.woodCutter);
                }
                else
                {
                    Debug.Log("not enought resources");
                }
                break;

            case (int)BuildingType.FoodProcessor: //5
                if (resourceManager.amountOfWood >= gameSettings.foodProcessor.BuildingCostInWood && resourceManager.amountOfFood >= gameSettings.foodProcessor.BuildingCostInFood)
                {
                    InstantiateBuilding(gameSettings.foodProcessor.PrefabBuilding, gameSettings.foodProcessor);
                }
                else
                {
                    Debug.Log("not enought resources");
                }
                break;

            case (int)BuildingType.EnergyGenerator: //6
                if (resourceManager.amountOfWood >= gameSettings.spellHouse.BuildingCostInWood && resourceManager.amountOfFood >= gameSettings.spellHouse.BuildingCostInFood)
                {
                    InstantiateBuilding(gameSettings.spellHouse.PrefabBuilding, gameSettings.spellHouse);
                }
                else
                {
                    Debug.Log("not enought resources");
                }
                break;
        }
    }

    void InstantiateBuilding(GameObject gameObject, BuildingArchetype buildingArchetype)
    {
        GameObject newObj = Instantiate(gameObject);

        newObj.GetComponent<Building>().buildingArchetype = buildingArchetype;

        StartCoroutine(waitForFeedback(newObj));        
    }

    IEnumerator waitForFeedback(GameObject newObj)
    {
        yield return new WaitForSeconds(0.05f);

        GameObject.Find("Main Camera").GetComponent<Grab>().grabbedItem = newObj;
        GameObject.Find("Main Camera").GetComponent<Grab>().grabbing = true;
    }
}
