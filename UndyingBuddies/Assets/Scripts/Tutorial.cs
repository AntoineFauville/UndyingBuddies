using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private Text title;
    [SerializeField] private Text tutorialContent;
    
    void Start()
    {
        title.text = "You just got summoned!";
        tutorialContent.text = "Two scout demons have spotted villages of priest near them, scrared, they created a small autel to summon you. " + "\n" +
            "Devellop your campment to refine resources around you, for this you need resource, grab a tree or a bush and sacrifice it to gain wood and food" + "\n" +
            "To grab and sacrifice, simple left click to grab and place, and right click to sacrfice once grabbed" + "\n" +
            "With these resource, create a factory and a stockpile to rafine your resources" + "\n" +
            "When you sacrifice the resource around you, you get some energy out of it, the refined resource give you more energy then a simple tree does" + "\n" +
            "Once you have enough energy, unlock some spells and if you feel ready to face the priest town, start using your spell on them!" + "\n" +
            "Watchout, the priest town might have some resistance and you might need to adjust your strategy" + "\n" +
            "Now click on shreck to continue";
    }

}
