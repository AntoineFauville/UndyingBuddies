using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFactory : MonoBehaviour
{
    private GameObject _particle;

    private Transform _startingPoint;

    public int intervals;

    public bool activate;

    private ParticleFlowManager _particleFlowManager;

    public void Setup(GameObject particle, Transform startingPoint, ParticleFlowManager particleFlowManager)
    {
        _particle = particle;
        _startingPoint = startingPoint;
        _particleFlowManager = particleFlowManager;

        StartCoroutine(spawnParticleSlowly());
    }

    public void CreateParticle()
    {
        GameObject Particle = Instantiate(_particle, _startingPoint.position, new Quaternion());
        Particle.GetComponent<ParticleMover>().travelPoints = _particleFlowManager.Points;
    }

    IEnumerator spawnParticleSlowly()
    {
        if (activate)
        {
            CreateParticle();
        }

        yield return new WaitForSeconds(intervals);

        StartCoroutine(spawnParticleSlowly());
    }
}
