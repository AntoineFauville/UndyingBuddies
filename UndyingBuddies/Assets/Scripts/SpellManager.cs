﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellManager : MonoBehaviour
{
    public int unlockedFireSpell;

    public SpellArchetype firePatchSpell;
    public SpellArchetype spikesSpell;
    public SpellArchetype chtulhuEyeSpell;
    public SpellArchetype TentacleSpell;

    public PlaceSpell PlaceSpell;

    private ResourceManager resourceManager;

    public GameObject fireButton;
    public GameObject spikeButton;
    public GameObject eyeButton;
    public GameObject tentacleButton;

    public int indexOfCostSpells = 0;

    [SerializeField] private GameSettings _gameSettings;
    [SerializeField] private Text SpellCost;

    public List<spellCanvasView> spellCanvases = new List<spellCanvasView>();

    // Start is called before the first frame update
    void Start()
    {
        PlaceSpell = GameObject.Find("Main Camera").GetComponent<PlaceSpell>();
        resourceManager = GameObject.Find("Main Camera").GetComponent<ResourceManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (indexOfCostSpells < _gameSettings.CostSpell.Length)
        {
            SpellCost.text = _gameSettings.CostSpell[indexOfCostSpells].ToString();
        }

        switch (unlockedFireSpell)
        {
            case 0:
                //this is nothing
                break;

            case 1:
                if (!PlaceSpell.Spells.Contains(firePatchSpell) && resourceManager.amountOfEnergy >= _gameSettings.CostSpell[indexOfCostSpells])
                {
                    PlaceSpell.SetupSpell(firePatchSpell);
                    resourceManager.amountOfEnergy -= _gameSettings.CostSpell[indexOfCostSpells];
                    indexOfCostSpells++;
                    fireButton.SetActive(false);
                }
                else
                {
                    unlockedFireSpell = 0; //reset to not have the situation as follows:
                    //once you click on the button it sets  it to 1 so, as soon as you have enough money BOOM you get the spell, you mighjt have forgot since then that you
                    //even clicked on the spell here so it's not good.
                }
                break;

            case 2:
                if (!PlaceSpell.Spells.Contains(spikesSpell) && resourceManager.amountOfEnergy >= _gameSettings.CostSpell[indexOfCostSpells])
                {
                    PlaceSpell.SetupSpell(spikesSpell);
                    resourceManager.amountOfEnergy -= _gameSettings.CostSpell[indexOfCostSpells];
                    indexOfCostSpells++;
                    spikeButton.SetActive(false);
                }
                else
                {
                    unlockedFireSpell = 0; //reset to not have the situation as follows:
                    //once you click on the button it sets  it to 1 so, as soon as you have enough money BOOM you get the spell, you mighjt have forgot since then that you
                    //even clicked on the spell here so it's not good.
                }
                break;

            case 3:
                if (!PlaceSpell.Spells.Contains(chtulhuEyeSpell) && resourceManager.amountOfEnergy >= _gameSettings.CostSpell[indexOfCostSpells])
                {
                    PlaceSpell.SetupSpell(chtulhuEyeSpell);
                    resourceManager.amountOfEnergy -= _gameSettings.CostSpell[indexOfCostSpells];
                    indexOfCostSpells++;
                    eyeButton.SetActive(false);
                }
                else
                {
                    unlockedFireSpell = 0; //reset to not have the situation as follows:
                    //once you click on the button it sets  it to 1 so, as soon as you have enough money BOOM you get the spell, you mighjt have forgot since then that you
                    //even clicked on the spell here so it's not good.
                }
                break;

            case 4:
                if (!PlaceSpell.Spells.Contains(TentacleSpell) && resourceManager.amountOfEnergy >= _gameSettings.CostSpell[indexOfCostSpells])
                {
                    PlaceSpell.SetupSpell(TentacleSpell);
                    resourceManager.amountOfEnergy -= _gameSettings.CostSpell[indexOfCostSpells];
                    indexOfCostSpells++;
                    tentacleButton.SetActive(false);
                }
                else
                {
                    unlockedFireSpell = 0; //reset to not have the situation as follows:
                    //once you click on the button it sets  it to 1 so, as soon as you have enough money BOOM you get the spell, you mighjt have forgot since then that you
                    //even clicked on the spell here so it's not good.
                }
                break;
        }
    }

    public void UnlockFireSpell(int index)
    {
        unlockedFireSpell = index;
    }
}
