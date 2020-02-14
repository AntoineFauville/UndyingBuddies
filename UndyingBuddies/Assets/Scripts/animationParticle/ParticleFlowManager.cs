using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFlowManager : MonoBehaviour
{
    public List<GameObject> Points = new List<GameObject>();

    public GameObject StartingPoint;
    public GameObject EndingPoint;

    [SerializeField] private PathAddaptor pathAddaptor;
    [SerializeField] private ParticleFactory particleFactory;

    [SerializeField] private int intervals = 2;
    [SerializeField] private int particleHeight = 5;

    [SerializeField] private GameObject particle;

    public bool active;

    void Start()
    {
        particleFactory.intervals = intervals;

        particleFactory.Setup(particle, StartingPoint.transform, this);

        pathAddaptor.Setup(StartingPoint, EndingPoint, this, particleHeight);
    }

    void Update()
    {
        particleFactory.activate = active;
    }
}
