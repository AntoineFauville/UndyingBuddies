using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackFromPriestCamp : MonoBehaviour
{
    public float seconds;
    [SerializeField] private Image imageToFill;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(timerUpdate());
    }

    IEnumerator timerUpdate()
    {
        for (float i = 0; i < seconds; i++)
        {
            yield return new WaitForSeconds(1);
            imageToFill.fillAmount = i/seconds;
        }
    }
}
