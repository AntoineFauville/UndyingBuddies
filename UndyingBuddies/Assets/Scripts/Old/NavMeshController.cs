using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshController : MonoBehaviour
{
    public NavMeshSurface _navMeshSurface;

    void Start()
    {
        _navMeshSurface.BuildNavMesh();

        //ReGenerateNavMesh();
    }

    //public void ReGenerateNavMesh()
    //{
    //    StartCoroutine(BuildNavMeshEnsurer());
    //}

    //IEnumerator BuildNavMeshEnsurer()
    //{
    //    _navMeshSurface.BuildNavMesh();
    //    yield return new WaitForSeconds(3f);
    //    StartCoroutine(BuildNavMeshEnsurer());
    //}
}
