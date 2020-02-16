using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBuildingType : MonoBehaviour
{
    [SerializeField] private GameObject UI;
    [SerializeField] private GameObject white;
    [SerializeField] private GameObject blueViolet;
    [SerializeField] private GameObject violet;
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
                if (hit.collider.tag == "whiteSoulsStock" || hit.collider.tag == "blueVioletSoulsStock" || hit.collider.tag == "violetSoulsStock")
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
            _resourceManager.amountOfWhite -= building.currentStockage;
            building.currentStockage = 0;
            building.UpdateStockVisu();
            _aiManager.VioletSoulStorage.Remove(building.gameObject);
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
        }
    }
}
