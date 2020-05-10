using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class RatSpell : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;

    public GameObject Target;

    int distance = 10;

    public GameObject HorrorParts;
    public GameObject RatExplosionSpawner;
    public GameObject PoisonRat;

    void Update()
    {
        if (Target != null)
        {
            Vector3 pos = Target.transform.position;
            _navMeshAgent.destination = new Vector3(Random.Range(pos.x - distance, pos.x + distance), 0, Random.Range(pos.z - distance, pos.z + distance));

            Debug.DrawLine(this.transform.position, Target.transform.position, Color.white);
        }
    }
}
