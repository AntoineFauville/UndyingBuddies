using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasDamage : MonoBehaviour
{
    public Text text;
    public Text resistanceText;
    public GameObject Sanity;
    public GameObject Physical;
    public GameObject Energy;

    public void SetupCanvasDamage(AiStatus aiStatus, int DamageAmount, int resistance)
    {
        Sanity.SetActive(false);
        Physical.SetActive(false);
        Energy.SetActive(false);

        resistanceText.text = "";
        text.text = "";

        if (aiStatus == AiStatus.MentalHealth)
        {
            Sanity.SetActive(true);
            text.text = "+" + DamageAmount;
        }
        else if (aiStatus == AiStatus.Physical)
        {
            Physical.SetActive(true);
            text.text = "-" + DamageAmount;
        }
        else if (aiStatus == AiStatus.Lonelyness)
        {
            Energy.SetActive(true);
            text.text = "-" + DamageAmount;
        }

        if (resistance > 0)
        {
            resistanceText.text = "Res " + resistance;
        }
    }

    public void SetupCanvasEnergy(int EnergyAmount)
    {
        Sanity.SetActive(false);
        Physical.SetActive(false);

        resistanceText.text = "";

        Energy.SetActive(true);

        text.text = "+" + EnergyAmount;
    }
}
