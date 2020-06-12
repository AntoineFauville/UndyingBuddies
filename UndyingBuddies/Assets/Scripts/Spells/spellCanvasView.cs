using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class spellCanvasView : MonoBehaviour
{
    public bool isShowing;

    public SpellArchetype spellArchetype;

    public SpellManager _spellManager;

    public Image backGroundImage;

    public Image LoadingTime;

    void Start()
    {
        _spellManager = GameObject.Find("Main Camera").GetComponent<SpellManager>();

        this.transform.GetChild(0).GetComponent<Text>().text = spellArchetype.spellName + "\n" + spellArchetype.SpellCostInEnergy + " Energy";

        _spellManager.spellCanvases.Add(this);

        backGroundImage.sprite = spellArchetype.backGroundImage;
    }
    
    public void ActivateBool()
    {
        for (int i = 0; i < _spellManager.spellCanvases.Count; i++)
        {
            _spellManager.spellCanvases[i].isShowing = false;
        }
        isShowing = true;

        GameObject.Find("Main Camera").GetComponent<PlaceSpell>().activateShowOfSpell = isShowing;
    }

    void Update()
    {
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
