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

    void Start()
    {
        SetupCityPrep();

        AttributeAiToPriestList();

        AttributeBonus();

        SetupAiPerTownType();

        StartCoroutine(SlowUpdate());
    }

    void AttributeAiToPriestList()
    {
        for (int i = 0; i < AllRelatedAIOfThisTown.Count; i++)
        {
            if (!AllRelatedAIOfThisTown[i].GetComponent<AIPriest>().AmIBuilding)
            {
                AllPriestUnit.Add(AllRelatedAIOfThisTown[i]);
            }
        }
    }

    void SetupAiPerTownType()
    {
        if (isCamp)
        {
            StartCoroutine(CampUpdate());

            for (int i = 0; i < AllPriestUnit.Count; i++)
            {
                AllPriestUnit[i].GetComponent<AIPriest>().Target = null;
                AllPriestUnit[i].GetComponent<AIPriest>()._myAIPriestType = AIPriestType.Camper;
                AllPriestUnit[i].GetComponent<AIPriest>().CanAttackBack = true;
            }
        }
        else
        {
            for (int i = 0; i < AllPriestUnit.Count; i++)
            {
                AllPriestUnit[i].GetComponent<AIPriest>()._myAIPriestType = AIPriestType.TownCitizen;
                AllPriestUnit[i].GetComponent<AIPriest>().CanAttackBack = true;
            }
        }
    }

    void AttributeBonus()
    {
        if (AiCityBonus.Count > 0)
        {
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

    }

    void SetupCityPrep()
    {
        if (CityResistanceMentalImage != null)
        {
            CityResistanceMentalImage.enabled = false;
        }
        if (CityResistancePhysicalImage != null)
        {
            CityResistancePhysicalImage.enabled = false;
        }
    }

    void CleanupUnessesaryEmpty()
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
    }

    void GenerateARandomBuildingToGoTo(AIPriest aiPriest)
    {
        aiPriest.Target = BuildingToWalkTo[Random.Range(0, BuildingToWalkTo.Length)];
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
        CleanupUnessesaryEmpty();

        for (int i = 0; i < AllPriestUnit.Count; i++)
        {
            AIPriestType aIPriestType;
            AIPriest currentAIPriest;

            currentAIPriest = AllPriestUnit[i];
            aIPriestType = currentAIPriest._myAIPriestType;

            if (currentAIPriest.healthAmount <= 0)
            {
                currentAIPriest.Die(1);
                CleanupUnessesaryEmpty();
                yield return new WaitForSeconds(0.5f);
                StartCoroutine(SlowUpdate());
                yield break;
            }
            else if (currentAIPriest.MentalHealthAmount >= currentAIPriest.MentalHealthMaxAmount)
            {
                currentAIPriest.Die(0);
                CleanupUnessesaryEmpty();
                yield return new WaitForSeconds(0.5f);
                StartCoroutine(SlowUpdate());
                yield break;
            }

            if (Revenge && currentAIPriest._myAIPriestType != AIPriestType.Rusher)
            {
                currentAIPriest._myAIPriestType = AIPriestType.Rusher;
                currentAIPriest.Target = null;
            }

            if (currentAIPriest.healthAmount < currentAIPriest.maxHealth || currentAIPriest.MentalHealthAmount > 0)
            {
                Revenge = true;
            }

            if (currentAIPriest.AmUnderEffect)
            {
                switch (currentAIPriest.currentAiPriestEffects)
                {
                    case AiPriestEffects.OnFire:
                        yield return new WaitForSeconds(0.5f);
                        StartCoroutine(SlowUpdate());
                        yield break;

                    case AiPriestEffects.Feared:
                        yield return new WaitForSeconds(0.5f);
                        StartCoroutine(SlowUpdate());
                        yield break;

                    case AiPriestEffects.Stun:
                        yield return new WaitForSeconds(0.5f);
                        StartCoroutine(SlowUpdate());
                        yield break;
                }
            }

            switch (aIPriestType)
            {
                case AIPriestType.TownCitizen:

                    if (currentAIPriest.Target != null)
                    {
                        if (Vector3.Distance(currentAIPriest.transform.position, currentAIPriest.Target.transform.position) < 2)
                        {
                            GenerateARandomBuildingToGoTo(currentAIPriest);
                            currentAIPriest.Idle();
                        }
                        else
                        {
                            currentAIPriest.Walk();
                        }
                    }
                    else
                    {
                        GenerateARandomBuildingToGoTo(currentAIPriest);
                    }
                    break;

                case AIPriestType.Rusher:

                    CleanupUnessesaryEmpty();

                    currentAIPriest.CheckClosestDemonToAttack();

                    if (currentAIPriest.Target != null)
                    {
                        if (Vector3.Distance(currentAIPriest.transform.position, currentAIPriest.Target.transform.position) <= _gameSettings.demonRangeOfCloseBy)
                        {
                            currentAIPriest.Attack();
                        }
                        else
                        {
                            currentAIPriest.Walk();
                        }
                    }
                    else
                    {
                        CleanupUnessesaryEmpty();
                        currentAIPriest.CheckClosestDemonToAttack();
                        currentAIPriest.Idle();
                    }
                    break;

                case AIPriestType.Camper:

                    CleanupUnessesaryEmpty();

                    if (weNeedToPrepare)
                    {
                        StartCoroutine(waitForRaid());

                        currentAIPriest.Target = this.gameObject;

                        if (Vector3.Distance(currentAIPriest.transform.position, currentAIPriest.Target.transform.position) < 2)
                        {
                            currentAIPriest.Idle();
                        }
                        else
                        {
                            currentAIPriest.Walk();
                        }

                        if (RevengeCamp)
                        {
                            currentAIPriest._myAIPriestType = AIPriestType.Rusher;
                        }
                    }
                    else
                    {
                        if (!currentAIPriest.CanLookAround)
                        {
                            if (currentAIPriest.TargeForRandom == null)
                            {
                                currentAIPriest.TargeForRandom = new GameObject();
                                currentAIPriest.TargeForRandom.name = this.name + " TargetForRandom";

                                currentAIPriest.TargeForRandom.transform.position = currentAIPriest.FindRandomPositionNearMe(this.gameObject); // this is the camp

                                currentAIPriest.Target = currentAIPriest.TargeForRandom;
                            }

                            if (Vector3.Distance(currentAIPriest.transform.position, currentAIPriest.Target.transform.position) < 2)
                            {
                                currentAIPriest.CanLookAround = true;
                            }
                            else
                            {
                                currentAIPriest.Walk();
                            }
                        }
                        else
                        {
                            currentAIPriest.Observe();
                        }
                    }
                    break;
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

    IEnumerator waitForRaid()
    {
        yield return new WaitForSeconds(_gameSettings.timeToPrepareWithACamp);
        RevengeCamp = true;
    }
}
