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
        if (resourceType == ResourceType.brokenSoul)
        {
            amountOfResourceAvailable = GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.BrokenSoulsOnResource;
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
        if (resourceType == ResourceType.whiteSoul)
        {
            GameObject.Find("Main Camera").GetComponent<AiManager>().ResourceToProcess.Remove(this.gameObject);
        }

        DestroyImmediate(this.gameObject);
    }
}
