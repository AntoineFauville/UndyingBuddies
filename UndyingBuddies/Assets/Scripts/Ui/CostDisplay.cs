using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CostDisplay : MonoBehaviour
{
    [SerializeField] private BuildingArchetype building;
    [SerializeField] private Text text;
    [SerializeField] private GameSettings gameSettings;

    // Start is called before the first frame update
    void Start()
    {
        text.text = building.TheName + "\n" + gameSettings.CostOfNewBuilding + " Energy";
    }
}
