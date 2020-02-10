using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private Text title;
    [SerializeField] private Text tutorialContent;

    [SerializeField] private Text title2;
    [SerializeField] private Text tutorialContent2;

    [SerializeField] private Text title3;
    [SerializeField] private Text tutorialContent3;

    [SerializeField] private Text title4;
    [SerializeField] private Text tutorialContent4;

    [SerializeField] private Text title5;
    [SerializeField] private Text tutorialContent5;

    [SerializeField] private Text title6;
    [SerializeField] private Text tutorialContent6;

    void Start()
    {
        //part 1 //setup
        title.text = "You just got summoned!";
        tutorialContent.text = "Two scouts demons have spotted villages of priest near them, scrared, they created a small autel to summon you." + "\n" + "\n" +
            "Your goal is to destroy the priests inside those villages, scouts can't fight, you'll have to take care of that yourself!";

        //part 2 // key to success and how to get the goal
        title2.text = "Energy is key";
        tutorialContent2.text = "To kill the priest use spells, they can be found in the spell panel, you will need energy to unlock and use spells" + "\n" + "\n" +
            "This energy can be obtained by sacrificing resources";

        //part 3 //core mechanic
        title3.text = "Sacrifice";
        tutorialContent3.text = "To sacrifice left clic on an object you want to sacrifice, if it can be sacrificed, it'll turn into resources and energy" + "\n" + "\n" +
            "At first sacrifice tree and bushes to gain energy, this is not a very efficient way to get energy, you should process the resource to get more out of these basic objects";

        //part 3 //processing
        title4.text = "Processing";
        tutorialContent4.text = "First Create a processing building, once placed, it's instantly built and if you have an available demon, he'll start to work" + "\n" + "\n" +
            "He'll transform the trees, into planks and bushes into food baskets, these refined goods once in the stockpile, can be sacrificed for a good chunk of energy";

        //part 5 //attacking
        title5.text = "Attacking";
        tutorialContent5.text = "Be ready when you attack a city, all the priest will run to your village" + "\n" + "\n" +
            "Kill all of them before they reach you, if your cityhall gets destroyed, you loose";

        //part 6 //tips attacking
        title6.text = "Tips on Attacking";
        tutorialContent6.text = "In order to best choose your spells, inspect the cities, these have resistance to either physical damage or sanity damage, it's different every game" + "\n" + "\n" +
            "Since the spell unlock cost increases, choose carefully wich spells to purchase to addapt the best to the situation";

    }

}
