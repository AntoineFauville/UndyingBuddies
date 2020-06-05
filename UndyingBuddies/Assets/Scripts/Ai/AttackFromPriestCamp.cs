using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackFromPriestCamp : MonoBehaviour
{
    public float seconds;
    [SerializeField] private Image imageToFill;

    public Vector3 CameraPos;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(timerUpdate());
    }

    public void AlignToCamp()
    {
        GameObject.Find("Main Camera").transform.position = new Vector3 (CameraPos.x +6, 20, CameraPos.z - 17);
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
