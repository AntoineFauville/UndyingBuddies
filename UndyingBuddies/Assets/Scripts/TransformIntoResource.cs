using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformIntoResource : MonoBehaviour
{
    public ResourceType myResourceType;

    [SerializeField] private GameSettings gameSettings;

    public bool haveIGotGrabbed;
    public bool instantiateOnce;

    void Update()
    {
        if (!haveIGotGrabbed && this.transform.GetComponent<Grabable>().grabbed)
        {
            haveIGotGrabbed = true;
        }

        if (haveIGotGrabbed && !this.transform.GetComponent<Grabable>().grabbed)//when i release the object, it turns into resources
        {
            Transform();
        }
    }

    void Transform()
    {
        GameObject newResource;

        if (myResourceType == ResourceType.wood)
        {
            if (!instantiateOnce)
            {
                instantiateOnce = true;
                newResource = Instantiate(gameSettings.woodResourcePrefab, this.transform.position, new Quaternion());
                GameObject.Find("Main Camera").GetComponent<AiManager>().AddResource(newResource);
                Clean();
            }
        }
        else if (myResourceType == ResourceType.food)
        {
            if (!instantiateOnce)
            {
                instantiateOnce = true;
                newResource = Instantiate(gameSettings.foodResourcePrefab, this.transform.position, new Quaternion());
                GameObject.Find("Main Camera").GetComponent<AiManager>().AddResource(newResource);
                Clean();
            }
        } 
        else if (myResourceType == ResourceType.energy)
        {
            if (!instantiateOnce)
            {
                instantiateOnce = true;
                newResource = Instantiate(gameSettings.energyResourcePrefab, this.transform.position, new Quaternion());
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
