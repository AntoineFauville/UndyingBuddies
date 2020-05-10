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

    public bool turnPoisonousOnce;
    public bool alreadyCollidedWithFireExplosionAfterPoison;
    private bool DoOnce;

    [SerializeField] private GameObject _poisonPrefab;

    [SerializeField] private bool mod_Poison;
    [SerializeField] private GameObject ARt;

    void Start()
    {
        LiveSpellState = 0;
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = new Color(0, 1, 0, 0.2f);
        Gizmos.DrawSphere(transform.position, _gameSettings.tentacleSpell.Range);
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
        if (allPriestTouched.Count > 0)
        {
            for (int i = 0; i < allPriestTouched.Count; i++)
            {
                if (allPriestTouched[i] != null)
                {
                    if (mod_Poison)
                    {
                        allPriestTouched[i].gameObject.GetComponent<AIStatController>().TakeDamage(AiStatus.Physical, _gameSettings.tentacleSpell);
                    }
                    else
                    {
                        allPriestTouched[i].gameObject.GetComponent<AIStatController>().TakeDamage(AiStatus.MentalHealth, _gameSettings.tentacleSpell);
                        allPriestTouched[i].gameObject.GetComponent<AIPriest>().FearAmount += _gameSettings.tentacleSpell.FearAmount;
                    }
                }
                else
                {
                    allPriestTouched.Remove(allPriestTouched[i]);
                }
            }
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
        //poison patch
        if (!turnPoisonousOnce)
        {
            Collider[] HitCollider = Physics.OverlapSphere(this.transform.position, (float)_gameSettings.tentacleSpell.Range +
            ((float)_gameSettings.poisonExplosionSpell.Range / 2)//we take the sphere of the fire spell and we add half of the explosion spell to see if the two circle collides
            );

            if (HitCollider.Length > 0)
            {
                for (int i = 0; i < HitCollider.Length; i++)
                {
                    if (HitCollider[i].GetComponent<PoisonExplosion>() != null)
                    {
                        Debug.Log("I've hit " + HitCollider[i].name);

                        turnPoisonousOnce = true;

                        TurnIntoPoisonous();
                    }
                }
            }
        }
        else
        {
            //if i'm poisonoous check to turn the other dudes around poisonous too continuously
            TurnNextOneIntoPoisonous();
        }

        //eye

    }

    public void TurnIntoPoisonous()
    {
        turnPoisonousOnce = true;

        mod_Poison = true;

        ARt.SetActive(false);

        _poisonPrefab = Instantiate(_gameSettings.PoisonousTentacles, this.transform.position, new Quaternion());
    }

    private void TurnNextOneIntoPoisonous() // checks if we can propagate the fire transformation to other flammes spell, this tells the other flames around me taht they should explode
    {
        Collider[] HitCollider = Physics.OverlapSphere(this.transform.position, (float)_gameSettings.tentacleSpell.Range +
                    ((float)_gameSettings.tentacleSpell.Range / 2 + 1)//we take the sphere of the fire spell and we add half of the explosion spell to see if the two circle collides
                    );

        for (int i = 0; i < HitCollider.Length; i++)
        {
            //please make sure to not infinite loop by taking ourself into account
            if (HitCollider[i].GetComponent<Tentacle>() != null && !HitCollider[i].GetComponent<Tentacle>().turnPoisonousOnce)
            {
                HitCollider[i].GetComponent<Tentacle>().TurnIntoPoisonous();

                Debug.Log("I've hit " + HitCollider[i].name);
            }
        }
    }

    // once it's poisonous it can explode

    public void ExplodeOncePoisonous()
    {
        alreadyCollidedWithFireExplosionAfterPoison = true;

        if (!DoOnce)
        {
            DoOnce = true;
            Instantiate(_gameSettings.FlamesExplosion, this.transform.position, new Quaternion());

            for (int i = 0; i < allPriestTouched.Count; i++)
            {
                allPriestTouched[i].gameObject.GetComponent<AIStatController>().TakeDamage(AiStatus.Physical, _gameSettings.tentacleSpell);
            }

        }

        Debug.Log("I'm turning into a burning hell and destroy myself");

        //now check if you need to turn other tentacle patch into explosion

        StartCoroutine(delayOnExplosionCheck());
    }

    private void ExplodeNextOne() // checks if we can propagate the fire transformation to other flammes spell, this tells the other flames around me taht they should explode
    {
        Collider[] HitCollider = Physics.OverlapSphere(this.transform.position, (float)_gameSettings.tentacleSpell.Range +
                    ((float)_gameSettings.fireSpell.Range / 2)//we take the sphere of the fire spell and we add half of the explosion spell to see if the two circle collides
                    );

        for (int i = 0; i < HitCollider.Length; i++)
        {
            //please make sure to not infinite loop by taking ourself into account
            //WATCHOUT this is only when the tentacle has become poisonous, it can now explode with fire! But hasn't exploded yet!
            if (HitCollider[i].GetComponent<Tentacle>() != null && HitCollider[i].GetComponent<Tentacle>().turnPoisonousOnce 
                && !HitCollider[i].GetComponent<Tentacle>().alreadyCollidedWithFireExplosionAfterPoison)
            {
                HitCollider[i].GetComponent<Tentacle>().ExplodeOncePoisonous();

                Debug.Log("I've hit " + HitCollider[i].name);
            }
        }

        Collider[] HitColliderWithPoisonPatch = Physics.OverlapSphere(this.transform.position, (float)_gameSettings.tentacleSpell.Range +
                   ((float)_gameSettings.poisonExplosionSpell.Range / 2)//we take the sphere of the fire spell and we add half of the explosion spell to see if the two circle collides
                   );

        for (int i = 0; i < HitColliderWithPoisonPatch.Length; i++)
        {
            //please make sure to not infinite loop by taking ourself into account
            //WATCHOUT this is only when the tentacle has become poisonous, it can now explode with fire! But hasn't exploded yet!
            if (HitColliderWithPoisonPatch[i].GetComponent<PoisonExplosion>() != null && !HitColliderWithPoisonPatch[i].GetComponent<PoisonExplosion>().alreadyCollidedWithFireExplosion)
            {
                HitColliderWithPoisonPatch[i].GetComponent<PoisonExplosion>().CollideWithFireSpell();

                Debug.Log("I've hit " + HitCollider[i].name);
            }
        }
    }
    
    IEnumerator AnimationSpawning()
    {
        yield return new WaitForSeconds(0.1f);

        LiveSpellState = 1;
    }

    IEnumerator AnimationEnding()
    {
        yield return new WaitForSeconds(0.2f);

        if (_poisonPrefab != null)
        {
            DestroyImmediate(_poisonPrefab);
        }
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

    IEnumerator delayOnExplosionCheck()
    {
        yield return new WaitForSeconds(0.1f);

        if (_poisonPrefab != null)
        {
            DestroyImmediate(_poisonPrefab);
        }

        //check if we are colliding with poison
        ExplodeNextOne();

        StartCoroutine(AnimationEnding());
    }
}
