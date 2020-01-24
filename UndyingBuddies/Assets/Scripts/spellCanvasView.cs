using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spellCanvasView : MonoBehaviour
{
    public bool isShowing;

    public SpellArchetype spellArchetype;
    
    public void ActivateBool()
    {
        isShowing = !isShowing;

        GameObject.Find("Main Camera").GetComponent<PlaceSpell>().activateShowOfSpell = isShowing;

        if (isShowing)
        {
            GameObject.Find("Main Camera").GetComponent<PlaceSpell>().SetSpellToActive(spellArchetype);
        }
    }
}
