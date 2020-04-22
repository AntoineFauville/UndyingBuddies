using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rats : MonoBehaviour
{
    [SerializeField] private GameSettings _gameSettings;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private GameObject Rat;

    public GameObject Target;
    
    int distance = 2;

    private int LiveSpellState;

    private bool RatReady;

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

                Vector3 pos = Target.transform.position;
                _navMeshAgent.destination = new Vector3(Random.Range(pos.x - distance, pos.x + distance), 0, Random.Range(pos.z - distance, pos.z + distance));

                Debug.DrawLine(this.transform.position, Target.transform.position, Color.white);

                LiveSpellState = 2;
                break;
            case 2: //systemic check if i'm crossing an other spell
                SystemicCheck();
                LiveSpellState = 3;
                break;
            case 3: //check if enemies around & do cycle

                RatReady = true;

                //to check if the rats walk again in systemic & refresh their walk
                LiveSpellState = 1;

                break;
            case 4: //do spell
                
                break;
            case 5: //end the spell with animation
                //stop beam
                StartCoroutine(AnimationEnding());
                break;
        }

        
    }

    void OnCollisionEnter(Collision coll)
    {
        Debug.Log(coll.gameObject.name);

        if (coll.gameObject.GetComponent<AIPriest>() != null && !coll.gameObject.GetComponent<AIPriest>().AmIBuilding && RatReady)
        {
            if (coll.gameObject.GetComponent<AIPriest>().AmUnderEffect == false)
            {
                coll.gameObject.GetComponent<AIPriest>().AmUnderEffect = true;
                coll.gameObject.GetComponent<AIPriest>().currentAiPriestEffects = AiPriestEffects.Poisoned;
            }

            coll.gameObject.GetComponent<AIStatController>().TakeDamage(AiStatus.Physical, _gameSettings.ratsSpell);

            LiveSpellState = 5;
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
        yield return new WaitForSeconds(0.1f);
        
        Destroy(Rat);
    }
}
