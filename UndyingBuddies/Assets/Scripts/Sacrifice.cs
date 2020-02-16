using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sacrifice : MonoBehaviour
{
    [SerializeField] private GameSettings gameSettings;
    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private GameObject[] _particleSystem;
    AiManager _aiManager;

    [SerializeField] private GameObject spawnPointCanvas;

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
            if (grabbedObject.GetComponent<Resource>().resourceType == ResourceType.brokenSoul)
            {
                resourceManager.amountOfEnergy += gameSettings.BrokenSoulValueInEnergy;

                CanvasDamage canvasDamage = Instantiate(gameSettings.CanvasDamagePrefab, spawnPointCanvas.transform.position, new Quaternion());
                canvasDamage.SetupCanvasEnergy(gameSettings.BrokenSoulValueInEnergy);
            }
            else if (grabbedObject.GetComponent<Resource>().resourceType == ResourceType.energy)
            {
                resourceManager.amountOfEnergy += gameSettings.EnergyGetOutOfSacrificingHouse;

                CanvasDamage canvasDamage = Instantiate(gameSettings.CanvasDamagePrefab, spawnPointCanvas.transform.position, new Quaternion());
                canvasDamage.SetupCanvasEnergy(gameSettings.EnergyGetOutOfSacrificingHouse);
            }
        }

        if (grabbedObject.GetComponent<AIDemons>() != null)
        {
            resourceManager.amountOfEnergy += gameSettings.EnergyOutOfDemonSacrifice;

            CanvasDamage canvasDamage = Instantiate(gameSettings.CanvasDamagePrefab, spawnPointCanvas.transform.position, new Quaternion());
            canvasDamage.SetupCanvasEnergy(gameSettings.EnergyOutOfDemonSacrifice);

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

    public void TransformIntoEnergy(ResourceType resourceType, RaycastHit hit, Animator handAnim)
    {
        if (hit.transform.gameObject.GetComponent<Building>().currentStockage > 0)
        {
            hit.transform.gameObject.GetComponent<Building>().currentStockage -= 1;
            hit.transform.gameObject.GetComponent<Building>().UpdateStockVisu();

            if (resourceType == ResourceType.whiteSoul)
            {
                handAnim.Play("hand anim Sacrifice");
                resourceManager.amountOfEnergy += gameSettings.WhiteSoulValueInEnergy;

                for (int i = 0; i < _particleSystem.Length; i++)
                {
                    _particleSystem[i].GetComponent<ParticleSystem>().Play();
                }

                CanvasDamage canvasDamage = Instantiate(GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.CanvasDamagePrefab, hit.collider.transform.position, new Quaternion());
                canvasDamage.transform.localScale = new Vector3(2, 2, 2);
                canvasDamage.SetupCanvasEnergy(GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.WhiteSoulValueInEnergy);
            }

            if (resourceType == ResourceType.blueVioletSoul)
            {
                handAnim.Play("hand anim Sacrifice");
                resourceManager.amountOfEnergy += gameSettings.BlueVioletSoulValueInEnergy;

                for (int i = 0; i < _particleSystem.Length; i++)
                {
                    _particleSystem[i].GetComponent<ParticleSystem>().Play();
                }

                CanvasDamage canvasDamage = Instantiate(GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.CanvasDamagePrefab, hit.collider.transform.position, new Quaternion());
                canvasDamage.transform.localScale = new Vector3(2, 2, 2);
                canvasDamage.SetupCanvasEnergy(GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.BlueVioletSoulValueInEnergy);
            }

            if (resourceType == ResourceType.violetSoul)
            {
                handAnim.Play("hand anim Sacrifice");
                resourceManager.amountOfEnergy += gameSettings.VioletSoulValueInEnergy;

                for (int i = 0; i < _particleSystem.Length; i++)
                {
                    _particleSystem[i].GetComponent<ParticleSystem>().Play();
                }

                CanvasDamage canvasDamage = Instantiate(GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.CanvasDamagePrefab, hit.collider.transform.position, new Quaternion());
                canvasDamage.transform.localScale = new Vector3(2, 2, 2);
                canvasDamage.SetupCanvasEnergy(GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.VioletSoulValueInEnergy);
            }

            if (resourceType == ResourceType.blueSoul)
            {
                handAnim.Play("hand anim Sacrifice");
                resourceManager.amountOfEnergy += gameSettings.BlueSoulValueInEnergy;

                for (int i = 0; i < _particleSystem.Length; i++)
                {
                    _particleSystem[i].GetComponent<ParticleSystem>().Play();
                }

                CanvasDamage canvasDamage = Instantiate(GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.CanvasDamagePrefab, hit.collider.transform.position, new Quaternion());
                canvasDamage.transform.localScale = new Vector3(2, 2, 2);
                canvasDamage.SetupCanvasEnergy(GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.BlueSoulValueInEnergy);
            }

            if (resourceType == ResourceType.redSoul)
            {
                handAnim.Play("hand anim Sacrifice");
                resourceManager.amountOfEnergy += gameSettings.RedSoulValueInEnergy;

                for (int i = 0; i < _particleSystem.Length; i++)
                {
                    _particleSystem[i].GetComponent<ParticleSystem>().Play();
                }

                CanvasDamage canvasDamage = Instantiate(GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.CanvasDamagePrefab, hit.collider.transform.position, new Quaternion());
                canvasDamage.transform.localScale = new Vector3(2, 2, 2);
                canvasDamage.SetupCanvasEnergy(GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.RedSoulValueInEnergy);
            }
        }
    }
}
