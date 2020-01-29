using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDemons : MonoBehaviour
{
    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private GameSettings gameSettings;

    [SerializeField] private AiManager AiManager;

    public void SpawnNewDemon()
    {
        if (resourceManager.amountOfFood >= gameSettings.CostOfNewDemonFood)
        {
            resourceManager.amountOfFood -= gameSettings.CostOfNewDemonFood;

            GameObject demon = Instantiate(gameSettings.DemonPrefab, GameObject.Find("CityHall").GetComponent<Building>().SpawningPoint_01.transform.position, new Quaternion());
            
            demon.GetComponent<AIDemons>().Setup(demon.name, JobType.IdleVillager, gameSettings.demonLife, gameSettings.demonRangeOfDetection, gameSettings.demonRangeOfCloseBy);

            AiManager.Demons.Add(demon);
        }
    }
}
