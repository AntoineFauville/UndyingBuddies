using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITown : MonoBehaviour
{
    [SerializeField] private List<AIPriest> AllRelatedAIOfThisTown = new List<AIPriest>();
    [SerializeField] private List<AIPriest> AllPriestUnit = new List<AIPriest>();

    bool Revenge;

    [SerializeField] private GameObject waveController;

    [SerializeField] private GameObject buildingToDestroy;
    [SerializeField] private GameObject visualsToShowActivation;

    [SerializeField] private AiCityBonus[] AiCityBonus;
    [SerializeField] private AiCityBonus ActiveAiCityBonus;

    void Awake()
    {
        waveController.GetComponent<WaveSpawner>().enabled = false;

        foreach (var priest in AllRelatedAIOfThisTown)
        {
            if (!priest.GetComponent<AIPriest>().AmIBuilding) {

                AllPriestUnit.Add(priest);
            }
        }

        //select random bonus for city
        int randCityBonus = Random.Range(0, AiCityBonus.Length);
        ActiveAiCityBonus = AiCityBonus[randCityBonus];

        if (AiCityBonus != null) {
            for (int i = 0; i < AllPriestUnit.Count; i++)
            {
                AllPriestUnit[i].GetComponent<AIStatController>().PhysicalResistance = ActiveAiCityBonus.PhysicalResistanceBonus;
                AllPriestUnit[i].GetComponent<AIStatController>().MentalHealthResistance = ActiveAiCityBonus.MentalHealthResistanceBonus;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < AllRelatedAIOfThisTown.Count; i++)
        {
            if (AllRelatedAIOfThisTown[i] == null)
            {
                AllRelatedAIOfThisTown.Remove(AllRelatedAIOfThisTown[i]);
            }
        }

        for (int i = 0; i < AllRelatedAIOfThisTown.Count; i++)
        {
            if (AllRelatedAIOfThisTown[i].healthAmount < AllRelatedAIOfThisTown[i].maxHealth)
            {
                Revenge = true;
            }
        }

        if (Revenge)
        {
            for (int i = 0; i < AllRelatedAIOfThisTown.Count; i++)
            {
                AllRelatedAIOfThisTown[i].PriestAttackerType = PriestAttackerType.rusher;
            }

            if (buildingToDestroy != null)
            {
                if (visualsToShowActivation != null)
                {
                    visualsToShowActivation.SetActive(true);
                }
                waveController.GetComponent<WaveSpawner>().enabled = true;
            }
            else
            {
                waveController.GetComponent<WaveSpawner>().enabled = false;
            }
        }
        else
        {
            waveController.GetComponent<WaveSpawner>().enabled = false;
        }
    }
}
