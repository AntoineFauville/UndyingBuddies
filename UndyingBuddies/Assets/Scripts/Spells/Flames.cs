using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flames : MonoBehaviour
{
    [SerializeField] private GameSettings _gameSettings;

    [SerializeField] private List<GameObject> allPriestTouched = new List<GameObject>();
    [SerializeField] private List<GameObject> allPriestThatWillBeOnFire = new List<GameObject>();

    [SerializeField] private GameObject PrefabEntrance;
    private GameObject _prefab_Entrance;
    bool hasEntranceBeenSpawned;
    [SerializeField] private GameObject PrefabMid;
    private GameObject _prefab_Mid;
    bool hasMidBeenSpawned;
    [SerializeField] private GameObject PrefabEnd;
    private GameObject _prefab_end;
    bool hasEndBeenSpawned;

    private Vector3 VectorOffset;

    private int LiveSpellState = 0;

    private int counterForSpellLongevity;

    private float ticRate = 0.3f;

    private bool doCoroutineOnce;

    void Start()
    {
        LiveSpellState = 0;
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = new Color(0, 1, 0, 0.2f);
        Gizmos.DrawSphere(transform.position, _gameSettings.fireSpell.Range);
    }

    void Update()
    {
        switch (LiveSpellState)
        {
            case 0: //spawn

                VectorOffset = new Vector3(this.transform.position.x, this.transform.position.y + 0.05f, this.transform.position.z);

                //play animation spawning
                StartCoroutine(AnimationSpawning());
                break;
            case 1: //fill information after spawning

                CheckAllTouchedEnemies();

                //since this is an explosion, we are attacking the enemy now, the fire dot is left after
                DamageOfTheExplosion();

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
                    FireEnemy(); //fire dot
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

    void CheckAllTouchedEnemies()
    {
        allPriestTouched.Clear();
        allPriestThatWillBeOnFire.Clear();

        Collider[] HitCollider = Physics.OverlapSphere(this.transform.position, _gameSettings.fireSpell.Range);

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

    void FireEnemy()
    {
        for (int i = 0; i < allPriestTouched.Count; i++)
        {
            int rand = Random.Range(0, 100);
            if (rand > _gameSettings.fireSpell.chancesOfInfecting)
            {
                allPriestThatWillBeOnFire.Add(allPriestTouched[i].gameObject);
            }
        }

        for (int i = 0; i < allPriestThatWillBeOnFire.Count; i++)
        {
            if (allPriestThatWillBeOnFire[i].GetComponent<AIPriest>().AmUnderEffect == false)
            {
                allPriestThatWillBeOnFire[i].GetComponent<AIPriest>().AmUnderEffect = true;
                allPriestThatWillBeOnFire[i].GetComponent<AIPriest>().currentAiPriestEffects = AiPriestEffects.OnFire;
            }
        }
    }

    void DamageOfTheExplosion()
    {
        for (int i = 0; i < allPriestTouched.Count; i++)
        {
            allPriestTouched[i].gameObject.GetComponent<AIStatController>().TakeDamage(AiStatus.Physical, _gameSettings.poisonExplosionSpell);
        }
    }

    void SystemicCheck()
    {
        //check if we are colliding with poison
        Collider[] HitCollider = Physics.OverlapSphere(this.transform.position, (float)_gameSettings.fireSpell.Range + 
            ((float)_gameSettings.poisonExplosionSpell.Range/2)//we take the sphere of the fire spell and we add half of the explosion spell to see if the two circle collides
            );

        for (int i = 0; i < HitCollider.Length; i++)
        {
            if (HitCollider[i].GetComponent<PoisonExplosion>() != null)
            {
                HitCollider[i].GetComponent<PoisonExplosion>().CollideWithFireSpell();

                Debug.Log("I've hit " + HitCollider[i].name);
            }
        }

        //creates the zone if nothing interfear with it

        if (!hasMidBeenSpawned)
        {
            _prefab_Mid = Instantiate(PrefabMid, VectorOffset, new Quaternion());
            hasMidBeenSpawned = true;
        }
    }

    IEnumerator AnimationSpawning()
    {
        if (!hasEntranceBeenSpawned)
        {
            _prefab_Entrance = Instantiate(PrefabEntrance, VectorOffset, new Quaternion());
            hasEntranceBeenSpawned = true;
        }

        yield return new WaitForSeconds(0.1f);

        LiveSpellState = 1;
    }

    IEnumerator AnimationEnding()
    {
        if (!hasEndBeenSpawned)
        {
            Instantiate(PrefabEnd, VectorOffset, new Quaternion());
            hasEndBeenSpawned = true;
        }

        DestroyImmediate(_prefab_Mid);

        yield return new WaitForSeconds(0.2f);

        //clean the existing left overs
        DestroyImmediate(_prefab_end);
        DestroyImmediate(_prefab_Entrance);
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
