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
            if (grabbedObject.GetComponent<Resource>().resourceType == ResourceType.whiteSoul)
            {
                resourceManager.amountOfEnergy += gameSettings.EnergyOnSacrificeSouls;

                int rand;
                rand = Random.Range(0, _aiManager.WhiteSoulStockage.Count);
                _aiManager.WhiteSoulStockage[rand].GetComponent<Building>().currentStockage += gameSettings.SoulsOnSacrificeResource;
                _aiManager.WhiteSoulStockage[rand].GetComponent<Building>().UpdateStockVisu();
            }
            else if (grabbedObject.GetComponent<Resource>().resourceType == ResourceType.energy)
            {
                resourceManager.amountOfEnergy += gameSettings.EnergyGetOutOfSacrificingHouse;
            }
        }

        if (grabbedObject.GetComponent<AIDemons>() != null)
        {
            resourceManager.amountOfEnergy += gameSettings.EnergyOutOfDemonSacrifice;

            if (grabbedObject.GetComponent<AIDemons>().AssignedBuilding != null)
            {
                grabbedObject.GetComponent<AIDemons>().AssignedBuilding.GetComponent<Building>().amountOfActiveWorker -= 1;
                grabbedObject.GetComponent<AIDemons>().AssignedBuilding = null;
            }
        }

        CleanFromAiManagerAnyResidualInconveniences(grabbedObject);
    }

    void CleanFromAiManagerAnyResidualInconveniences(GameObject grabbedObject)
    {
        if (_aiManager.Demons.Contains(grabbedObject))
        {
            _aiManager.Demons.Remove(grabbedObject);
        }
        else if (_aiManager.ResourceToProcess.Contains(grabbedObject))
        {
            _aiManager.ResourceToProcess.Remove(grabbedObject);
        }

        for (int i = 0; i < _particleSystem.Length; i++)
        {
            _particleSystem[i].GetComponent<ParticleSystem>().Play();
        }

        DestroyImmediate(grabbedObject);
    }

    public void TransformIntoEnergy(ResourceType resourceType, GameObject stockGameObject, Animator handAnim)
    {
        if (stockGameObject.GetComponent<Building>().currentStockage > 0)
        {
            stockGameObject.GetComponent<Building>().currentStockage -= 1;
            stockGameObject.GetComponent<Building>().UpdateStockVisu();

            if (resourceType == ResourceType.whiteSoul)
            {
                handAnim.Play("hand anim Sacrifice");
                resourceManager.amountOfEnergy += gameSettings.SoulConversionOnStockpile;

                for (int i = 0; i < _particleSystem.Length; i++)
                {
                    _particleSystem[i].GetComponent<ParticleSystem>().Play();
                }
            }
        }
    }
}
