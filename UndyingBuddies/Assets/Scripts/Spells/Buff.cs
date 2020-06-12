using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buff : MonoBehaviour
{
    BuffType thisBuffType;

    public int LifeCounter;

    public int BuffDamage;

    [SerializeField] private Text text;

    public void Setup(BuffType buffType)
    {
        this.transform.SetParent(GameObject.Find("BuffContainer").transform);
        this.transform.localScale = new Vector3(1, 1, 1);
        this.transform.rotation = new Quaternion();
        this.transform.position = new Vector3(0, 0, 0); 
        this.transform.localPosition = new Vector3(0, 0, 0);

        thisBuffType = buffType;

        switch (thisBuffType)
        {
            case BuffType.PoisonBuff:
                break;
            case BuffType.PhysicalBuff:
                break;
            case BuffType.SanityBuff:
                break;
        }

        LifeCounter = 60;

        StartCoroutine(waitToDie());
    }

    void UpdateLifeCounter()
    {
        text.text = LifeCounter.ToString();
    }

    IEnumerator waitToDie()
    {
        LifeCounter--;

        UpdateLifeCounter();

        yield return new WaitForSeconds(1);

        StartCoroutine(waitToDie());
    }
}
