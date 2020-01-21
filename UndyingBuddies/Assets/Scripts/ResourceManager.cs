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

    void Start()
    {
        amountOfWood = GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.initialWoodAmount;
        amountOfFood = GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.initialFoodAmount;
        amountOfEnergy = GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.initialEnergyAmount;
    }

    void Update()
    {
        textWood.text = amountOfWood.ToString();
        textFood.text = amountOfFood.ToString();
        textEnergy.text = amountOfEnergy.ToString();
    }
}
