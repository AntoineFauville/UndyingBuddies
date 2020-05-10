using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private GameSettings _gameSettings;

    [SerializeField] private List<GameObject> allPriestTouched = new List<GameObject>();

    private int LiveSpellState;

    [SerializeField] private GameObject PrefabSpike;
    [SerializeField] private bool mod_Poison;
    [SerializeField] private GameObject PrefabPoisonSpike;
    [SerializeField] private bool mod_Physical;
    [SerializeField] private GameObject PrefabFireSpike;
    [SerializeField] private bool mod_MentalHealth;
    [SerializeField] private GameObject PrefabHorrorSpike;

    private GameObject prefabSpawned;
    private bool spawnOncePrefab;

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
                
                ApplyVisuals();

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
        if (!mod_Poison && !mod_MentalHealth)
        {
            Collider[] HitColliderWithFlammes = Physics.OverlapSphere(this.transform.position, (float)_gameSettings.spikeSpell.Range +
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
            Collider[] HitColliderWithHorror = Physics.OverlapSphere(this.transform.position, (float)_gameSettings.spikeSpell.Range +
            ((float)_gameSettings.tentacleSpell.Range / 2)//we take the sphere of the fire spell and we add half of the explosion spell to see if the two circle collides
            );

            for (int i = 0; i < HitColliderWithHorror.Length; i++)
            {
                if (HitColliderWithHorror[i].GetComponent<Tentacle>() != null)
                {
                    mod_MentalHealth = true;

                    Debug.Log("I've hit " + HitColliderWithHorror[i].name);
                }
            }
        }

        //check if spikes are in Poison Patch (Poison)
        if (!mod_Physical && !mod_MentalHealth)
        {
            Collider[] HitColliderWithPoison = Physics.OverlapSphere(this.transform.position, (float)_gameSettings.spikeSpell.Range +
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

    void ApplyVisuals()
    {
        if (mod_MentalHealth)
        {
            //change visual effectif (!spawnOncePrefab)
            {
                spawnOncePrefab = true;
                prefabSpawned = Instantiate(PrefabHorrorSpike, this.transform.position, new Quaternion());
            }
        }
        else if (mod_Physical)
        {
            //change visual effect
            if (!spawnOncePrefab)
            {
                spawnOncePrefab = true;
                prefabSpawned = Instantiate(PrefabFireSpike, this.transform.position, new Quaternion());
            }
        }
        else if (mod_Poison)
        {
            //change visual effect
            if (!spawnOncePrefab)
            {
                spawnOncePrefab = true;
                prefabSpawned = Instantiate(PrefabPoisonSpike, this.transform.position, new Quaternion());
            }
        }
        else
        {
            if (!spawnOncePrefab)
            {
                spawnOncePrefab = true;
                prefabSpawned = Instantiate(PrefabSpike, this.transform.position, new Quaternion());
            }
        }
    }

    void ApplySpikeDamage()
    {
        if (allPriestTouched.Count > 0)
        {
            for (int i = 0; i < allPriestTouched.Count; i++)
            {
                if (mod_Poison)
                {
                    if (allPriestTouched[i] != null)
                    {
                        allPriestTouched[i].gameObject.GetComponent<AIStatController>().TakeDamage(AiStatus.Physical, _gameSettings.spikeSpell.DamageToEnemy + _gameSettings.damageModForSpike_Poison);
                    }

                    List<GameObject> allPriestThatArePoisoned = new List<GameObject>();

                    if (allPriestTouched.Count > 0)
                    {
                        for (int y = 0; y < allPriestTouched.Count; y++)
                        {
                            int rand = Random.Range(0, 100);
                            if (rand > _gameSettings.poisonExplosionSpell.chancesOfInfecting)
                            {
                                allPriestThatArePoisoned.Add(allPriestTouched[y].gameObject);
                            }
                        }
                    }

                    if (allPriestThatArePoisoned.Count > 0)
                    {
                        for (int j = 0; j < allPriestThatArePoisoned.Count; j++)
                        {
                            if (allPriestThatArePoisoned[j] != null)
                            {
                                if (allPriestThatArePoisoned[j].GetComponent<AIPriest>().AmUnderEffect == false)
                                {
                                    allPriestThatArePoisoned[j].GetComponent<AIPriest>().AmUnderEffect = true;
                                    allPriestThatArePoisoned[j].GetComponent<AIPriest>().currentAiPriestEffects = AiPriestEffects.Poisoned;
                                }
                            }
                        }
                    }
                }
                else if (mod_Physical)
                {
                    if (allPriestTouched[i] != null)
                    {
                        allPriestTouched[i].gameObject.GetComponent<AIStatController>().TakeDamage(AiStatus.Physical, _gameSettings.spikeSpell.DamageToEnemy + _gameSettings.damageModForSpike_Physical);
                    }
                }
                else if (mod_MentalHealth)
                {
                    if (allPriestTouched[i] != null)
                    {
                        allPriestTouched[i].gameObject.GetComponent<AIStatController>().TakeDamage(AiStatus.MentalHealth, _gameSettings.spikeSpell.DamageToEnemy + _gameSettings.damageModForSpike_MentalHealth);
                    }
                }
                else
                {
                    if (allPriestTouched[i] != null)
                    {
                        allPriestTouched[i].gameObject.GetComponent<AIStatController>().TakeDamage(AiStatus.Physical, _gameSettings.spikeSpell);
                    }
                }

                if (allPriestTouched[i] != null)
                {
                    allPriestTouched[i].gameObject.GetComponent<AIPriest>().Stun(1);
                }
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
        yield return new WaitForSeconds(0.4f);

        DestroyImmediate(prefabSpawned);
        DestroyImmediate(this.gameObject);
    }
}
