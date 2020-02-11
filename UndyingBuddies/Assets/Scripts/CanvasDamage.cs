using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasDamage : MonoBehaviour
{
    public Text text;
    public GameObject Sanity;
    public GameObject Physical;

    public void SetupCanvasDamage(AiStatus aiStatus, int DamageAmount)
    {
        Sanity.SetActive(false);
        Physical.SetActive(false);

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
    }
}
