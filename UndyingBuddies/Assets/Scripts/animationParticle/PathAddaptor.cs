using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathAddaptor : MonoBehaviour
{
    private GameObject _startingPoint;
    private GameObject _endingPoint;

    private ParticleFlowManager _particleFlowManager;

    private int _particleHeight;
    
    public void Setup(GameObject startingPoint, GameObject endingPoint, ParticleFlowManager particleFlowManager, int particleHeight)
    {
        _startingPoint = startingPoint;
        _endingPoint = endingPoint;
        _particleFlowManager = particleFlowManager;
        _particleHeight = particleHeight;

        StartCoroutine(AdjustPositionSlowly());
    }

    void AdjustPosition()
    {
        _particleFlowManager.Points[0].transform.position = _startingPoint.transform.position;
        _particleFlowManager.Points[1].transform.position = new Vector3(_startingPoint.transform.position.x, _startingPoint.transform.position.y + _particleHeight, _startingPoint.transform.position.z);

        _particleFlowManager.Points[2].transform.position = new Vector3(_endingPoint.transform.position.x, _endingPoint.transform.position.y + _particleHeight, _endingPoint.transform.position.z);
        _particleFlowManager.Points[3].transform.position = _endingPoint.transform.position;
    }

    IEnumerator AdjustPositionSlowly()
    {
        AdjustPosition();

        yield return new WaitForSeconds(1);

        StartCoroutine(AdjustPositionSlowly());
    }
}
