using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public ResourceType resourceType;

    public int amountOfResourceAvailable;

    public UiHealth uiHealth;

    public bool processedResource;

    void Start()
    {
        uiHealth.life = amountOfResourceAvailable;
        uiHealth.maxLife = amountOfResourceAvailable;
    }

    void Update()
    {
        uiHealth.life = amountOfResourceAvailable;

        if (amountOfResourceAvailable <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (resourceType == ResourceType.wood)
        {
            GameObject.Find("Main Camera").GetComponent<AiManager>().WoodToProcess.Remove(this.gameObject);

        }
        else if (resourceType == ResourceType.food)
        {
            GameObject.Find("Main Camera").GetComponent<AiManager>().FoodToProcess.Remove(this.gameObject);
        }

        DestroyImmediate(this.gameObject);
    }
}
