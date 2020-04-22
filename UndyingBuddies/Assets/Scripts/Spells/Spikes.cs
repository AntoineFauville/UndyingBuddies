using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private GameSettings _gameSettings;

    [SerializeField] private List<GameObject> allPriestTouched = new List<GameObject>();

    private int LiveSpellState;

    [SerializeField] private bool mod_Poison;
    [SerializeField] private bool mod_Physical;
    [SerializeField] private bool mod_MentalHealth;

    void Start()
    {
        LiveSpellState = 0;
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = new Color(0, 1, 0, 0.2f);
        Gizmos.DrawSphere(transform.position, _gameSettings.spikeSpell.Range);
    }

    void Update()
    {
        switch (LiveSpellState)
        {
            case 0: //spawn
                //play animation spawning
                StartCoroutine(AnimationSpawning());
                break;
            case 1: //fill information after spawning

                CheckAllTouchedEnemies();

                LiveSpellState = 2;
                break;
            case 2: //systemic check if i'm crossing an other spell
                SystemicCheck();
                LiveSpellState = 3;
                break;
            case 3: //check if enemies around & do cycle

                if (allPriestTouched.Count > 0)
                {
                    LiveSpellState = 4;
                }
                else
                {
                    LiveSpellState = 5;
                }

                break;
            case 4: //do spell

                ApplySpikeDamage();

                LiveSpellState = 5;

                break;
            case 5: //end the spell with animation
                //stop beam
                StartCoroutine(AnimationEnding());
                break;
        }

        //we can place this here because we just need to know on placement what we hit
       
    }

    void ApplySpikeDamage()
    {
        for (int i = 0; i < allPriestTouched.Count; i++)
        {
            if (mod_Poison)
            {
                allPriestTouched[i].gameObject.GetComponent<AIStatController>().TakeDamage(AiStatus.Physical, _gameSettings.spikeSpell.DamageToEnemy + _gameSettings.damageModForSpike_Poison);

                //change visual effect
            }
            else if (mod_Physical)
            {
                allPriestTouched[i].gameObject.GetComponent<AIStatController>().TakeDamage(AiStatus.Physical, _gameSettings.spikeSpell.DamageToEnemy + _gameSettings.damageModForSpike_Physical);

                //change visual effect
            }
            else if (mod_MentalHealth)
            {
                allPriestTouched[i].gameObject.GetComponent<AIStatController>().TakeDamage(AiStatus.MentalHealth, _gameSettings.spikeSpell.DamageToEnemy + _gameSettings.damageModForSpike_MentalHealth);

                //change visual effect
            }
            else
            {
                allPriestTouched[i].gameObject.GetComponent<AIStatController>().TakeDamage(AiStatus.Physical, _gameSettings.spikeSpell);
            }
            
            allPriestTouched[i].gameObject.GetComponent<AIPriest>().Stun(1);
        }
    }

    void CheckAllTouchedEnemies()
    {
        allPriestTouched.Clear();

        Collider[] HitCollider = Physics.OverlapSphere(this.transform.position, _gameSettings.spikeSpell.Range);

        for (int i = 0; i < HitCollider.Length; i++)
        {
            if (HitCollider[i].GetComponent<AIPriest>() != null && !HitCollider[i].GetComponent<AIPriest>().AmIBuilding)
            {
                allPriestTouched.Add(HitCollider[i].gameObject);
            }
        }
    }

    void SystemicCheck()
    {
        //check if spikes are in Physical Patch (Flammes)

        //check if spikes are in Horror Patch (Horror)

        //check if spikes are in Poison Patch (Poison)
    }

    IEnumerator AnimationSpawning()
    {
        yield return new WaitForSeconds(0.1f);

        LiveSpellState = 1;
    }

    IEnumerator AnimationEnding()
    {
        yield return new WaitForSeconds(0.2f);

        DestroyImmediate(this.gameObject);
    }
}
