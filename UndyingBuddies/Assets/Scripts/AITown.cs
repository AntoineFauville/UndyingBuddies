using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AITown : MonoBehaviour
{
    [SerializeField] private List<AIPriest> AllRelatedAIOfThisTown = new List<AIPriest>();
    [SerializeField] private List<AIPriest> AllPriestUnit = new List<AIPriest>();

    public bool Revenge;
    public bool RevengeCamp;
    
    [SerializeField] private GameObject buildingToDestroy;
    [SerializeField] private GameObject visualsToShowActivation;

    [SerializeField] private List<AiCityBonus> AiCityBonus;
    [SerializeField] private AiCityBonus ActiveAiCityBonus;

    [SerializeField] private Image CityResistanceMentalImage, CityResistancePhysicalImage;

    [SerializeField] private GameObject[] BuildingToWalkTo;

    public bool isTheVillageDestroyed;
    bool hasBeenDestroyedOnce;

    [SerializeField] private AiBuilding[] buildingToTransformInEnergy;

    public bool isCamp;
    public bool weNeedToPrepare;
    private GameObject ui;
    [SerializeField] private GameSettings _gameSettings;

    void Awake()
    {
        if (CityResistanceMentalImage != null)
        {
            CityResistanceMentalImage.enabled = false;
        }
        if (CityResistancePhysicalImage != null)
        {
            CityResistancePhysicalImage.enabled = false;
        }

        for (int i = 0; i < AllRelatedAIOfThisTown.Count; i++)
        {
            if (!AllRelatedAIOfThisTown[i].GetComponent<AIPriest>().AmIBuilding)
            {
                AllPriestUnit.Add(AllRelatedAIOfThisTown[i]);
            }
        }
        
        if (AiCityBonus.Count > 0) {
            //select random bonus for city
            int randCityBonus = Random.Range(0, AiCityBonus.Count);
            ActiveAiCityBonus = AiCityBonus[randCityBonus];

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

        if (isCamp)
        {
            StartCoroutine(CampUpdate());
        }
    }

    IEnumerator CampUpdate()
    {
        yield return new WaitForSeconds(60);
        for (int i = 0; i < AllPriestUnit.Count; i++)
        {
            AllPriestUnit[i].GetComponent<AIPriest>().MaxPositionCamper += _gameSettings.CampSightIncrease;
        }

        StartCoroutine(CampUpdate());
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

        if (weNeedToPrepare)
        {
            for (int i = 0; i < AllPriestUnit.Count; i++)
            {
                AllPriestUnit[i].GetComponent<AIPriest>().preparationForAttack = true;
            }

            if (ui == null)
            {
                ui = Instantiate(_gameSettings.UIAttack);
                ui.GetComponent<AttackFromPriestCamp>().seconds = _gameSettings.timeToPrepareWithACamp;
                ui.transform.SetParent(GameObject.Find("AttackContainer").transform);
                ui.transform.localScale = new Vector3(1, 1, 1);
                ui.transform.localPosition = new Vector3(0, 0, 0);
                ui.transform.rotation = new Quaternion();
            }
        }


        if (AllPriestUnit.Count <= 0)
        {
            isTheVillageDestroyed = true;
        }
        else
        {
            isTheVillageDestroyed = false;

            if (Revenge || RevengeCamp)
            {
                for (int i = 0; i < AllPriestUnit.Count; i++)
                {
                    if (AllPriestUnit[i] == null)
                    {
                        AllPriestUnit.Remove(AllPriestUnit[i]);
                    }
                    else
                    {
                        AllPriestUnit[i].PriestAttackerType = PriestAttackerType.rusherFromCity;

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
