using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticleAfterSec : MonoBehaviour
{
    [SerializeField] private float seconds;
    [SerializeField] private GameObject trigger;
    public bool moduleEnabled;

    [SerializeField] private ParticleSystem[] particleSystems;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(waittoDie());
    }

    IEnumerator waittoDie()
    {
        yield return new WaitForSeconds(seconds);

        //for (int i = 0; i < particleSystems.Length; i++)
        //{
        //    var emission = particleSystems[i].emission;
        //    moduleEnabled = false;
        //    emission.enabled = moduleEnabled;
        //}
        trigger.SetActive(false);

        yield return new WaitForSeconds(2);
        DestroyImmediate(this.gameObject);
    }
}
