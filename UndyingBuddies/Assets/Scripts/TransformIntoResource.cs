using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformIntoResource : MonoBehaviour
{
    public ResourceType myResourceType;
    public ResourceSizeType resourceSizeType;

    [SerializeField] private GameSettings gameSettings;

    public bool haveIGotGrabbed;
    public bool instantiateOnce;

    public GameObject spawnPoint;
    public GameObject BuildingWhereImPlaced;

    private bool CanTransform;

    void Start()
    {
        if (gameSettings != null)
        {
            gameSettings = GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings;
        }
    }

    void Update()
    {
        if (!haveIGotGrabbed && this.transform.GetComponent<Grabable>().grabbed)
        {
            haveIGotGrabbed = true;
        }

        if (CanTransform)//when i release the object, it turns into resources
        {
            TransformIntoOtherResource();
        }
    }

    public void TransformIntoOtherResource()
    {
        GameObject newResource;

        if (myResourceType == ResourceType.wood)
        {
            if (!instantiateOnce)
            {
                instantiateOnce = true;
                newResource = Instantiate(gameSettings.woodResourcePrefab, spawnPoint.transform.position, new Quaternion());
                if (resourceSizeType == ResourceSizeType.smoll)
                {
                    newResource.GetComponent<Resource>().amountOfResourceAvailable = gameSettings.woodSmallContainer;
                }
                else if (resourceSizeType == ResourceSizeType.medium)
                {
                    newResource.GetComponent<Resource>().amountOfResourceAvailable = gameSettings.woodMediumContainer;
                }
                
                GameObject.Find("Main Camera").GetComponent<AiManager>().AddResource(newResource);
                Clean();
            }
        }
        else if (myResourceType == ResourceType.food)
        {
            if (!instantiateOnce)
            {
                instantiateOnce = true;
                newResource = Instantiate(gameSettings.foodResourcePrefab, spawnPoint.transform.position, new Quaternion());
                if (resourceSizeType == ResourceSizeType.smoll)
                {
                    newResource.GetComponent<Resource>().amountOfResourceAvailable = gameSettings.foodSmallContainer;
                }
                else if (resourceSizeType == ResourceSizeType.medium)
                {
                    newResource.GetComponent<Resource>().amountOfResourceAvailable = gameSettings.foodMediumContainer;
                }
                GameObject.Find("Main Camera").GetComponent<AiManager>().AddResource(newResource);
                Clean();
            }
        } 
        else if (myResourceType == ResourceType.energy)
        {
            if (!instantiateOnce)
            {
                instantiateOnce = true;
                newResource = Instantiate(gameSettings.energyResourcePrefab, spawnPoint.transform.position, new Quaternion());
                newResource.GetComponent<Resource>().amountOfResourceAvailable = gameSettings.energyAmount;
                GameObject.Find("Main Camera").GetComponent<AiManager>().AddResource(newResource);
                Clean();
            }
        }
    }

    void Clean()
    {
        this.transform.gameObject.SetActive(false);
    }
}
