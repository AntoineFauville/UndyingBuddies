using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BoyFactory _boyFactory;
    
    [SerializeField] private GameObject LoosePanel;
    [SerializeField] private GameObject WinPanel;

    [SerializeField] private Text TextBoyDisplay;

    [SerializeField] private SettingsData _settingsData;

    [SerializeField] private House _house;
    public int amountOfFinishedHouse;

    public int duration = 5;
    public int timeRemaining;
    public bool isCountingDown = false;
    [SerializeField] private Text countdownText;

    void Start()
    {
        LoosePanel.SetActive(false);
        WinPanel.SetActive(false);

        Begin();

        countdownText.text = timeRemaining.ToString();

        TextBoyDisplay.text = _boyFactory.TotalOfTheBoys.Count + " / " + _settingsData.BoyAmountToLoose;
    }

    void Update()
    {
        TextBoyDisplay.text = _boyFactory.TotalOfTheBoys.Count + " / " + _settingsData.BoyAmountToLoose;

        countdownText.text = timeRemaining.ToString();

        //CheckWinOrLooseWithAmountOfBoys();
        CheckWinOrLooseWithHouseBuildinng();
        CheckWinWithTimer();
    }

    void CheckWinOrLooseWithAmountOfBoys()
    {
        if (_boyFactory.TotalOfTheBoys.Count <= 0)
        {
            WinPanel.SetActive(true);
            StartCoroutine(waitToLaunchAgain());
        }

        if (_boyFactory.TotalOfTheBoys.Count >= _settingsData.BoyAmountToLoose)
        {
            LoosePanel.SetActive(true);
            StartCoroutine(waitToLaunchAgain());
        }
    }

    void CheckWinOrLooseWithHouseBuildinng()
    {
        if (amountOfFinishedHouse == _settingsData.houseNeededToWin)
        {
            LoosePanel.SetActive(true);
            StartCoroutine(waitToLaunchAgain());
        }

        if (_boyFactory.TotalOfTheBoys.Count <= 0)
        {
            WinPanel.SetActive(true);
            StartCoroutine(waitToLaunchAgain());
        }
    }

    public void Begin()
    {
        if (!isCountingDown)
        {
            isCountingDown = true;
            timeRemaining = duration;
            Invoke("_tick", 1f);
        }
    }

    private void _tick()
    {
        timeRemaining--;
        if (timeRemaining > 0)
        {
            Invoke("_tick", 1f);
        }
        else
        {
            isCountingDown = false;
        }
    }

    void CheckWinWithTimer()
    {
        if (isCountingDown == false)
        {
            WinPanel.SetActive(true);
            StartCoroutine(waitToLaunchAgain());
        }
    }

    IEnumerator waitToLaunchAgain()
    {
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(0);
    }
}
