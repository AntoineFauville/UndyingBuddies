using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EyeOfDoom : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private GameObject Target;

    public int LiveSpellState;

    public int EyeState;

    [SerializeField] List<GameObject> listToCheck = new List<GameObject>();

    [SerializeField] private GameSettings gameSettings;
    private bool canDoDamage;

    private bool OnlyOneCoroutineAtTime;

    private int countOfExplosions = 0;

    // Start is called before the first frame update
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
                if (GameObject.Find("Main Camera").GetComponent<AiManager>().Priest.Count > 0) // if i have a building to attack make sure the priest can attack the building
                {
                    for (int i = 0; i < GameObject.Find("Main Camera").GetComponent<AiManager>().Priest.Count; i++)
                    {
                        if (!listToCheck.Contains(GameObject.Find("Main Camera").GetComponent<AiManager>().Priest[i])
                            && !GameObject.Find("Main Camera").GetComponent<AiManager>().Priest[i].GetComponent<AIPriest>().AmIBuilding)
                        {
                            listToCheck.Add(GameObject.Find("Main Camera").GetComponent<AiManager>().Priest[i]);
                        }
                    }
                }
                LiveSpellState = 2;
                break;
            case 2: //systemic check if i'm crossing an other spell
                SystemicCheck();
                LiveSpellState = 3;
                break;
            case 3: //find a target state
                if (Target == null)
                {
                    if (!OnlyOneCoroutineAtTime)
                    {
                        CheckClosestPriestToAttack();
                    }
                }
                break;
            case 4: //do spell
                //if we've done too many explosions we fade out
                if (countOfExplosions >= gameSettings.amountOfExplosionForEye)
                {
                    LiveSpellState = 5;
                }

                //1 find a target
                if (Target == null)
                {
                    //with this if target dies no matter what, the eye will despawn. might now be a great 
                    //game design decision but it's simple at least, find a target and chase her to death.
                    LiveSpellState = 5;
                }
                else
                {
                    switch (EyeState)
                    {
                        case 0: //go to target
                            if (Vector3.Distance(this.transform.position, Target.transform.position) < 4)
                            {
                                EyeState = 1;
                            }
                            else
                            {
                                navMeshAgent.destination = Target.transform.position;

                                Debug.DrawLine(this.transform.position, Target.transform.position, Color.white);
                            }
                            break;

                        case 1:
                            //create beam

                            // attack the dude
                            if (Target.GetComponent<AIStatController>() != null)
                            {
                                if (!canDoDamage)
                                {
                                    canDoDamage = true;
                                    StartCoroutine(EyeOfDoomDamage());
                                }
                            }
                            break;
                    }
                }
                // since this spell can navigate around it needs to check systemic every movement
                SystemicCheck();
                break;
            case 5: //end the spell with animation
                //stop beam
                StartCoroutine(AnimationEnding());
                break;
        }
    }

    public void CheckClosestPriestToAttack()
    {
        for (int i = 0; i < listToCheck.Count; i++)
        {
            if (listToCheck[i] == null)
            {
                listToCheck.Remove(listToCheck[i]);
            }
        }

        GameObject bestPriest = null;

        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        for (int i = 0; i < listToCheck.Count; i++)
        {
            if (listToCheck[i] == null)
            {
                listToCheck.Remove(listToCheck[i]);
            }
            else
            {
                Vector3 directionToTarget = listToCheck[i].transform.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestPriest = listToCheck[i];
                }
            }
        }

        Target = bestPriest;

        LiveSpellState = 4;
    }

    void SystemicCheck()
    {

    }

    IEnumerator AnimationSpawning()
    {
        yield return new WaitForSeconds(0.3f);

        LiveSpellState = 1;
    }

    IEnumerator AnimationEnding()
    {
        yield return new WaitForSeconds(0.2f);

        DestroyImmediate(this.gameObject);
    }

    IEnumerator EyeOfDoomDamage()
    {
        //explosion of sanity on enemy

        //this.transform.LookAt(Target.transform);

        Collider[] HitCollider = Physics.OverlapSphere(Target.transform.position, gameSettings.eyeSpell.Range);
        
        for (int i = 0; i < HitCollider.Length; i++)
        {
            if (HitCollider[i].GetComponent<AIPriest>() != null && !HitCollider[i].GetComponent<AIPriest>().AmIBuilding)
            {
                if (!HitCollider[i].GetComponent<AIPriest>().AmUnderEffect)
                {
                    HitCollider[i].GetComponent<AIPriest>().Stun(3);
                }

                HitCollider[i].GetComponent<AIStatController>().TakeDamage(AiStatus.MentalHealth, gameSettings.eyeSpell);
                HitCollider[i].GetComponent<AIPriest>().FearAmount += gameSettings.eyeSpell.FearAmount;
            }
        }

        countOfExplosions++;

        yield return new WaitForSeconds(0.4f);
        canDoDamage = false;
    }
}
