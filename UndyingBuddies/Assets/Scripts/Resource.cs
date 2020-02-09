using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public ResourceType resourceType;
    public ResourceSizeType resourceSizeType;

    public int amountOfResourceAvailable;

    public UiHealth uiHealth;

    public bool processedResource;

    void Start()
    {
        if (resourceType == ResourceType.food)
        {
            if (resourceSizeType == ResourceSizeType.smoll)
            {
                amountOfResourceAvailable = GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.foodSmallContainer;
            }
            else if (resourceSizeType == ResourceSizeType.medium)
            {
                amountOfResourceAvailable = GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.foodMediumContainer;
            }
        }
        else if (resourceType == ResourceType.wood)
        {
            if (resourceSizeType == ResourceSizeType.smoll)
            {
                amountOfResourceAvailable = GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.woodSmallContainer;
            }
            else if (resourceSizeType == ResourceSizeType.medium)
            {
                amountOfResourceAvailable = GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.woodMediumContainer;
            }
        }

        if (uiHealth != null)
        {
            uiHealth.life = amountOfResourceAvailable;
            uiHealth.maxLife = amountOfResourceAvailable;
        }
    }

    void Update()
    {
        if (uiHealth != null)
        {
            uiHealth.life = amountOfResourceAvailable;
        }

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
