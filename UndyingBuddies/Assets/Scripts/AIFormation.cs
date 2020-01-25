using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIFormation : MonoBehaviour
{
    //have the same amount of spawnpoint as ai
    //if lower ai than spawnpoint it's ok but not more ai than spawnpoint

    public List<GameObject> spawnPoint = new List<GameObject>();

    public List<GameObject> aiOnMe = new List<GameObject>();

    [SerializeField] private NavMeshAgent navMeshAgent;

    [Range(1,12)]
    public int amountOfAiInFormation;

    public void Setup(int amountOfEnemy, GameObject enemyPrefab)
    {
        amountOfAiInFormation = amountOfEnemy;

        navMeshAgent.destination = GameObject.Find("CityHall").transform.position;

        for (int i = 0; i < amountOfAiInFormation; i++)
        {
            GameObject aiPriest = Instantiate(enemyPrefab, spawnPoint[i].transform.position, new Quaternion());
            aiPriest.transform.GetComponent<AIPriest>().CanAttackBack = true;
            aiPriest.transform.GetComponent<AIPriest>().PriestAttackerType = PriestAttackerType.followFormation;
            aiPriest.transform.GetComponent<AIPriest>().aiFormationFollowPoint = spawnPoint[i];
            aiOnMe.Add(aiPriest);
        }

        StartCoroutine(SlowUpdate());
    }

    IEnumerator SlowUpdate()
    {
        for (int i = 0; i < aiOnMe.Count; i++) //if an ia on me dies, then destroy formation
        {
            if (aiOnMe[i] == null)
            {
                aiOnMe.Remove(aiOnMe[i]);
            }
        }

        if (aiOnMe.Count <= 0)
        {
            DestroyImmediate(this.gameObject);
        }

        yield return new WaitForSeconds(1);

        StartCoroutine(SlowUpdate());
    }
}
