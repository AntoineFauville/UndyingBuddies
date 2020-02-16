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

    private bool waitToSpawn;

    private Color _color;

    public void Setup(GameObject particle, Transform startingPoint, ParticleFlowManager particleFlowManager, Color colorParticle)
    {
        _color = colorParticle;
        _particle = particle;
        _startingPoint = startingPoint;
        _particleFlowManager = particleFlowManager;

        if (!waitToSpawn)
        {
            StartCoroutine(spawnParticleSlowly());
        }
    }

    public void CreateParticle()
    {
        GameObject Particle = Instantiate(_particle, _startingPoint.position, new Quaternion());
        Particle.GetComponent<SoulColor>().ChangeColor(_color);
        Particle.GetComponent<ParticleMover>().travelPoints = _particleFlowManager.Points;
    }

    IEnumerator spawnParticleSlowly()
    {
        waitToSpawn = true;

        if (activate)
        {
            CreateParticle();
        }

        yield return new WaitForSeconds(intervals);

        waitToSpawn = false;

        //StartCoroutine(spawnParticleSlowly());
    }
}
