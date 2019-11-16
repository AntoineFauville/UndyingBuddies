using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class House : MonoBehaviour
{
    [SerializeField] private SettingsData _settingsData;

    [SerializeField] private int currentWood;

    [SerializeField] private Text textHealthBar;
    [SerializeField] private Image lifeBar;

    public bool houseFinished;
    public bool sendcheck;

    public GameObject VisualState0;
    public GameObject VisualState1;
    public GameObject VisualState2;
    public GameObject VisualState3;

    // Start is called before the first frame update
    void Start()
    {
        currentWood = 0;
    }

    void Update()
    {
        if (currentWood >= _settingsData.woodNeedToFinishHouse)
        {
            currentWood = _settingsData.woodNeedToFinishHouse;

            if (GameObject.Find("GameController").GetComponent<Usables>().House.Contains(this.gameObject))
            {
                GameObject.Find("GameController").GetComponent<Usables>().House.Remove(this.gameObject);
            }
        }

        UpdateVisuals();

        if (currentWood == _settingsData.woodNeedToFinishHouse)
        {
            houseFinished = true;
            if (houseFinished && !sendcheck)
            {
                GameObject.Find("GameController").GetComponent<GameManager>().amountOfFinishedHouse++;
                sendcheck = true;
            }
        }
    }

    void UpdateVisuals()
    {
        textHealthBar.text = currentWood.ToString() + " / " + _settingsData.woodNeedToFinishHouse.ToString();
        if (currentWood == 0)
        {
            lifeBar.fillAmount = 0;
        }
        else
        {
            lifeBar.fillAmount = (float)currentWood / (float)_settingsData.woodNeedToFinishHouse;
        }

        if (currentWood == 0)
        {
            VisualState0.SetActive(true);
            VisualState1.SetActive(false);
            VisualState2.SetActive(false);
            VisualState3.SetActive(false);
        }

        //update visuals along the way
        if (currentWood >= _settingsData.woodForFirstStateOfHouse) // 10
        {
            VisualState0.SetActive(false);
            VisualState1.SetActive(true);
            VisualState2.SetActive(false);
            VisualState3.SetActive(false);
        }

        if (currentWood >= _settingsData.woodForSecondStateOfHouse) // 20
        {
            VisualState0.SetActive(false);
            VisualState1.SetActive(false);
            VisualState2.SetActive(true);
            VisualState3.SetActive(false);
        }

        if (currentWood >= _settingsData.woodForThirdStateOfHouse) // 30
        {
            VisualState0.SetActive(false);
            VisualState1.SetActive(false);
            VisualState2.SetActive(false);
            VisualState3.SetActive(true);
        }
    }

    public void AddWoodToBuilding(int woodAmount)
    {
        currentWood += woodAmount;

        if (currentWood == _settingsData.woodNeedToFinishHouse)
        {
            //loose game
        }
    }
}
