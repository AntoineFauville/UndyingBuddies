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
            }
        }
        else
        {
            Debug.Log("place what you have in hand first");
        }
    }

    void InstantiateBuilding(GameObject gameObject, BuildingArchetype buildingArchetype)
    {
        GameObject newObj = Instantiate(gameObject);

        valueForName++;

        newObj.name = valueForName.ToString();

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
