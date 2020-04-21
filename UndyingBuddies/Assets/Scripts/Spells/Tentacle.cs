using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
    [SerializeField] private GameSettings _gameSettings;

    [SerializeField] private List<GameObject> allPriestTouched = new List<GameObject>();

    private int LiveSpellState = 0;

    private int counterForSpellLongevity;

    private float ticRate = 0.3f;

    private bool doCoroutineOnce;

    void Start()
    {
        LiveSpellState = 0;
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

                //to check when to destroy the spell
                CheckHowLongSpellStillNeedToLast();

                if (allPriestTouched.Count <= 0)
                {
                    if (!doCoroutineOnce)
                    {
                        StartCoroutine(WaitToTriggerDamageOrSkip());
                    }
                }
                else
                {
                    //if there are enemies then do damage to them
                    LiveSpellState = 4;
                }
                break;
            case 4: //do spell
                
                if (!doCoroutineOnce)
                {
                    TentacleEffect(); //tentacle
                    //placed in here to only activate once per tic

                    StartCoroutine(WaitToTriggerDamageOrSkip());
                }
                break;
            case 5: //end the spell with animation
                //stop beam
                StartCoroutine(AnimationEnding());
                break;
        }

    }

    void TentacleEffect()
    {
        for (int i = 0; i < allPriestTouched.Count; i++)
        {
            allPriestTouched[i].gameObject.GetComponent<AIStatController>().TakeDamage(AiStatus.MentalHealth, _gameSettings.tentacleSpell);
            allPriestTouched[i].gameObject.GetComponent<AIPriest>().FearAmount += _gameSettings.tentacleSpell.FearAmount;
        }
    }

    void CheckAllTouchedEnemies()
    {
        allPriestTouched.Clear();

        Collider[] HitCollider = Physics.OverlapSphere(this.transform.position, _gameSettings.tentacleSpell.Range);

        for (int i = 0; i < HitCollider.Length; i++)
        {
            if (HitCollider[i].GetComponent<AIPriest>() != null && !HitCollider[i].GetComponent<AIPriest>().AmIBuilding)
            {
                allPriestTouched.Add(HitCollider[i].gameObject);
            }
        }
    }

    void CheckHowLongSpellStillNeedToLast()
    {
        if (((float)counterForSpellLongevity * ticRate) > _gameSettings.fireSpell.spellTimer)
        {
            LiveSpellState = 5;
        }
    }

    void SystemicCheck()
    {
       
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

    IEnumerator WaitToTriggerDamageOrSkip()
    {
        doCoroutineOnce = true;

        yield return new WaitForSeconds(ticRate);

        LiveSpellState = 2;

        counterForSpellLongevity++;

        doCoroutineOnce = false;
    }
}
