using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    public int amountOfWhite;
    public int amountOfBlueViolet;
    public int amountOfEnergy;

    public Text textWood;
    public Text textFood;
    public Text textEnergy;

    public List<GameObject> Resource = new List<GameObject>();

    [SerializeField] private AiManager _aiManager;

    void Start()
    {
        foreach (var resource in GameObject.FindGameObjectsWithTag("Resource"))
        {
            if (resource.GetComponent<CharacterTypeTagger>().characterType == CharacterType.demon)
            {
                Resource.Add(resource);
            }
        }

        if (_aiManager == null)
        {
            _aiManager = GameObject.Find("Main Camera").GetComponent<AiManager>();
        }

        StartCoroutine(SlowUpdate());
    }

    public void ManageCostOfPurchaseDemon()
    {
        amountOfEnergy -= _aiManager.GameSettings.CostOfNewDemon;
    }

    public void ManageCostOfPurchaseForBuilding()
    {
        amountOfEnergy -= _aiManager.GameSettings.CostOfNewBuilding;
    }

    IEnumerator SlowUpdate()
    {
        amountOfWhite = 0;
        if (_aiManager.WhiteSoulStockage.Count <=  0)
        {
            amountOfWhite = 0;
        }
        else
        {
            for (int i = 0; i < _aiManager.WhiteSoulStockage.Count; i++)
            {
                if (_aiManager.WhiteSoulStockage[i] == null)
                {
                    _aiManager.WhiteSoulStockage.Remove(_aiManager.WhiteSoulStockage[i]);
                }
                else if (_aiManager.WhiteSoulStockage[i] != null)
                {
                    amountOfWhite += _aiManager.WhiteSoulStockage[i].GetComponent<Building>().currentStockage;
                }
            }
        }

        amountOfBlueViolet = 0;
        if (_aiManager.BlueVioletSoulStockage.Count <= 0)
        {
            amountOfBlueViolet = 0;
        }
        else
        {
            for (int i = 0; i < _aiManager.BlueVioletSoulStockage.Count; i++)
            {
                if (_aiManager.BlueVioletSoulStockage[i] == null)
                {
                    _aiManager.BlueVioletSoulStockage.Remove(_aiManager.BlueVioletSoulStockage[i]);
                }
                else if (_aiManager.BlueVioletSoulStockage[i] != null)
                {
                    amountOfBlueViolet += _aiManager.BlueVioletSoulStockage[i].GetComponent<Building>().currentStockage;
                }
            }
        }

        textWood.text = amountOfBlueViolet.ToString();
        textFood.text = amountOfWhite.ToString();
        textEnergy.text = amountOfEnergy.ToString();

        yield return new WaitForSeconds(0.02f);

        StartCoroutine(SlowUpdate());
    }
}
