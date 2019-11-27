using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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

    public int duration = 5;
    public int timeRemaining;
    private int timeRemainingAtStart;
    public bool isCountingDown = false;
    [SerializeField] private Text countdownText;

    [SerializeField] private int houseNeededToWinTheGame;
    public int BoyAmount;

    private SceneManagerDontDestroy SceneManagerDontDestroy;

    [SerializeField] private Image[] _pentacle;
    [SerializeField] private Image _pentacleUI;

    [SerializeField] List<GameObject> particleSystemDeadAnim = new List<GameObject>();
    [SerializeField] List<GameObject> AnimDemonFinAnim = new List<GameObject>();

    void Start()
    {
        SceneManagerDontDestroy = GameObject.Find("SceneManager").GetComponent<SceneManagerDontDestroy>();

        LoosePanel.SetActive(false);
        WinPanel.SetActive(false);

        Begin();

        countdownText.text = timeRemaining.ToString();

        TextBoyDisplay.text = _boyFactory.TotalOfTheBoys.Count.ToString();

        for (int i = 0; i < _pentacle.Length; i++)
        {
            _pentacle[i].fillAmount = 0;
        }
        _pentacleUI.fillAmount = 0;

        foreach (var gameobject in GameObject.FindGameObjectsWithTag("VillageDeadAnimation"))
        {
            particleSystemDeadAnim.Add(gameobject);
        }

        foreach (var gameobject in GameObject.FindGameObjectsWithTag("VillageDead"))
        {
            AnimDemonFinAnim.Add(gameobject);
        }
    }

    void Update()
    {
        TextBoyDisplay.text = _boyFactory.TotalOfTheBoys.Count.ToString();

        countdownText.text = "Time to win : " + timeRemaining.ToString();

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
        if (amountOfFinishedHouse == houseNeededToWinTheGame)
        {
            LoosePanel.SetActive(true);
            StartCoroutine(waitToLaunchAgain());
        }

        if (_boyFactory.TotalOfTheBoys.Count <= 0)
        {
            WinPanel.SetActive(true);
            StartCoroutine(waitToLaunchAgain());

            LaunchWinAnimation();
        }
    }

    void LaunchWinAnimation()
    {
        for (int i = 0; i < particleSystemDeadAnim.Count; i++)
        {
            particleSystemDeadAnim[i].GetComponent<ParticleSystem>().Play();
        }

        for (int i = 0; i < AnimDemonFinAnim.Count; i++)
        {
            AnimDemonFinAnim[i].GetComponent<Animator>().Play("InvokeChtulhu");
        }

        for (int i = 0; i < _boyFactory.TotalOfTheBoys.Count; i++)
        {
            _boyFactory.TotalOfTheBoys[i].GetComponent<NavMeshAgent>().enabled = false;
            _boyFactory.TotalOfTheBoys[i].GetComponent<Survive>().food = 0;
            _boyFactory.TotalOfTheBoys[i].GetComponent<Survive>().WoodAmount = 0;
            _boyFactory.TotalOfTheBoys[i].GetComponent<Survive>().dieded = true;
        }
    }

    public void Begin()
    {
        if (!isCountingDown)
        {
            isCountingDown = true;
            timeRemaining = duration;

            timeRemainingAtStart = timeRemaining;

            Invoke("_tick", 1f);
        }
    }

    private void _tick()
    {
        timeRemaining--;
        if (timeRemaining > 0)
        {
            for (int i = 0; i < _pentacle.Length; i++)
            {
                _pentacle[i].fillAmount = 1 - (float)((float)timeRemaining / (float)timeRemainingAtStart);
            }
            _pentacleUI.fillAmount = 1 - (float)((float)timeRemaining / (float)timeRemainingAtStart);

            Invoke("_tick", 1f);
        }
        else
        {
            for (int i = 0; i < _pentacle.Length; i++)
            {
                _pentacle[i].fillAmount = 1;
            }

            isCountingDown = false;
        }
    }

    void CheckWinWithTimer()
    {
        if (isCountingDown == false)
        {
            WinPanel.SetActive(true);
            StartCoroutine(waitToLaunchAgain());

            LaunchWinAnimation();
        }
    }

    IEnumerator waitToLaunchAgain()
    {
        yield return new WaitForSeconds(4f);
        SceneManagerDontDestroy.LoadScene();
    }
}
