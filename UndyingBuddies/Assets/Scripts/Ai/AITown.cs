using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AITown : MonoBehaviour
{
    [SerializeField] private List<AIPriest> AllRelatedAIOfThisTown = new List<AIPriest>();
    public List<AIPriest> AllPriestUnit = new List<AIPriest>();

    public bool Revenge;
    
    [SerializeField] private GameObject ChurchInMiddleOfTown;
    [SerializeField] private GameObject visualsToShowActivation;

    [SerializeField] private List<AiCityBonus> AiCityBonus;
    [SerializeField] private AiCityBonus ActiveAiCityBonus;

    [SerializeField] private Image CityResistanceMentalImage, CityResistancePhysicalImage;

    [SerializeField] private GameObject[] BuildingToWalkTo;
    [SerializeField] private List<GameObject> paths = new List<GameObject>();

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

        /*
        Collider[] Paths = Physics.OverlapSphere(aiPriest.transform.position, 5);

        paths.Clear();

        for (int i = 0; i < Paths.Length; i++)
        {
            if (Paths[i].gameObject.tag == "path")
            {
                paths.Add(Paths[i].gameObject);
            }
        }

        if (paths.Count <= 0)
        {
            aiPriest.Target = BuildingToWalkTo[Random.Range(0, BuildingToWalkTo.Length)];
        }
        else
        {
            aiPriest.Target = paths[Random.Range(0, paths.Count)];
        }*/
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
            yield return new WaitForSeconds(0.02f);

            AIPriestType aIPriestType;
            AIPriest currentAIPriest;
            
            currentAIPriest = AllPriestUnit[i];
            aIPriestType = currentAIPriest._myAIPriestType;

            currentAIPriest.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
            currentAIPriest.gameObject.GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, 0f, 0f);

            currentAIPriest.UiHealth.questionMark.enabled = false;

            if (currentAIPriest.myAiTown == null)
            {
                currentAIPriest.myAiTown = this;
            }

            if (currentAIPriest.healthAmount < currentAIPriest.maxHealth || currentAIPriest.MentalHealthAmount > 0)
            {
                Revenge = true;
            }

            if (Revenge && currentAIPriest._myAIPriestType != AIPriestType.Rusher)
            {
                currentAIPriest._myAIPriestType = AIPriestType.Rusher;
                currentAIPriest.Target = null;
            }

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
            
            if (currentAIPriest.AmUnderEffect)
            {
                //if i'm already under an effect what do i do ? i have to prioritize!
                //fear < than fire ? but fire does damage so fire cancels fear ?
                //rule is as long as you are under effect deal with that then the other effect will wait and kick after or not at all

                Debug.Log(currentAIPriest.currentAiPriestEffects);

                switch (currentAIPriest.currentAiPriestEffects)
                {
                    case AiPriestEffects.OnFire:
                        currentAIPriest.OnFire();
                        goto Skip;

                    case AiPriestEffects.Feared:
                        currentAIPriest.Fear();
                        goto Skip;

                    case AiPriestEffects.Stun:

                        goto Skip;

                    case AiPriestEffects.Poisoned:
                        currentAIPriest.Poisoned();
                        goto Skip;
                }
            }
            else
            {
                if (currentAIPriest.FearAmount >= currentAIPriest.fearMaxAmount)
                {
                    currentAIPriest.AmUnderEffect = true;
                    currentAIPriest.currentAiPriestEffects = AiPriestEffects.Feared;
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

                            if (currentAIPriest != null)
                            {
                                if (currentAIPriest.Target != null)
                                {
                                    if (Vector3.Distance(currentAIPriest.transform.position, currentAIPriest.Target.transform.position) < 2)
                                    {
                                        currentAIPriest.CanLookAround = true;
                                    }
                                    else
                                    {
                                        currentAIPriest.Walk();
                                    }
                                }
                            }
                        }
                        else
                        {
                            currentAIPriest.Observe();
                        }
                    }
                    break;
            }

            Skip:
                continue;
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

                if (ChurchInMiddleOfTown != null)
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

            if (visualsToShowActivation != null)
            {
                visualsToShowActivation.SetActive(false);
            }

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
        Revenge = true;
    }
}
