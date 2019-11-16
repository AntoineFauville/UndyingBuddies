using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushLife : MonoBehaviour
{
    [SerializeField] private float health;

    private Usables usables;

    public void Setup(Usables usable, float bushHealth)
    {
        usables = usable;

        health = bushHealth;
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

    IEnumerator waitaSecBeforeDestroy()
    {
        yield return new WaitForSeconds(0.1f);
        DestroyImmediate(this.gameObject);
    }
}
