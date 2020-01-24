using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public ResourceType resourceType;

    public int amountOfResourceAvailable;

    void Update()
    {
        if (amountOfResourceAvailable <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (resourceType == ResourceType.wood)
        {
            GameObject.Find("Main Camera").GetComponent<AiManager>().Woods.Remove(this.gameObject);

        }
        else if (resourceType == ResourceType.food)
        {
            GameObject.Find("Main Camera").GetComponent<AiManager>().Foods.Remove(this.gameObject);
        }

        DestroyImmediate(this.gameObject);
    }
}
