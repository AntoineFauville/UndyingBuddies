using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AITown : MonoBehaviour
{
    [SerializeField] private List<AIPriest> AllRelatedAIOfThisTown = new List<AIPriest>();
    [SerializeField] private List<AIPriest> AllPriestUnit = new List<AIPriest>();

    bool Revenge;
    
    [SerializeField] private GameObject buildingToDestroy;
    [SerializeField] private GameObject visualsToShowActivation;

    [SerializeField] private AiCityBonus[] AiCityBonus;
    [SerializeField] private AiCityBonus ActiveAiCityBonus;

    [SerializeField] private Image CityResistanceMentalImage, CityResistancePhysicalImage;

    [SerializeField] private GameObject[] BuildingToWalkTo;

    public bool isTheVillageDestroyed;
    bool hasBeenDestroyedOnce;

    [SerializeField] private AiBuilding[] buildingToTransformInEnergy;

    void Awake()
    {
        CityResistanceMentalImage.enabled = false;
        CityResistancePhysicalImage.enabled = false;

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

                if (AiCityBonus[randCityBonus].MentalHealthResistanceBonus > 0)
                {
                    CityResistanceMentalImage.enabled = true;
                }
                if (AiCityBonus[randCityBonus].PhysicalResistanceBonus > 0)
                {
                    CityResistancePhysicalImage.enabled = true;
                }
            }
        }

        StartCoroutine(animateAIInCity());

        StartCoroutine(SlowUpdate());
    }

    IEnumerator SlowUpdate()
    {
        for (int i = 0; i < AllRelatedAIOfThisTown.Count; i++)
        {
            if (AllRelatedAIOfThisTown[i] == null)
            {
                AllRelatedAIOfThisTown.Remove(AllRelatedAIOfThisTown[i]);
            }
        }

        for (int i = 0; i < AllPriestUnit.Count; i++)
        {
            if (AllPriestUnit[i] == null)
            {
                AllPriestUnit.Remove(AllPriestUnit[i]);
            }
        }

        if (AllPriestUnit.Count <= 0)
        {
            isTheVillageDestroyed = true;
        }
        else
        {
            isTheVillageDestroyed = false;

            if (Revenge)
            {
                for (int i = 0; i < AllPriestUnit.Count; i++)
                {
                    if (AllPriestUnit[i] == null)
                    {
                        AllPriestUnit.Remove(AllPriestUnit[i]);
                    }
                    else
                    {
                        AllPriestUnit[i].PriestAttackerType = PriestAttackerType.rusher;

                        AllPriestUnit[i].isAttacked = true;

                        if (AllPriestUnit[i].GetComponent<AIStatController>().PhysicalResistance > 0)
                        {
                            AllPriestUnit[i].GetComponent<AIPriest>().UiHealth.physicalResistance.enabled = true;
                        }
                        if (AllPriestUnit[i].GetComponent<AIStatController>().MentalHealthResistance > 0)
                        {
                            AllPriestUnit[i].GetComponent<AIPriest>().UiHealth.mentalResistance.enabled = true;
                        }
                    }
                }

                if (buildingToDestroy != null)
                {
                    if (visualsToShowActivation != null)
                    {
                        visualsToShowActivation.SetActive(true);
                    }
                }
            }
        }

        if (isTheVillageDestroyed && !hasBeenDestroyedOnce)
        {
            hasBeenDestroyedOnce = true;

            visualsToShowActivation.SetActive(false);

            for (int i = 0; i < buildingToTransformInEnergy.Length; i++)
            {
                buildingToTransformInEnergy[i].Destroy();
            }
        }

        for (int i = 0; i < AllRelatedAIOfThisTown.Count; i++)
        {
            if (AllRelatedAIOfThisTown[i].healthAmount < AllRelatedAIOfThisTown[i].maxHealth || AllRelatedAIOfThisTown[i].MentalHealthAmount > 0)
            {
                Revenge = true;
            }
        }

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(SlowUpdate());
    }

    IEnumerator animateAIInCity()
    {
        for (int i = 0; i < AllPriestUnit.Count; i++)
        {
            AllPriestUnit[i].buildingToWalkTo = BuildingToWalkTo[Random.Range(0, BuildingToWalkTo.Length)];
        }

        yield return new WaitForSeconds(5);

        StartCoroutine(animateAIInCity());
    }
}
