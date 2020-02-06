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
    
    void Start()
    {
        _aIPriest.maxHealth = _aIPriest.healthAmount;
        _aIPriest.MentalHealthMaxAmount = _gameSettings.PriestMaxMentalHealth;

        if (_aIPriest.PriestType == PriestType.soldier)
        {
            _aIPriest.healthAmount = _gameSettings.PriestHealth;
            _aIPriest.MentalHealthAmount = 0;
        }
        else if (_aIPriest.PriestType == PriestType.building)
        {
            _aIPriest.healthAmount = _gameSettings.PriestBuildingHealth;
        }

        UpdateLifeBars();

        StartCoroutine(HealSlowlyOverTime());
        StartCoroutine(slowUpdate());
    }

    public void TakeDamage(AiStatus aiStatus, SpellArchetype spellArchetype)
    {
        //
        switch (aiStatus)
        {
            case AiStatus.Physical:
                _aIPriest.healthAmount -= spellArchetype.DamageToEnemy - PhysicalResistance;
                break;
            case AiStatus.Fear:
                break;
            case AiStatus.MentalHealth:
                _aIPriest.MentalHealthAmount += spellArchetype.DamageToEnemy - MentalHealthResistance;
                break;
            case AiStatus.Lonelyness:
                break;
            case AiStatus.IntestineStatus:
                break;
        }

        UpdateLifeBars();
    }

    void UpdateLifeBars()
    {
        _aIPriest.UiHealth.life = _aIPriest.healthAmount;
        _aIPriest.UiHealth.maxLife = _gameSettings.PriestHealth;
    }

    void OnTriggerStay(Collider collider)
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

        yield return new WaitForSeconds(0.2f);

        _aIPriest.attackedByTentacle = false;
    }

    IEnumerator slowUpdate()
    {
        yield return new WaitForSeconds(0.3f);

        if (_aIPriest.healthAmount <= 0 || _aIPriest.MentalHealthAmount == _aIPriest.MentalHealthMaxAmount)
        {
            _aIPriest.Die();
        }

        StartCoroutine(slowUpdate());
    }
}
