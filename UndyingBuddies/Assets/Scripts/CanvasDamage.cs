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

    public void SetupCanvasDamage(AiStatus aiStatus, int DamageAmount, int resistance)
    {
        Sanity.SetActive(false);
        Physical.SetActive(false);

        resistanceText.text = "";
        text.text = "";

        if (aiStatus == AiStatus.MentalHealth)
        {
            Sanity.SetActive(true);
            text.text = "+" + DamageAmount;
        }
        else if(aiStatus == AiStatus.Physical)
        {
            Physical.SetActive(true);
            text.text = "-" + DamageAmount;
        }

        if (resistance > 0)
        {
            resistanceText.text = "Res " + resistance;
        }
    }
}
