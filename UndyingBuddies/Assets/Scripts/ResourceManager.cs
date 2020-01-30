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

    IEnumerator SlowUpdate()
    {
        amountOfFood = 0;
        for (int i = 0; i < GameObject.Find("Main Camera").GetComponent<AiManager>().FoodStockageBuilding.Count; i++)
        {
            amountOfFood += GameObject.Find("Main Camera").GetComponent<AiManager>().FoodStockageBuilding[i].GetComponent<Building>().currentStockage;
        }
        
        textWood.text = amountOfWood.ToString();
        textFood.text = amountOfFood.ToString();
        textEnergy.text = amountOfEnergy.ToString();

        yield return new WaitForSeconds(0.1f);

        StartCoroutine(SlowUpdate());
    }
}
