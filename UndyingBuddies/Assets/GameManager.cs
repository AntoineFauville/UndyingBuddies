using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BoyFactory _boyFactory;

    

    [SerializeField] private GameObject LoosePanel;
    [SerializeField] private GameObject WinPanel;

    [SerializeField] private Text TextBoyDisplay;

    [SerializeField] private SettingsData _settingsData;

    [SerializeField] private House _house;
    public int amountOfFinishedHouse;

    void Start()
    {
        LoosePanel.SetActive(false);
        WinPanel.SetActive(false);

        TextBoyDisplay.text = _boyFactory.Boys.Count + " / " + _settingsData.BoyAmountToLoose;
    }

    void Update()
    {
        TextBoyDisplay.text = _boyFactory.Boys.Count + " / " + _settingsData.BoyAmountToLoose;

        //CheckWinOrLooseWithAmountOfBoys();
        CheckWinOrLooseWithHouseBuildinng();
    }

    void CheckWinOrLooseWithAmountOfBoys()
    {
        if (_boyFactory.Boys.Count <= 0)
        {
            WinPanel.SetActive(true);
        }

        if (_boyFactory.Boys.Count >= _settingsData.BoyAmountToLoose)
        {
            LoosePanel.SetActive(true);
        }
    }

    void CheckWinOrLooseWithHouseBuildinng()
    {
        if (amountOfFinishedHouse == 3)
        {
            LoosePanel.SetActive(true);
        }
    }
}
