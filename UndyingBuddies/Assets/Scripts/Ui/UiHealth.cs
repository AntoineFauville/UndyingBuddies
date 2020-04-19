using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiHealth : MonoBehaviour
{
    public float life;
    public float maxLife;
    public float MentalHealth;
    public float maxMentalHealth;
    public Text text;
    public Image image;
    public Image background;
    public Image imageMentalHealth;
    public Image backgroundMentalHealth;
    public Image physicalResistance;
    public Image mentalResistance;

    void Start()
    {
        physicalResistance.enabled = false;
        mentalResistance.enabled = false;

        StartCoroutine(UiHealthverySlowUpdate());
    }

    IEnumerator UiHealthverySlowUpdate()
    {
        if (life == maxLife)
        {
            image.enabled = false;
            background.enabled = false;
        }
        else
        {
            image.enabled = true;
            background.enabled = true;
        }

        if (MentalHealth == 0)
        {
            imageMentalHealth.enabled = false;
            backgroundMentalHealth.enabled = false;
        }
        else
        {
            imageMentalHealth.enabled = true;
            backgroundMentalHealth.enabled = true;
        }

        text.text = life.ToString() + "/" + maxLife.ToString();

        float fill = life / maxLife;

        image.fillAmount = fill;

        float fillMental = MentalHealth / maxMentalHealth;

        imageMentalHealth.fillAmount = fillMental;

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(UiHealthverySlowUpdate());
    }
}
