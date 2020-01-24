using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellManager : MonoBehaviour
{
    public int unlockedFireSpell;

    public SpellArchetype spellArchetype;

    public PlaceSpell PlaceSpell;

    private ResourceManager resourceManager;

    public GameObject ButtonToPurchase;

    // Start is called before the first frame update
    void Start()
    {
        PlaceSpell = GameObject.Find("Main Camera").GetComponent<PlaceSpell>();
        resourceManager = GameObject.Find("Main Camera").GetComponent<ResourceManager>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (unlockedFireSpell)
        {
            case 0:
                //this is nothing
                break;

            case 1:
                if (!PlaceSpell.Spells.Contains(spellArchetype) && resourceManager.amountOfEnergy >= spellArchetype.CostToUnlockEnergy)
                {
                    PlaceSpell.SetupSpell(spellArchetype);
                    ButtonToPurchase.SetActive(false);
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
