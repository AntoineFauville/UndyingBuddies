using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public ResourceType resourceType;

    public int amountOfResourceAvailable;

    public UiHealth uiHealth;

    public bool processedResource;

    [SerializeField] private GameObject ArtAlive;
    [SerializeField] private GameObject ArtDed;

    void Start()
    {
        ArtAlive.SetActive(true);
        ArtDed.SetActive(false);

        this.transform.localScale = new Vector3(Random.Range(0.75f, 1.25f), Random.Range(0.75f, 1.25f), Random.Range(0.75f, 1.25f));
        this.transform.rotation = Quaternion.Euler(0, Random.Range(180, -180), 0);

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
        if (resourceType == ResourceType.brokenSoul)
        {
            GameObject.Find("Main Camera").GetComponent<AiManager>().ResourceToProcess.Remove(this.gameObject);
        }

        uiHealth.gameObject.SetActive(false);
        ArtAlive.SetActive(false);
        ArtDed.SetActive(true);
    }
}
