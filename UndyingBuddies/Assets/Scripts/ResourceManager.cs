using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    public int amountOfWood;
    public int amountOfFood;
    public int amountOfEnergy;

    public Text textWood;
    public Text textFood;
    public Text textEnergy;

    public List<GameObject> Resource = new List<GameObject>();

    [SerializeField] private AiManager AiManager;

    void Start()
    {
        foreach (var resource in GameObject.FindGameObjectsWithTag("Resource"))
        {
            if (resource.GetComponent<CharacterTypeTagger>().characterType == CharacterType.demon)
            {
                Resource.Add(resource);
            }
        }

        StartCoroutine(SlowUpdate());
    }

    public void ManageCostOfPurchaseDemon()
    {
        if (AiManager.GameSettings.CostOfNewDemonFood > 0)
        {
            for (int i = 0; i < AiManager.GameSettings.CostOfNewDemonFood; i++)
            {
                List<GameObject> foodStockToTakeFrom = new List<GameObject>();

                foreach (var foodStock in AiManager.FoodStockageBuilding)
                {
                    if (foodStock.GetComponent<Building>().currentStockage > 0)
                    {
                        foodStockToTakeFrom.Add(foodStock);
                    }
                }

                int rand = Random.Range(0, foodStockToTakeFrom.Count);
                foodStockToTakeFrom[rand].GetComponent<Building>().currentStockage -= 1;
                foodStockToTakeFrom[rand].GetComponent<Building>().UpdateStockVisu();

                if (foodStockToTakeFrom[rand].GetComponent<Building>().currentStockage <= 0)
                {
                    foodStockToTakeFrom.Remove(foodStockToTakeFrom[rand]);
                }
            }
        }
    }

    public void ManageCostOfPurchaseForBuilding(BuildingArchetype buildingArchetype)
    {
        if (buildingArchetype.BuildingCostInFood > 0)
        {
            for (int i = 0; i < buildingArchetype.BuildingCostInFood; i++)
            {
                List<GameObject> foodStockToTakeFrom = new List<GameObject>();

                foreach (var foodStock in AiManager.FoodStockageBuilding)
                {
                    if (foodStock.GetComponent<Building>().currentStockage > 0)
                    {
                        foodStockToTakeFrom.Add(foodStock);
                    }
                }

                int rand = Random.Range(0, foodStockToTakeFrom.Count);
                foodStockToTakeFrom[rand].GetComponent<Building>().currentStockage -= 1;
                foodStockToTakeFrom[rand].GetComponent<Building>().UpdateStockVisu();

                if (foodStockToTakeFrom[rand].GetComponent<Building>().currentStockage <= 0)
                {
                    foodStockToTakeFrom.Remove(foodStockToTakeFrom[rand]);
                }
            }
        }

        if (buildingArchetype.BuildingCostInWood > 0)
        {
            for (int i = 0; i < buildingArchetype.BuildingCostInWood; i++)
            {
                List<GameObject> woodStockToTakeFrom = new List<GameObject>();

                foreach (var woodStock in AiManager.WoodStockageBuilding)
                {
                    if (woodStock.GetComponent<Building>().currentStockage > 0)
                    {
                        woodStockToTakeFrom.Add(woodStock);
                    }
                }

                int rand = Random.Range(0, woodStockToTakeFrom.Count);
                woodStockToTakeFrom[rand].GetComponent<Building>().currentStockage -= 1;
                woodStockToTakeFrom[rand].GetComponent<Building>().UpdateStockVisu();

                if (woodStockToTakeFrom[rand].GetComponent<Building>().currentStockage <= 0)
                {
                    woodStockToTakeFrom.Remove(woodStockToTakeFrom[rand]);
                }
            }
        }
    }

    IEnumerator SlowUpdate()
    {
        amountOfFood = 0;
        for (int i = 0; i < GameObject.Find("Main Camera").GetComponent<AiManager>().FoodStockageBuilding.Count; i++)
        {
            amountOfFood += GameObject.Find("Main Camera").GetComponent<AiManager>().FoodStockageBuilding[i].GetComponent<Building>().currentStockage;
        }

        amountOfWood = 0;
        for (int i = 0; i < GameObject.Find("Main Camera").GetComponent<AiManager>().WoodStockageBuilding.Count; i++)
        {
            amountOfWood += GameObject.Find("Main Camera").GetComponent<AiManager>().WoodStockageBuilding[i].GetComponent<Building>().currentStockage;
        }

        textWood.text = amountOfWood.ToString();
        textFood.text = amountOfFood.ToString();
        textEnergy.text = amountOfEnergy.ToString();

        yield return new WaitForSeconds(0.1f);

        StartCoroutine(SlowUpdate());
    }
}
