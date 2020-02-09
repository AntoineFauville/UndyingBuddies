﻿using System.Collections;
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

                int rand;
                rand = Random.Range(0, _aiManager.FoodStockageBuilding.Count);
                _aiManager.FoodStockageBuilding[rand].GetComponent<Building>().currentStockage += 1;
                _aiManager.FoodStockageBuilding[rand].GetComponent<Building>().UpdateStockVisu();
            }
            else if (grabbedObject.GetComponent<Resource>().resourceType == ResourceType.wood)
            {
                resourceManager.amountOfEnergy += 2;

                int rand;
                rand = Random.Range(0, _aiManager.WoodStockageBuilding.Count);
                _aiManager.WoodStockageBuilding[rand].GetComponent<Building>().currentStockage += 2;
                _aiManager.WoodStockageBuilding[rand].GetComponent<Building>().UpdateStockVisu();
            }
            else if (grabbedObject.GetComponent<Resource>().resourceType == ResourceType.energy)
            {
                resourceManager.amountOfEnergy += 5;
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

    public void TransformIntoEnergy(ResourceType resourceType, GameObject stockGameObject)
    {
        if (stockGameObject.GetComponent<Building>().currentStockage > 0)
        {
            stockGameObject.GetComponent<Building>().currentStockage -= 1;
            stockGameObject.GetComponent<Building>().UpdateStockVisu();

            if (resourceType == ResourceType.food)
            {
                resourceManager.amountOfEnergy += 1;
            }
            else if (resourceType == ResourceType.wood)
            {
                resourceManager.amountOfEnergy += 1;
            }
            else if (resourceType == ResourceType.energy)
            {
                resourceManager.amountOfEnergy += 1;
            }
        }
    }
}
