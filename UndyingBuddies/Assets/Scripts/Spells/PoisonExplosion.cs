using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonExplosion : MonoBehaviour
{
    [SerializeField] private GameSettings _gameSettings;

    private int LiveSpellState = 0;

    [SerializeField] private List<GameObject> allPriestTouched = new List<GameObject>();
    [SerializeField] private List<GameObject> allPriestThatArePoisoned = new List<GameObject>();

    bool doCoroutineOnce;

    private int counterForSpellLongevity;

    private float ticRate = 0.3f;

    [SerializeField] private GameObject FlamesExplosions;
    private GameObject _flammesExplosion;
    bool DoOnce;

    [SerializeField] private GameObject PoisonPatchPrefab;
    private GameObject _poisonPrefab;
    private bool InstanciateOnce;
    private Vector3 OffsettedPosition;

    public bool alreadyCollidedWithFireExplosion;

    // Start is called before the first frame update
    void Start()
    {
        OffsettedPosition = new Vector3(this.transform.position.x, this.transform.position.y + 0.2f, this.transform.position.z);

        LiveSpellState = 0;
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = new Color(0, 1, 0, 0.2f);
        Gizmos.DrawSphere(transform.position, _gameSettings.poisonExplosionSpell.Range);
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
                    PoisonEnemy();

                    DamageEnemy();

                    StartCoroutine(WaitToTriggerDamageOrSkip());
                }
                break;
            case 5: //end the spell with animation
                //stop beam
                StartCoroutine(AnimationEnding());
                break;
        }
    }

    void CheckAllTouchedEnemies()
    {
        allPriestTouched.Clear();
        allPriestThatArePoisoned.Clear();

        Collider[] HitCollider = Physics.OverlapSphere(this.transform.position, _gameSettings.poisonExplosionSpell.Range);

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
        if (((float)counterForSpellLongevity * ticRate) > _gameSettings.poisonExplosionSpell.spellTimer)
        {
            LiveSpellState = 5;
        }
    }

    void PoisonEnemy()
    {
        if (allPriestThatArePoisoned.Count > 0)
        {
            for (int i = 0; i < allPriestThatArePoisoned.Count; i++)
            {
                if (allPriestThatArePoisoned[i] == null)
                {
                    allPriestThatArePoisoned.Remove(allPriestThatArePoisoned[i]);
                }
            }
        }

        if (allPriestTouched.Count > 0)
        {
            for (int i = 0; i < allPriestTouched.Count; i++)
            {
                int rand = Random.Range(0, 100);
                if (rand > _gameSettings.poisonExplosionSpell.chancesOfInfecting)
                {
                    allPriestThatArePoisoned.Add(allPriestTouched[i].gameObject);
                }
            }
        }

        if (allPriestTouched.Count > 0)
        {
            for (int i = 0; i < allPriestThatArePoisoned.Count; i++)
            {
                if (allPriestThatArePoisoned[i] != null && allPriestThatArePoisoned[i].GetComponent<AIPriest>().AmUnderEffect == false)
                {
                    allPriestThatArePoisoned[i].GetComponent<AIPriest>().AmUnderEffect = true;
                    allPriestThatArePoisoned[i].GetComponent<AIPriest>().currentAiPriestEffects = AiPriestEffects.Poisoned;
                }
            }
        }
    }

    void DamageEnemy()
    {
        if (allPriestTouched.Count > 0)
        {
            for (int i = 0; i < allPriestTouched.Count; i++)
            {
                if (allPriestTouched[i] != null)
                {
                    allPriestTouched[i].gameObject.GetComponent<AIStatController>().TakeDamage(AiStatus.Physical, _gameSettings.poisonExplosionSpell);
                }
            }
        }
    }

    void SystemicCheck()
    {

    }

    public void CollideWithFireSpell()
    {
        alreadyCollidedWithFireExplosion = true;
        
        if (!DoOnce)
        {
            DoOnce = true;
            _flammesExplosion = Instantiate(FlamesExplosions, this.transform.position, new Quaternion());

            for (int i = 0; i < allPriestTouched.Count; i++)
            {
                allPriestTouched[i].gameObject.GetComponent<AIStatController>().TakeDamage(AiStatus.Physical, _gameSettings.poisonExplosionSpell);
            }

        }

        Debug.Log("I'm turning into a burning hell and destroy myself");

        //now check if you need to turn other poison patch into fire

        StartCoroutine(delayOnExplosionCheck());
    }

    private void ExplodeNextOne()
    {
        Collider[] HitCollider = Physics.OverlapSphere(this.transform.position, (float)_gameSettings.poisonExplosionSpell.Range +
                    ((float)_gameSettings.poisonExplosionSpell.Range / 2)//we take the sphere of the fire spell and we add half of the explosion spell to see if the two circle collides
                    );

        for (int i = 0; i < HitCollider.Length; i++)
        {
            //please make sure to not infinite loop by taking ourself into account
            if (HitCollider[i].GetComponent<PoisonExplosion>() != null && !HitCollider[i].GetComponent<PoisonExplosion>().alreadyCollidedWithFireExplosion)
            {
                HitCollider[i].GetComponent<PoisonExplosion>().CollideWithFireSpell();

                Debug.Log("I've hit " + HitCollider[i].name);
            }
        }
    }

    IEnumerator AnimationSpawning()
    {
        if (!InstanciateOnce)
        {
            InstanciateOnce = true;
            _poisonPrefab = Instantiate(PoisonPatchPrefab, OffsettedPosition, new Quaternion());
        }

        yield return new WaitForSeconds(0.3f);

        LiveSpellState = 1;
    }

    IEnumerator AnimationEnding()
    {
        yield return new WaitForSeconds(0.2f);

        DestroyImmediate(_poisonPrefab);
        DestroyImmediate(_flammesExplosion);
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
        yield return new WaitForSeconds(0.2f);

        //check if we are colliding with poison
        ExplodeNextOne();

        StartCoroutine(AnimationEnding());
    }

    
}
