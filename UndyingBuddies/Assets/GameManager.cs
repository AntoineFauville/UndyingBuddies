using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BoyFactory _boyFactory;

    [SerializeField] private int BoyAmountToLoose;

    [SerializeField] private GameObject LoosePanel;
    [SerializeField] private GameObject WinPanel;

    [SerializeField] private Text TextBoyDisplay;

    void Start()
    {
        LoosePanel.SetActive(false);
        WinPanel.SetActive(false);

        TextBoyDisplay.text = _boyFactory.Boys.Count + " / " + BoyAmountToLoose;
    }

    void Update()
    {
        TextBoyDisplay.text = _boyFactory.Boys.Count + " / " + BoyAmountToLoose;

        if (_boyFactory.Boys.Count <= 0)
        {
            WinPanel.SetActive(true);
        }

        if (_boyFactory.Boys.Count >= BoyAmountToLoose)
        {
            LoosePanel.SetActive(true);
        }
    }
}
