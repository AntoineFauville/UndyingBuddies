using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class spellCanvasView : MonoBehaviour
{
    public bool isShowing;

    public SpellArchetype spellArchetype;

    void Start()
    {
        this.transform.GetChild(0).GetComponent<Text>().text = spellArchetype.spellName + " " + spellArchetype.SpellCostInEnergy + " Energy";
    }
    
    public void ActivateBool()
    {
        isShowing = !isShowing;

        GameObject.Find("Main Camera").GetComponent<PlaceSpell>().activateShowOfSpell = isShowing;

        if (isShowing)
        {
            GameObject.Find("Main Camera").GetComponent<PlaceSpell>().SetSpellToActive(spellArchetype);
        }

        if (isShowing)
        {
            this.GetComponent<Image>().color = Color.green;
            this.transform.GetChild(0).GetComponent<Text>().text = "Click to deactivate " + spellArchetype.spellName + " " + spellArchetype.SpellCostInEnergy + " Energy";
        }
        else
        {
            this.GetComponent<Image>().color = Color.white;
            this.transform.GetChild(0).GetComponent<Text>().text = spellArchetype.spellName + " " + spellArchetype.SpellCostInEnergy + " Energy";
        }
    }
}
