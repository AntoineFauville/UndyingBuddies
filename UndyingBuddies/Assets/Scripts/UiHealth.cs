using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiHealth : MonoBehaviour
{
    public float life;
    public float maxLife;
    public Text text;
    public Image image;
    public Image background;

    void Start()
    {
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

        text.text = life.ToString() + "/" + maxLife.ToString();

        float fill = life / maxLife;

        image.fillAmount = fill;

        yield return new WaitForSeconds(1.5f);

        StartCoroutine(UiHealthverySlowUpdate());
    }
}
