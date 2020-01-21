using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDemons : MonoBehaviour
{
    private ResourceManager resourceManager;
    private GameSettings gameSettings;

    void Start()
    {
        resourceManager = GameObject.Find("Main Camera").GetComponent<ResourceManager>();
        gameSettings = GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings;
    }


    public void SpawnNewDemon()
    {
        if (resourceManager.amountOfFood >= gameSettings.CostOfNewDemonFood)
        {
            resourceManager.amountOfFood -= gameSettings.CostOfNewDemonFood;

            GameObject demon = Instantiate(gameSettings.DemonPrefab, this.transform.GetComponent<Building>().SpawningPoint.transform.position, new Quaternion());
            
            demon.GetComponent<AIDemons>().Setup(demon.name, JobType.builder, gameSettings.demonLife, gameSettings.demonRangeOfDetection, gameSettings.demonRangeOfCloseBy);

            GameObject.Find("Main Camera").GetComponent<AiManager>().Demons.Add(demon);
        }
    }
}
