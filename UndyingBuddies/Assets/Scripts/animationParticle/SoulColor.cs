using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulColor : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _particleSystem;
    public Color ColorOfParticles;
    
    void Start()
    {
        for (int i = 0; i < _particleSystem.Length; i++)
        {
            ParticleSystem.MainModule settings = _particleSystem[i].GetComponent<ParticleSystem>().main;
            settings.startColor = ColorOfParticles;
        }
    }
    
    public void ChangeColor(Color color)
    {
        ColorOfParticles = color;

        for (int i = 0; i < _particleSystem.Length; i++)
        {
            ParticleSystem.MainModule settings = _particleSystem[i].GetComponent<ParticleSystem>().main;
            settings.startColor = ColorOfParticles;
        }
    }
}
