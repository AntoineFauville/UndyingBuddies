using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BushLife : MonoBehaviour
{
    [SerializeField] private float health;

    [SerializeField] private GameObject flamesObject;

    private Usables usables;

    [SerializeField] private Text amoutLeftDisplay;

    void Update()
    {
        amoutLeftDisplay.text = health.ToString("F0");
    }

    public void Setup(Usables usable, float bushHealth)
    {
        usables = usable;

        health = bushHealth;

        flamesObject.SetActive(false);
    }

    public void ReduceLife(float amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (usables.Bush.Contains(this.gameObject))
        {
            usables.Bush.Remove(this.gameObject);
        }

        StartCoroutine(waitaSecBeforeDestroy());
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "FireZone")
        {
            print("bush in fire zone");
            ReduceLife(50);
            flamesObject.SetActive(true);
            StartCoroutine(waitToUnSetFire());
        }
    }

    IEnumerator waitaSecBeforeDestroy()
    {
        yield return new WaitForSeconds(0.1f);
        DestroyImmediate(this.gameObject);
    }

    IEnumerator waitToUnSetFire()
    {
        yield return new WaitForSeconds(6f);
        flamesObject.SetActive(false);
    }
}
