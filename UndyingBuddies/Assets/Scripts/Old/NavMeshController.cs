using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshController : MonoBehaviour
{
    public NavMeshSurface _navMeshSurface;

    void Start()
    {
        ReGenerateNavMesh();
    }

    public void ReGenerateNavMesh()
    {
        StartCoroutine(BuildNavMeshEnsurer());
    }

    IEnumerator BuildNavMeshEnsurer()
    {
        _navMeshSurface.BuildNavMesh();
        yield return new WaitForSeconds(1f);
        StartCoroutine(BuildNavMeshEnsurer());
    }
}
