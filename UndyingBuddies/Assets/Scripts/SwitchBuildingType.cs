using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBuildingType : MonoBehaviour
{
    [SerializeField] private GameObject UI;
    [SerializeField] private GameObject white;
    [SerializeField] private GameObject blueViolet;
    [SerializeField] private GameObject violet;
    [SerializeField] private GameObject blue;
    [SerializeField] private GameObject red;
    [SerializeField] private AiManager _aiManager;
    [SerializeField] private ResourceManager _resourceManager;
    [SerializeField] private GameSettings _gameSettings;

    private Building building;

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
                    if (building.resourceProducedAtBuilding == ResourceType.whiteSoul)
                    {
                        Reset();
                        white.SetActive(true);
                    }
                    else if (building.resourceProducedAtBuilding == ResourceType.blueVioletSoul)
                    {
                        Reset();
                        blueViolet.SetActive(true);
                    }
                    else if (building.resourceProducedAtBuilding == ResourceType.violetSoul)
                    {
                        Reset();
                        violet.SetActive(true);
                    }
                    else if (building.resourceProducedAtBuilding == ResourceType.blueSoul)
                    {
                        Reset();
                        blue.SetActive(true);
                    }
                    else if (building.resourceProducedAtBuilding == ResourceType.redSoul)
                    {
                        Reset();
                        red.SetActive(true);
                    }
                    UI.SetActive(true);
                }
            }
        }
    }

    void Reset()
    {
        white.SetActive(false);
        blueViolet.SetActive(false);
        violet.SetActive(false);
        blue.SetActive(false);
        red.SetActive(false);
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

    public void SwitchResourceType(int type)
    {
        switch (type)
        {
            case 0:
                building.resourceProducedAtBuilding = ResourceType.whiteSoul;
                RemoveFromAnyStock();
                building.StockPileTrigger.tag = "whiteSoulsStock";
                _aiManager.WhiteSoulStockage.Add(building.gameObject);
                break;

            case 1:
                building.resourceProducedAtBuilding = ResourceType.blueVioletSoul;
                RemoveFromAnyStock();
                building.StockPileTrigger.tag = "blueVioletSoulsStock";
                _aiManager.BlueVioletSoulStockage.Add(building.gameObject);
                break;
            case 2:
                building.resourceProducedAtBuilding = ResourceType.violetSoul;
                RemoveFromAnyStock();
                building.StockPileTrigger.tag = "violetSoulsStock";
                _aiManager.VioletSoulStorage.Add(building.gameObject);
                break;
            case 3:
                building.resourceProducedAtBuilding = ResourceType.blueSoul;
                RemoveFromAnyStock();
                building.StockPileTrigger.tag = "blueSoulsStock";
                _aiManager.BlueSoulStockage.Add(building.gameObject);
                break;
            case 4:
                building.resourceProducedAtBuilding = ResourceType.redSoul;
                RemoveFromAnyStock();
                building.StockPileTrigger.tag = "redSoulsStock";
                _aiManager.RedSoulStorage.Add(building.gameObject);
                break;
        }
    }
}
