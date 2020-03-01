using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStatController : MonoBehaviour
{
    //Resistance
    public int PhysicalResistance;
    public int MentalHealthResistance;

    [SerializeField] private AIPriest _aIPriest;
    [SerializeField] private GameSettings _gameSettings;

    [SerializeField] private GameObject spawnPointCanvas;
    
    void Start()
    {
        _aIPriest.MentalHealthMaxAmount = _gameSettings.PriestMaxMentalHealth;

        if (_aIPriest.PriestType == PriestType.soldier)
        {
            _aIPriest.healthAmount = _gameSettings.PriestHealth;
            _aIPriest.maxHealth = _gameSettings.PriestHealth;
            _aIPriest.MentalHealthAmount = 0;
        }

        UpdateLifeBars();

        StartCoroutine(HealSlowlyOverTime());
        StartCoroutine(slowUpdate());
    }

    public void TakeDamage(AiStatus aiStatus, SpellArchetype spellArchetype)
    {
        int finalDamage = 0;
        int resistance = 0;
        //
        switch (aiStatus)
        {
            case AiStatus.Physical:
                resistance = PhysicalResistance;
                finalDamage = spellArchetype.DamageToEnemy - PhysicalResistance;
                _aIPriest.healthAmount -= finalDamage;
                break;
            case AiStatus.Fear:
                finalDamage = 0;
                break;
            case AiStatus.MentalHealth:
                resistance = MentalHealthResistance;
                finalDamage = spellArchetype.DamageToEnemy - MentalHealthResistance;
                _aIPriest.MentalHealthAmount += finalDamage;
                break;
            case AiStatus.Lonelyness:
                finalDamage = 0;
                break;
            case AiStatus.IntestineStatus:
                finalDamage = 0;
                break;
        }

        CanvasDamage canvasDamage = Instantiate(_gameSettings.CanvasDamagePrefab, spawnPointCanvas.transform.position, new Quaternion());
        canvasDamage.SetupCanvasDamage(aiStatus, finalDamage, resistance);

        UpdateLifeBars();

        
    }

    void UpdateLifeBars()
    {
        if (_aIPriest.UiHealth != null)
        {
            _aIPriest.UiHealth.life = _aIPriest.healthAmount;
            _aIPriest.UiHealth.maxLife = _gameSettings.PriestHealth;

            _aIPriest.UiHealth.MentalHealth = _aIPriest.MentalHealthAmount;
            _aIPriest.UiHealth.maxMentalHealth = _gameSettings.PriestMaxMentalHealth;
        }
    }

    void OnTriggerStay(Collider collider)
    {
        if (_aIPriest.PriestType == PriestType.soldier)
        {
            if (collider.tag == "FireZone")
            {
                if (_aIPriest.amIInFire == false)
                {
                    StartCoroutine(waitToStopFire());
                }
            }

            if (collider.tag == "tentacleZone")
            {
                if (_aIPriest.attackedByTentacle == false)
                {
                    StartCoroutine(waitForTentacleEffect());
                }
            }

            if (collider.tag == "spikeZone")
            {
                if (_aIPriest.attackedBySpike == false)
                {
                    StartCoroutine(Spikes());
                }
            }
        }
    }

    IEnumerator HealSlowlyOverTime()
    {
        yield return new WaitForSeconds(1);

        if (_aIPriest.healthAmount < _aIPriest.maxHealth)
        {
            _aIPriest.healthAmount += 1;
        }

        StartCoroutine(HealSlowlyOverTime());
    }

    IEnumerator waitToStopFire()
    {
        for (int i = 0; i < 4; i++)
        {
            if (!_aIPriest.amIInFire)
            {
                TakeDamage(AiStatus.Physical, _gameSettings.fireSpell);
            }
            _aIPriest.amIInFire = true;
            yield return new WaitForSeconds(0.5f);
        }
        _aIPriest.amIInFire = false;
    }

    IEnumerator Spikes()
    {
        if (!_aIPriest.attackedBySpike)
        {
            TakeDamage(AiStatus.Physical, _gameSettings.spikeSpell);
        }

        _aIPriest.attackedBySpike = true;
        _aIPriest.Stun(2);

        yield return new WaitForSeconds(1);

        _aIPriest.attackedBySpike = false;
    }

    IEnumerator waitForTentacleEffect()
    {
        if (!_aIPriest.attackedByTentacle) //otherwise it checks infinitly
        {
            TakeDamage(AiStatus.MentalHealth, _gameSettings.tentacleSpell);
        }

        _aIPriest.attackedByTentacle = true;

        _aIPriest.Stun(0.2f);

        yield return new WaitForSeconds(0.2f);

        _aIPriest.attackedByTentacle = false;
    }

    IEnumerator slowUpdate()
    {
        yield return new WaitForSeconds(0.3f);

        UpdateLifeBars();

        StartCoroutine(slowUpdate());
    }
}
