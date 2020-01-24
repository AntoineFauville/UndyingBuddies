using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerrainManagerLock : MonoBehaviour
{
    public List<GameObject> TerrainToUnlock = new List<GameObject>();
    public List<GameObject> AIOnMe = new List<GameObject>();
    public List<GameObject> ResourcesAvailable = new List<GameObject>();

    public TerrainStage terrainStage;

    ResourceManager resourceManager;
    GameSettings gameSettings;

    public GameObject TerrainUI;
    public GameObject killAllAIUI;
    public GameObject UnlockUI;

    void Start()
    {
        StartCoroutine(SlowUpdate());

        resourceManager = GameObject.Find("Main Camera").GetComponent<ResourceManager>();
        gameSettings = GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings;
    }

    public void UnlockTerrain()
    {
        if (AIOnMe.Count <= 0 && resourceManager.amountOfWood >= gameSettings.woodAmountToUnlockTerrain && resourceManager.amountOfFood >= gameSettings.foodAmountToUnlockTerrain)
        {
            terrainStage = TerrainStage.Unlocked;
        }
    }

    IEnumerator SlowUpdate()
    {
        switch (terrainStage)
        {
            case TerrainStage.CompleteLock:
                this.gameObject.tag = "Untagged";

                for (int i = 0; i < ResourcesAvailable.Count; i++)
                {
                    ResourcesAvailable[i].GetComponent<CharacterTypeTagger>().characterType = CharacterType.neutral;
                }

                TerrainUI.SetActive(false);

                break;

            case TerrainStage.SoftLock:
                this.gameObject.tag = "terrainWarFlagOnly";

                for (int i = 0; i < ResourcesAvailable.Count; i++)
                {
                    ResourcesAvailable[i].GetComponent<CharacterTypeTagger>().characterType = CharacterType.neutral;
                }

                TerrainUI.SetActive(true);

                if (AIOnMe.Count > 0)
                {
                    killAllAIUI.SetActive(true);
                    UnlockUI.SetActive(false);
                }
                else
                {
                    killAllAIUI.SetActive(false);
                    UnlockUI.SetActive(true);
                }

                break;

            case TerrainStage.Unlocked:
                this.gameObject.tag = "Floor";

                for (int i = 0; i < TerrainToUnlock.Count; i++)
                {
                    if (TerrainToUnlock[i].GetComponent<TerrainManagerLock>().terrainStage != TerrainStage.Unlocked)
                    {
                        TerrainToUnlock[i].GetComponent<TerrainManagerLock>().terrainStage = TerrainStage.SoftLock;
                    }
                }

                for (int i = 0; i < ResourcesAvailable.Count; i++)
                {
                    ResourcesAvailable[i].GetComponent<CharacterTypeTagger>().characterType = CharacterType.demon;
                }

                TerrainUI.SetActive(false);

                break;
        }

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(SlowUpdate());
    }
}
