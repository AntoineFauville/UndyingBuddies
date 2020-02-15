using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnDemons : MonoBehaviour
{
    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private GameSettings gameSettings;

    [SerializeField] private AiManager AiManager;

    [SerializeField] private string demonCost;
    [SerializeField] private Text textCostDisplay;
    [SerializeField] private GameObject spawnPoint;

    void Start()
    {
        textCostDisplay.text = demonCost + "\n" + gameSettings.CostOfNewDemon.ToString() + " Energy";
    }

    public void SpawnNewDemon()
    {
        if (resourceManager.amountOfEnergy >= gameSettings.CostOfNewDemon)
        {
            resourceManager.ManageCostOfPurchaseDemon();

            GameObject demon = Instantiate(gameSettings.DemonPrefab, spawnPoint.transform.position, new Quaternion());

            demon.transform.SetParent(spawnPoint.transform);

            demon.GetComponent<AIDemons>().Setup(demon.name, JobType.IdleVillager, gameSettings.demonLife, gameSettings.demonRangeOfDetection, gameSettings.demonRangeOfCloseBy);

            AiManager.Demons.Add(demon);
        }
    }
}
