using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rats : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;

    [SerializeField] private GameObject Target;

    int distance = 2;

    void Update()
    {
        Vector3 pos = Target.transform.position;
        _navMeshAgent.destination = new Vector3(Random.Range(pos.x- distance, pos.x+ distance), 0, Random.Range(pos.z - distance, pos.z + distance));
    }
}
