using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sacrifice : MonoBehaviour
{
    [SerializeField] private GameSettings gameSettings;
    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private GameObject[] _particleSystem;
    AiManager _aiManager;

    void Start()
    {
        for (int i = 0; i < _particleSystem.Length; i++)
        {
            _particleSystem[i].GetComponent<ParticleSystem>().Stop();
        }
    }

    public void SacrificeForLordSavior(GameObject grabbedObject, AiManager aiManager)
    {
        _aiManager = aiManager;

        if (grabbedObject.GetComponent<Resource>() != null)
        {
            if (grabbedObject.GetComponent<Resource>().resourceType == ResourceType.food)
            {
                resourceManager.amountOfEnergy += 1;
            }
            else if (grabbedObject.GetComponent<Resource>().resourceType == ResourceType.wood)
            {
                resourceManager.amountOfEnergy += 2;
            }
        }

        if (grabbedObject.GetComponent<AIDemons>() != null)
        {
            resourceManager.amountOfEnergy += 5;
        }

        CleanFromAiManagerAnyResidualInconveniences(grabbedObject);
    }

    void CleanFromAiManagerAnyResidualInconveniences(GameObject grabbedObject)
    {
        if (_aiManager.Demons.Contains(grabbedObject))
        {
            _aiManager.Demons.Remove(grabbedObject);
        }
        else if (_aiManager.WoodToProcess.Contains(grabbedObject))
        {
            _aiManager.WoodToProcess.Remove(grabbedObject);
        }
        else if (_aiManager.FoodToProcess.Contains(grabbedObject))
        {
            _aiManager.FoodToProcess.Remove(grabbedObject);
        }

        for (int i = 0; i < _particleSystem.Length; i++)
        {
            _particleSystem[i].GetComponent<ParticleSystem>().Play();
        }

        DestroyImmediate(grabbedObject);
    }
}
