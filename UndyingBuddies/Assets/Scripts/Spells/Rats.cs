using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rats : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;

    public GameObject Target;

    [SerializeField] private GameObject Rat;

    [SerializeField] private GameSettings _gameSettings;

    int distance = 2;

    void Update()
    {
        Vector3 pos = Target.transform.position;
        _navMeshAgent.destination = new Vector3(Random.Range(pos.x- distance, pos.x+ distance), 0, Random.Range(pos.z - distance, pos.z + distance));
    }

    void OnCollisionEnter(Collision coll)
    {
        Debug.Log(coll.gameObject.name);

        if (coll.gameObject.GetComponent<AIPriest>() != null && !coll.gameObject.GetComponent<AIPriest>().AmIBuilding)
        {
            if (coll.gameObject.GetComponent<AIPriest>().AmUnderEffect == false)
            {
                coll.gameObject.GetComponent<AIPriest>().AmUnderEffect = true;
                coll.gameObject.GetComponent<AIPriest>().currentAiPriestEffects = AiPriestEffects.Poisoned;
            }

            coll.gameObject.GetComponent<AIStatController>().TakeDamage(AiStatus.Physical, _gameSettings.ratsSpell);

            Destroy(Rat);
        }
    }
}
