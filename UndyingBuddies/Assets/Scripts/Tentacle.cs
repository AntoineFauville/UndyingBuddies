using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
    [SerializeField] private GameSettings _gameSettings;

    [SerializeField] private List<GameObject> allPriestTouched = new List<GameObject>();
    
    void Start()
    {
        //we can place this here because we just need to know on placement what we hit
        Collider[] HitCollider = Physics.OverlapSphere(this.transform.position, _gameSettings.tentacleSpell.Range);

        for (int i = 0; i < HitCollider.Length; i++)
        {
            if (HitCollider[i].GetComponent<AIPriest>() != null && !HitCollider[i].GetComponent<AIPriest>().AmIBuilding)
            {
                allPriestTouched.Add(HitCollider[i].gameObject);
            }
        }
        StartCoroutine(DamagePerXSeconds());
    }

    IEnumerator DamagePerXSeconds()
    {
        Debug.Log("doing damage tentacle");

        Collider[] HitCollider = Physics.OverlapSphere(this.transform.position, _gameSettings.tentacleSpell.Range);

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
            allPriestTouched[i].gameObject.GetComponent<AIStatController>().TakeDamage(AiStatus.MentalHealth, _gameSettings.tentacleSpell);
        }

        yield return new WaitForSeconds(0.4f);
        StartCoroutine(DamagePerXSeconds());
    }
}
