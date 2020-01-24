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

    void Update()
    {
        text.text = life.ToString() + "/" + maxLife.ToString();

        float fill = life / maxLife;

        image.fillAmount = fill;
    }
}
