using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] List<WaveType> WaveType = new List<WaveType>();
    public GameSettings GameSettings;

    private WaveType currentWave;
    public int currentWaveTimeIndex = 0;

    void Start()
    {
        currentWave = WaveType[currentWaveTimeIndex];

        StartCoroutine(waitForWaves());
    }

    IEnumerator waitForWaves()
    {
        GameObject aiformation = Instantiate(GameSettings.AIFormationPrefab, this.transform.position, new Quaternion());
        aiformation.GetComponent<AIFormation>().Setup(currentWave.EnemyAmount, currentWave.EnemyPrefab);

        yield return new WaitForSeconds(currentWave.waveTime);

        if (currentWaveTimeIndex < WaveType.Count - 1)//stricly smaller beacuse trhen we don't have a problem with the offset index
        {
            currentWaveTimeIndex++;
        }

        currentWave = WaveType[currentWaveTimeIndex];

        StartCoroutine(waitForWaves());
    }
}
