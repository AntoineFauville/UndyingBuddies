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
        

        if (_aIPriest.PriestType == PriestType.soldier)
        {
            _aIPriest.healthAmount = _gameSettings.PriestHealth;
            _aIPriest.MentalHealthAmount = 0;
        }
        else if (_aIPriest.PriestType == PriestType.building)
        {
            _aIPriest.healthAmount = _gameSettings.PriestBuildingHealth;
        }

        StartCoroutine(HealSlowlyOverTime());
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
                _aIPriest.MentalHealthAmount -= spellArchetype.DamageToEnemy - MentalHealthResistance;
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
        _aIPriest.MentalHealthMaxAmount = _gameSettings.PriestMaxMentalHealth;
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
    }

    void OnTriggerExit()
    {
        _aIPriest.amIInFire = false;
    }

    IEnumerator HealSlowlyOverTime()
    {
        yield return new WaitForSeconds(1);

        StartCoroutine(HealSlowlyOverTime());
    }

    IEnumerator waitToStopFire()
    {
        _aIPriest.amIInFire = true;
        yield return new WaitForSeconds(1.4f);
        _aIPriest.amIInFire = false;
    }

    IEnumerator slowUpdate()
    {
        yield return new WaitForSeconds(0.7f);

        if (_aIPriest.healthAmount <= 0 || _aIPriest.MentalHealthAmount == _aIPriest.MentalHealthMaxAmount)
        {
            _aIPriest.Die();
        }

        if (_aIPriest.amIInFire)
        {
            TakeDamage(AiStatus.Physical, _gameSettings.fireSpell);
        }
        
        StartCoroutine(slowUpdate());
    }
}
