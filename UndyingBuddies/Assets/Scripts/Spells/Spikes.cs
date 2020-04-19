using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private GameSettings _gameSettings;

    [SerializeField] private List<GameObject> allPriestTouched = new List<GameObject>();

    void Start()
    {
        //we can place this here because we just need to know on placement what we hit
        Collider[] HitCollider = Physics.OverlapSphere(this.transform.position, _gameSettings.spikeSpell.Range);
        
        for (int i = 0; i < HitCollider.Length; i++)
        {
            if (HitCollider[i].GetComponent<AIPriest>() != null && !HitCollider[i].GetComponent<AIPriest>().AmIBuilding)
            {
                allPriestTouched.Add(HitCollider[i].gameObject);
            }
        }

        for (int i = 0; i < allPriestTouched.Count; i++)
        {
            allPriestTouched[i].gameObject.GetComponent<AIStatController>().TakeDamage(AiStatus.Physical, _gameSettings.spikeSpell);
            allPriestTouched[i].gameObject.GetComponent<AIPriest>().Stun(1);
        }
    }
}
