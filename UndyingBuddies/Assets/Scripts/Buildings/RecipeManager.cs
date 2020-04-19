using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeManager : MonoBehaviour
{
    public ResourceType SoulInput_01;
    public ResourceType SoulInput_02;

    [SerializeField] private Image _imageToProcess_01;
    [SerializeField] private Text _TextToProcess_01;

    [SerializeField] private GameObject[] _recipeNotUnlockedInInput01;

    [SerializeField] private Image _imageToProcess_02;
    [SerializeField] private Text _TextToProcess_02;
    [SerializeField] private Image _imageProcessed;
    [SerializeField] private Text _TextProcessed;

    [SerializeField] private Text _EmergencyText;

    [SerializeField] private SwitchBuildingType _switchBuildingType;
    [SerializeField] private GameSettings _gameSettings;
    [SerializeField] private AiManager _aiManager;
    [SerializeField] private ResourceManager _resourceManager;

    [SerializeField] private GameObject Input02UI;

    public bool BoughtSecondInput;

    [SerializeField] private ResourceType _resourceTypeProduced;
    
    void Start()
    {
        SoulInput_01 = ResourceType.brokenSoul;
        SoulInput_02 = ResourceType.noResource;
        _resourceTypeProduced = ResourceType.whiteSoul;

        Input02UI.SetActive(false);

        for (int i = 0; i < _recipeNotUnlockedInInput01.Length; i++)
        {
            _recipeNotUnlockedInInput01[i].SetActive(false);
        }

        _EmergencyText.text = "You don't seem to have the correct setup for this";
        _EmergencyText.enabled = false;
        
        //default settings
        checkIfRecipeChange();
    }

    public void LoadInfoFromBuilding()
    {
        if (_switchBuildingType.building != null)
        {
            _resourceTypeProduced = _switchBuildingType.building.resourceProducedAtBuilding;
            switch (_resourceTypeProduced)
            {
                case ResourceType.whiteSoul:
                    SoulInput_01 = ResourceType.brokenSoul;
                    SoulInput_02 = ResourceType.noResource;
                    break;
                case ResourceType.blueVioletSoul:
                    SoulInput_01 = ResourceType.whiteSoul;
                    SoulInput_02 = ResourceType.noResource;
                    break;
                case ResourceType.violetSoul:
                    SoulInput_01 = ResourceType.whiteSoul;
                    SoulInput_02 = ResourceType.blueVioletSoul;
                    break;
                case ResourceType.blueSoul:
                    SoulInput_01 = ResourceType.whiteSoul;
                    SoulInput_02 = ResourceType.violetSoul;
                    break;
                case ResourceType.redSoul:
                    SoulInput_01 = ResourceType.blueVioletSoul;
                    SoulInput_02 = ResourceType.violetSoul;
                    break;
            }
        }
    }

    public void checkIfRecipeChange()
    {
        if (SoulInput_01 == ResourceType.brokenSoul)
        {
            _TextToProcess_01.text = ResourceType.brokenSoul.ToString() + "\n" + _gameSettings.BrokenSoulValueInEnergy + " Energy";
            _imageToProcess_01.color = _gameSettings.brokenSoulColor;
        }
        else if (SoulInput_01 == ResourceType.whiteSoul)
        {
            _TextToProcess_01.text = ResourceType.whiteSoul.ToString() + "\n" + _gameSettings.WhiteSoulValueInEnergy + " Energy";
            _imageToProcess_01.color = _gameSettings.whiteSoulColor;
        }
        else if (SoulInput_01 == ResourceType.blueVioletSoul)
        {
            _TextToProcess_01.text = ResourceType.blueVioletSoul.ToString() + "\n" + _gameSettings.BlueVioletSoulValueInEnergy + " Energy";
            _imageToProcess_01.color = _gameSettings.blueVioletColor;
        }
        else if (SoulInput_01 == ResourceType.violetSoul)
        {
            _TextToProcess_01.text = ResourceType.violetSoul.ToString() + "\n" + _gameSettings.VioletSoulValueInEnergy + " Energy";
            _imageToProcess_01.color = _gameSettings.violetColor;
        }

        if (BoughtSecondInput)
        {
            if (SoulInput_02 == ResourceType.brokenSoul)
            {
                _TextToProcess_02.text = ResourceType.brokenSoul.ToString() + "\n" + _gameSettings.BrokenSoulValueInEnergy + " Energy";
                _imageToProcess_02.color = _gameSettings.brokenSoulColor;
            }
            else if (SoulInput_02 == ResourceType.whiteSoul)
            {
                _TextToProcess_02.text = ResourceType.whiteSoul.ToString() + "\n" + _gameSettings.WhiteSoulValueInEnergy + " Energy";
                _imageToProcess_02.color = _gameSettings.whiteSoulColor;
            }
            else if (SoulInput_02 == ResourceType.blueVioletSoul)
            {
                _TextToProcess_02.text = ResourceType.blueVioletSoul.ToString() + "\n" + _gameSettings.BlueVioletSoulValueInEnergy + " Energy";
                _imageToProcess_02.color = _gameSettings.blueVioletColor;
            }
            else if (SoulInput_02 == ResourceType.violetSoul)
            {
                _TextToProcess_02.text = ResourceType.violetSoul.ToString() + "\n" + _gameSettings.VioletSoulValueInEnergy + " Energy";
                _imageToProcess_02.color = _gameSettings.violetColor;
            }
            else if (SoulInput_02 == ResourceType.noResource)
            {
                _TextToProcess_02.text = "";
                _imageToProcess_02.color = _gameSettings.defaultWhenNotWorking;
            }
        }

        //check recipes
        if (_switchBuildingType.building != null)
        {
            _EmergencyText.enabled = false;

            if (SoulInput_01 == ResourceType.brokenSoul && SoulInput_02 == ResourceType.noResource)
            {
                _resourceTypeProduced = ResourceType.whiteSoul;
                _imageProcessed.color = _gameSettings.whiteSoulColor;
                _TextProcessed.text = ResourceType.whiteSoul.ToString() + "\n" + _gameSettings.WhiteSoulValueInEnergy + " Energy";

                if (_aiManager.ResourceToProcess.Count <= 0)
                {
                    _EmergencyText.enabled = true;
                    _EmergencyText.text = "No more resource to transform to get " + ResourceType.brokenSoul;
                }
            }
            else if (SoulInput_01 == ResourceType.whiteSoul && SoulInput_02 == ResourceType.noResource)
            {
                _resourceTypeProduced = ResourceType.blueVioletSoul;
                _imageProcessed.color = _gameSettings.blueVioletColor;
                _TextProcessed.text = ResourceType.blueVioletSoul.ToString() + "\n" + _gameSettings.BlueVioletSoulValueInEnergy + " Energy";

                List<GameObject> _whiteSoulStorage = new List<GameObject>();

                for (int i = 0; i < _aiManager.WhiteSoulStockage.Count; i++)
                {
                    if (_aiManager.WhiteSoulStockage[i] != _switchBuildingType.building.gameObject)
                    {
                        _whiteSoulStorage.Add(_aiManager.WhiteSoulStockage[i]);
                    }
                }

                if (_whiteSoulStorage.Count <= 0) // since i put the apply, i can't check if i'm not included, so i'll do -1 directly to exclude me right away anyway
                {
                    _EmergencyText.enabled = true;
                    _EmergencyText.text = "You don't seem to have the correct setup for this";
                }
            }
            else if (SoulInput_01 == ResourceType.whiteSoul && SoulInput_02 == ResourceType.blueVioletSoul)
            {
                _resourceTypeProduced = ResourceType.violetSoul;
                _imageProcessed.color = _gameSettings.violetColor;
                _TextProcessed.text = ResourceType.violetSoul.ToString() + "\n" + _gameSettings.VioletSoulValueInEnergy + " Energy";

                List<GameObject> _whiteSoulStorage = new List<GameObject>();

                for (int i = 0; i < _aiManager.WhiteSoulStockage.Count; i++)
                {
                    if (_aiManager.WhiteSoulStockage[i] != _switchBuildingType.building.gameObject)
                    {
                        _whiteSoulStorage.Add(_aiManager.WhiteSoulStockage[i]);
                    }
                }

                List<GameObject> _blueVioletStockage = new List<GameObject>();

                for (int i = 0; i < _aiManager.BlueVioletSoulStockage.Count; i++)
                {
                    if (_aiManager.BlueVioletSoulStockage[i] != _switchBuildingType.building.gameObject)
                    {
                        _blueVioletStockage.Add(_aiManager.BlueVioletSoulStockage[i]);
                    }
                }

                if (_whiteSoulStorage.Count <= 0 || _blueVioletStockage.Count <= 0)// since i put the apply, i can't check if i'm not included, so i'll do -1 directly to exclude me right away anyway
                {
                    _EmergencyText.enabled = true;
                    _EmergencyText.text = "You don't seem to have the correct setup for this";
                }
            }
            else if (SoulInput_01 == ResourceType.whiteSoul && SoulInput_02 == ResourceType.violetSoul)
            {
                _resourceTypeProduced = ResourceType.blueSoul;
                _imageProcessed.color = _gameSettings.blueColor;
                _TextProcessed.text = ResourceType.blueSoul.ToString() + "\n" + _gameSettings.BlueSoulValueInEnergy + " Energy";

                List<GameObject> _whiteSoulStorage = new List<GameObject>();

                for (int i = 0; i < _aiManager.WhiteSoulStockage.Count; i++)
                {
                    if (_aiManager.WhiteSoulStockage[i] != _switchBuildingType.building.gameObject)
                    {
                        _whiteSoulStorage.Add(_aiManager.WhiteSoulStockage[i]);
                    }
                }

                List<GameObject> _VioletStockage = new List<GameObject>();

                for (int i = 0; i < _aiManager.VioletSoulStorage.Count; i++)
                {
                    if (_aiManager.VioletSoulStorage[i] != _switchBuildingType.building.gameObject)
                    {
                        _VioletStockage.Add(_aiManager.VioletSoulStorage[i]);
                    }
                }

                if (_whiteSoulStorage.Count <= 0 || _VioletStockage.Count <= 0)// since i put the apply, i can't check if i'm not included, so i'll do -1 directly to exclude me right away anyway
                {
                    _EmergencyText.enabled = true;
                    _EmergencyText.text = "You don't seem to have the correct setup for this";
                }
            }
            else if (SoulInput_01 == ResourceType.blueVioletSoul && SoulInput_02 == ResourceType.violetSoul)
            {
                _resourceTypeProduced = ResourceType.redSoul;
                _imageProcessed.color = _gameSettings.redColor;
                _TextProcessed.text = ResourceType.redSoul.ToString() + "\n" + _gameSettings.RedSoulValueInEnergy + " Energy";

                List<GameObject> _blueVioletStockage = new List<GameObject>();

                for (int i = 0; i < _aiManager.BlueVioletSoulStockage.Count; i++)
                {
                    if (_aiManager.BlueVioletSoulStockage[i] != _switchBuildingType.building.gameObject)
                    {
                        _blueVioletStockage.Add(_aiManager.BlueVioletSoulStockage[i]);
                    }
                }

                List<GameObject> _VioletStockage = new List<GameObject>();

                for (int i = 0; i < _aiManager.VioletSoulStorage.Count; i++)
                {
                    if (_aiManager.VioletSoulStorage[i] != _switchBuildingType.building.gameObject)
                    {
                        _VioletStockage.Add(_aiManager.VioletSoulStorage[i]);
                    }
                }

                if (_blueVioletStockage.Count <= 0 || _VioletStockage.Count <= 0)
                {
                    _EmergencyText.enabled = true;
                    _EmergencyText.text = "You don't seem to have the correct setup for this";
                }
            }
            else
            {
                _imageProcessed.color = _gameSettings.defaultWhenNotWorking;
                _TextProcessed.text = "???";
                _EmergencyText.enabled = true;
                _EmergencyText.text = "Couldn't find a matching soul to process";
            }
        }
    }

    public void SwitchSoulInput01(int soul)
    {
        SoulInput_01 = (ResourceType)soul;
        checkIfRecipeChange();
    }

    public void SwitchSoulInput02(int soul)
    {
        SoulInput_02 = (ResourceType)soul;
        checkIfRecipeChange();
    }

    public void UnlockSecondInput(GameObject purschaseButton)
    {
        if (_resourceManager.amountOfEnergy >= _gameSettings.CostToUnlockTwoInputInEnergy)
        {
            _resourceManager.amountOfEnergy -= _gameSettings.CostToUnlockTwoInputInEnergy;

            purschaseButton.SetActive(false);
            BoughtSecondInput = true;

            for (int i = 0; i < _recipeNotUnlockedInInput01.Length; i++)
            {
                _recipeNotUnlockedInInput01[i].SetActive(true);
            }

            Input02UI.SetActive(true);
            checkIfRecipeChange();
        }
        else
        {
            Debug.Log("Not enought energy to unlock second input");
        }
    }

    public void Apply()
    {
        _switchBuildingType.SwitchResourceType(_resourceTypeProduced);
        _switchBuildingType.building.UpdateVisuParticleWell(_resourceTypeProduced);
    }
}
