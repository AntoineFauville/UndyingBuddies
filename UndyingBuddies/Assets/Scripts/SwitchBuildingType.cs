using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBuildingType : MonoBehaviour
{
    [SerializeField] private GameObject UI;
    [SerializeField] private GameObject white;
    [SerializeField] private GameObject blueViolet;
    [SerializeField] private AiManager _aiManager;

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
                if (hit.collider.tag == "foodStock")
                {
                    building = hit.collider.GetComponentInParent<Building>();
                    if (building.resourceProducedAtBuilding == ResourceType.whiteSoul)
                    {
                        Reset();
                        white.SetActive(true);
                    }
                    else if (building.resourceProducedAtBuilding == ResourceType.whiteSoul)
                    {
                        Reset();
                        blueViolet.SetActive(true);
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
    }

    void RemoveFromAnyStock()
    {
        if (_aiManager.BlueVioletSoulStockage.Contains(building.gameObject))
        {
            _aiManager.BlueVioletSoulStockage.Remove(building.gameObject);
        }
        if (_aiManager.WhiteSoulStockage.Contains(building.gameObject))
        {
            _aiManager.WhiteSoulStockage.Remove(building.gameObject);
        }
    }

    public void SwitchResourceType(int type)
    {
        switch (type)
        {
            case 0:
                building.resourceProducedAtBuilding = ResourceType.whiteSoul;
                RemoveFromAnyStock();
                _aiManager.WhiteSoulStockage.Add(building.gameObject);
                break;

            case 1:
                building.resourceProducedAtBuilding = ResourceType.blueVioletSoul;
                RemoveFromAnyStock();
                _aiManager.BlueVioletSoulStockage.Add(building.gameObject);
                break;
        }
    }
}
