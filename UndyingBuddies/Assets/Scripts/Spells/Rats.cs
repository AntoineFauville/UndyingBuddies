using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rats : MonoBehaviour
{
    [SerializeField] private GameSettings _gameSettings;
    [SerializeField] private GameObject[] RatsEntities;

    [SerializeField] private List<GameObject> allPriestTouched = new List<GameObject>();
    [SerializeField] private List<GameObject> allPriestEatenByRats = new List<GameObject>();

    public GameObject Target;
    
    private int LiveSpellState;
    private bool coroutineOnce;

    [SerializeField] private bool mod_Physical;
    [SerializeField] private bool mod_Poison;
    [SerializeField] private bool mod_Horror;

   

    void Start()
    {
        
        LiveSpellState = 0;

        for (int i = 0; i < RatsEntities.Length; i++)
        {
            RatsEntities[i].GetComponent<RatSpell>().Target = Target;
            RatsEntities[i].GetComponent<RatSpell>().HorrorParts.SetActive(false);
            RatsEntities[i].GetComponent<RatSpell>().PoisonRat.SetActive(true);
        }
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = new Color(0, 1, 0, 0.2f);
        Gizmos.DrawSphere(transform.position, _gameSettings.ratsSpell.Range);
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
                
                //to check if the rats walk again in systemic & refresh their walk
                if (!coroutineOnce)
                {
                    DamageArea();
                    SlowArea();

                    if (mod_Horror)
                    {
                        for (int i = 0; i < RatsEntities.Length; i++)
                        {
                            RatsEntities[i].GetComponent<RatSpell>().HorrorParts.SetActive(true);
                        }
                        AddFear();
                    }

                    StartCoroutine(TickFrameDamage());
                }

                break;
            case 4: //do spell

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
        allPriestEatenByRats.Clear();

        Collider[] HitCollider = Physics.OverlapSphere(this.transform.position, _gameSettings.ratsSpell.Range);

        for (int i = 0; i < HitCollider.Length; i++)
        {
            if (HitCollider[i].GetComponent<AIPriest>() != null && !HitCollider[i].GetComponent<AIPriest>().AmIBuilding)
            {
                allPriestTouched.Add(HitCollider[i].gameObject);
            }
        }
    }

    void DamageArea()
    {
        if (mod_Physical)
        {
            if (allPriestTouched.Count > 0)
            {
                for (int i = 0; i < allPriestTouched.Count; i++)
                {
                    allPriestTouched[i].gameObject.GetComponent<AIStatController>().TakeDamage(AiStatus.Physical, _gameSettings.ratsSpell.DamageToEnemy + _gameSettings.damageModRat_Physical);
                }
            }

            for (int j = 0; j < RatsEntities.Length; j++)
            {
                GameObject ratExplosion = Instantiate(_gameSettings.RatExplosion);

                ratExplosion.transform.SetParent(RatsEntities[j].GetComponent<RatSpell>().RatExplosionSpawner.transform);
                ratExplosion.transform.position = RatsEntities[j].GetComponent<RatSpell>().RatExplosionSpawner.transform.position;
                ratExplosion.transform.localScale = RatsEntities[j].GetComponent<RatSpell>().RatExplosionSpawner.transform.localScale;
                ratExplosion.transform.rotation = new Quaternion();
                ratExplosion.SetActive(true);
            }
        }
        else if (mod_Poison)
        {
            if (allPriestTouched.Count > 0)
            {
                for (int i = 0; i < allPriestTouched.Count; i++)
                {
                    if (allPriestTouched[i] != null)
                    {
                        allPriestTouched[i].gameObject.GetComponent<AIStatController>().TakeDamage(AiStatus.Physical, _gameSettings.ratsSpell);
                    }
                }

                PoisonEnemy();
            }

            for (int i = 0; i < RatsEntities.Length; i++)
            {
                RatsEntities[i].GetComponent<RatSpell>().PoisonRat.SetActive(true);
            }
        }
        else
        {
            if (allPriestTouched.Count > 0)
            {
                for (int i = 0; i < allPriestTouched.Count; i++)
                {
                    allPriestTouched[i].gameObject.GetComponent<AIStatController>().TakeDamage(AiStatus.Physical, _gameSettings.ratsSpell);
                }
            }
        }
    }

    void PoisonEnemy()
    {
        if (allPriestTouched.Count > 0)
        {
            for (int i = 0; i < allPriestTouched.Count; i++)
            {
                int rand = Random.Range(0, 100);
                if (rand > _gameSettings.poisonExplosionSpell.chancesOfInfecting)
                {
                    if (allPriestTouched[i] != null)
                    {
                        if (allPriestTouched[i].GetComponent<AIPriest>().AmUnderEffect == false)
                        {
                            allPriestTouched[i].GetComponent<AIPriest>().AmUnderEffect = true;
                            allPriestTouched[i].GetComponent<AIPriest>().currentAiPriestEffects = AiPriestEffects.Poisoned;
                        }
                    }
                }
            }
        }
    }

    void SlowArea()
    {
        if (allPriestTouched.Count > 0)
        {
            for (int i = 0; i < allPriestTouched.Count; i++)
            {
                if(allPriestTouched[i] != null)
                {
                    allPriestTouched[i].gameObject.GetComponent<AIPriest>().Slow();
                }
            }
        }
    }

    void AddFear()
    {
        if (allPriestTouched.Count > 0)
        {
            for (int i = 0; i < allPriestTouched.Count; i++)
            {
                allPriestTouched[i].gameObject.GetComponent<AIPriest>().FearAmount += _gameSettings.ratsSpell.FearAmount;
            }
        }
    }

    void SystemicCheck()
    {
        //check if spikes are in Physical Patch (Flammes)
        if (!mod_Poison && !mod_Horror)
        {
            Collider[] HitColliderWithFlammes = Physics.OverlapSphere(this.transform.position, (float)_gameSettings.ratsSpell.Range +
                ((float)_gameSettings.fireSpell.Range / 2)//we take the sphere of the fire spell and we add half of the explosion spell to see if the two circle collides
                );

            for (int i = 0; i < HitColliderWithFlammes.Length; i++)
            {
                if (HitColliderWithFlammes[i].GetComponent<Flames>() != null)
                {
                    mod_Physical = true;

                    Debug.Log("I've hit " + HitColliderWithFlammes[i].name);
                }
            }
        }


        //check if spikes are in Horror Patch (Horror)
        if (!mod_Physical && !mod_Poison)
        {
            Collider[] HitColliderWithHorror = Physics.OverlapSphere(this.transform.position, (float)_gameSettings.ratsSpell.Range +
            ((float)_gameSettings.tentacleSpell.Range / 2)//we take the sphere of the fire spell and we add half of the explosion spell to see if the two circle collides
            );

            for (int i = 0; i < HitColliderWithHorror.Length; i++)
            {
                if (HitColliderWithHorror[i].GetComponent<Tentacle>() != null)
                {
                    mod_Horror = true;

                    Debug.Log("I've hit " + HitColliderWithHorror[i].name);
                }
            }
        }

        //check if spikes are in Poison Patch (Poison)
        if (!mod_Physical && !mod_Horror)
        {
            Collider[] HitColliderWithPoison = Physics.OverlapSphere(this.transform.position, (float)_gameSettings.ratsSpell.Range +
            ((float)_gameSettings.poisonExplosionSpell.Range / 2)//we take the sphere of the fire spell and we add half of the explosion spell to see if the two circle collides
            );

            for (int i = 0; i < HitColliderWithPoison.Length; i++)
            {
                if (HitColliderWithPoison[i].GetComponent<PoisonExplosion>() != null)
                {
                    mod_Poison = true;

                    Debug.Log("I've hit " + HitColliderWithPoison[i].name);
                }
            }
        }
    }

    IEnumerator AnimationSpawning()
    {
        yield return new WaitForSeconds(0.1f);

        LiveSpellState = 1;

        StartCoroutine(RatEnding());
    }

    IEnumerator RatEnding()
    {
        yield return new WaitForSeconds(_gameSettings.ratsSpell.spellTimer);

        LiveSpellState = 5;
    }

    IEnumerator AnimationEnding()
    {
        yield return new WaitForSeconds(0.1f);
        
        Destroy(this.gameObject);
    }

    IEnumerator TickFrameDamage()
    {
        coroutineOnce = true;
        yield return new WaitForSeconds(0.3f);
        LiveSpellState = 1;
        coroutineOnce = false;
    }
}
