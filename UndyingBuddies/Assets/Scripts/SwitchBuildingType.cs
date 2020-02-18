using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBuildingType : MonoBehaviour
{
    [SerializeField] private GameObject UI;
    [SerializeField] private AiManager _aiManager;
    [SerializeField] private ResourceManager _resourceManager;
    [SerializeField] private GameSettings _gameSettings;
    [SerializeField] private RecipeManager _recipeManager;

    public Building building;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //hit is the object that i want to grab
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "whiteSoulsStock" 
                    || hit.collider.tag == "blueVioletSoulsStock" 
                    || hit.collider.tag == "violetSoulsStock" 
                    || hit.collider.tag == "blueSoulsStock"
                    || hit.collider.tag == "redSoulsStock")
                {
                    building = hit.collider.GetComponentInParent<Building>();
                    _recipeManager.checkIfRecipeChange();
                    UI.SetActive(true);
                }
            }
        }
    }

    void RemoveFromAnyStock()
    {
        if (_aiManager.BlueVioletSoulStockage.Contains(building.gameObject))
        {
            _resourceManager.amountOfEnergy += building.currentStockage * _gameSettings.BlueVioletSoulValueInEnergy;
            _resourceManager.amountOfBlueViolet -= building.currentStockage;
            building.currentStockage = 0;
            building.UpdateStockVisu();
            _aiManager.BlueVioletSoulStockage.Remove(building.gameObject);
        }
        if (_aiManager.WhiteSoulStockage.Contains(building.gameObject))
        {
            _resourceManager.amountOfEnergy += building.currentStockage * _gameSettings.WhiteSoulValueInEnergy;
            _resourceManager.amountOfWhite -= building.currentStockage;
            building.currentStockage = 0;
            building.UpdateStockVisu();
            _aiManager.WhiteSoulStockage.Remove(building.gameObject);
        }
        if (_aiManager.VioletSoulStorage.Contains(building.gameObject))
        {
            _resourceManager.amountOfEnergy += building.currentStockage * _gameSettings.VioletSoulValueInEnergy;
            _resourceManager.amountOfViolet -= building.currentStockage;
            building.currentStockage = 0;
            building.UpdateStockVisu();
            _aiManager.VioletSoulStorage.Remove(building.gameObject);
        }
        if (_aiManager.BlueSoulStockage.Contains(building.gameObject))
        {
            _resourceManager.amountOfEnergy += building.currentStockage * _gameSettings.BlueSoulValueInEnergy;
            _resourceManager.amountOfBlue -= building.currentStockage;
            building.currentStockage = 0;
            building.UpdateStockVisu();
            _aiManager.BlueSoulStockage.Remove(building.gameObject);
        }
        if (_aiManager.RedSoulStorage.Contains(building.gameObject))
        {
            _resourceManager.amountOfEnergy += building.currentStockage * _gameSettings.RedSoulValueInEnergy;
            _resourceManager.amountOfRed -= building.currentStockage;
            building.currentStockage = 0;
            building.UpdateStockVisu();
            _aiManager.RedSoulStorage.Remove(building.gameObject);
        }
    }

    public void SwitchResourceType(ResourceType type)
    {
        switch (type)
        {
            case ResourceType.whiteSoul:
                building.resourceProducedAtBuilding = ResourceType.whiteSoul;
                RemoveFromAnyStock();
                building.StockPileTrigger.tag = "whiteSoulsStock";
                _aiManager.WhiteSoulStockage.Add(building.gameObject);
                break;

            case ResourceType.blueVioletSoul:
                building.resourceProducedAtBuilding = ResourceType.blueVioletSoul;
                RemoveFromAnyStock();
                building.StockPileTrigger.tag = "blueVioletSoulsStock";
                _aiManager.BlueVioletSoulStockage.Add(building.gameObject);
                break;
            case ResourceType.violetSoul:
                building.resourceProducedAtBuilding = ResourceType.violetSoul;
                RemoveFromAnyStock();
                building.StockPileTrigger.tag = "violetSoulsStock";
                _aiManager.VioletSoulStorage.Add(building.gameObject);
                break;
            case ResourceType.blueSoul:
                building.resourceProducedAtBuilding = ResourceType.blueSoul;
                RemoveFromAnyStock();
                building.StockPileTrigger.tag = "blueSoulsStock";
                _aiManager.BlueSoulStockage.Add(building.gameObject);
                break;
            case ResourceType.redSoul:
                building.resourceProducedAtBuilding = ResourceType.redSoul;
                RemoveFromAnyStock();
                building.StockPileTrigger.tag = "redSoulsStock";
                _aiManager.RedSoulStorage.Add(building.gameObject);
                break;
        }
    }
}
