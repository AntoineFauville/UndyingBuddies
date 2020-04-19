using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flames : MonoBehaviour
{
    [SerializeField] private GameSettings _gameSettings;

    [SerializeField] private List<GameObject> allPriestTouched = new List<GameObject>();
    [SerializeField] private List<GameObject> allPriestThatWillBeOnFire = new List<GameObject>();

    void Start()
    {
        //we can place this here because we just need to know on placement what we hit
        Collider[] HitCollider = Physics.OverlapSphere(this.transform.position, _gameSettings.fireSpell.Range);

        for (int i = 0; i < HitCollider.Length; i++)
        {
            if (HitCollider[i].GetComponent<AIPriest>() != null && !HitCollider[i].GetComponent<AIPriest>().AmIBuilding)
            {
                allPriestTouched.Add(HitCollider[i].gameObject);

                int rand = Random.Range(0, 100);
                if (rand > 50)
                {
                    allPriestThatWillBeOnFire.Add(HitCollider[i].gameObject);
                }
            }
        }

        //this means you can only put on fire the guys from the beginning
        for (int i = 0; i < allPriestThatWillBeOnFire.Count; i++)
        {
            if (allPriestThatWillBeOnFire[i].GetComponent<AIPriest>().AmUnderEffect == false)
            {
                allPriestThatWillBeOnFire[i].GetComponent<AIPriest>().AmUnderEffect = true;
                allPriestThatWillBeOnFire[i].GetComponent<AIPriest>().currentAiPriestEffects = AiPriestEffects.OnFire;
            }
        }

        StartCoroutine(DamagePerXSeconds());
    }

    IEnumerator DamagePerXSeconds()
    {
        Debug.Log("doing damage");

        Collider[] HitCollider = Physics.OverlapSphere(this.transform.position, _gameSettings.fireSpell.Range);

        allPriestTouched.Clear();

        for (int i = 0; i < HitCollider.Length; i++)
        {
            if (HitCollider[i].GetComponent<AIPriest>() != null && !HitCollider[i].GetComponent<AIPriest>().AmIBuilding)
            {
                allPriestTouched.Add(HitCollider[i].gameObject);
            }
        }

        for (int i = 0; i < allPriestTouched.Count; i++)
        {
            allPriestTouched[i].gameObject.GetComponent<AIStatController>().TakeDamage(AiStatus.Physical, _gameSettings.fireSpell);
        }
        
        yield return new WaitForSeconds(0.4f);
        StartCoroutine(DamagePerXSeconds());
    }
}
