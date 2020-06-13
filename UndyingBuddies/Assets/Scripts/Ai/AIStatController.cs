﻿using System.Collections;
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
        if (_aIPriest.PriestType == PriestType.soldier)
        {
            _aIPriest.healthAmount = _gameSettings.PriestHealth;
            _aIPriest.maxHealth = _gameSettings.PriestHealth;
            _aIPriest.fearMaxAmount = _gameSettings.PriestMaxFearAmount;
            _aIPriest.MentalHealthMaxAmount = _gameSettings.PriestMaxMentalHealth;
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

        int buffPhysicalDamage = 0;
        int buffSanityDamage = 0;
        int buffPoisonDamage = 0;

        //i'm noot checking all the buffs but this is to test since I know i will only have 1 buff active to test 
        if (GameObject.Find("Main Camera").GetComponent<BuffManager>().activeBuffs.Count > 0)
        {
            buffSanityDamage = GameObject.Find("Main Camera").GetComponent<BuffManager>().activeBuffs[0].BuffDamage;
        }

        Debug.Log("PhysicalResistance " + PhysicalResistance + "   MentalHealthResistance " + MentalHealthResistance);

        //
        switch (aiStatus)
        {
            case AiStatus.Physical:
                resistance = PhysicalResistance;
                finalDamage = spellArchetype.DamageToEnemy + buffPhysicalDamage + buffPoisonDamage - PhysicalResistance;
                _aIPriest.healthAmount -= finalDamage;
                break;
            case AiStatus.Fear:
                finalDamage = 0;
                break;
            case AiStatus.MentalHealth:
                resistance = MentalHealthResistance;
                finalDamage = spellArchetype.DamageToEnemy + buffSanityDamage - MentalHealthResistance;
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

        Debug.Log("finalDamage " + finalDamage);

        UpdateLifeBars();
    }

    public void TakeDamage(AiStatus aiStatus, int damage)
    {
        int finalDamage = 0;
        int resistance = 0;

        int buffPhysicalDamage = 0;
        int buffSanityDamage = 0;
        int buffPoisonDamage = 0;

        //i'm noot checking all the buffs but this is to test since I know i will only have 1 buff active to test 
        if (GameObject.Find("Main Camera").GetComponent<BuffManager>().activeBuffs.Count > 0)
        {
            buffSanityDamage = GameObject.Find("Main Camera").GetComponent<BuffManager>().activeBuffs[0].BuffDamage;
        }

        Debug.Log("PhysicalResistance " + PhysicalResistance + "   MentalHealthResistance " + MentalHealthResistance);

        //
        switch (aiStatus)
        {
            case AiStatus.Physical:
                resistance = PhysicalResistance;
                finalDamage = damage + buffPhysicalDamage + buffPoisonDamage - PhysicalResistance;
                _aIPriest.healthAmount -= finalDamage;
                break;
            case AiStatus.Fear:
                finalDamage = 0;
                break;
            case AiStatus.MentalHealth:
                resistance = MentalHealthResistance;
                finalDamage = damage + buffSanityDamage - MentalHealthResistance;
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

        Debug.Log("finalDamage " + finalDamage);

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
    
    IEnumerator HealSlowlyOverTime()
    {
        yield return new WaitForSeconds(1);

        if (_aIPriest.healthAmount < _aIPriest.maxHealth)
        {
            _aIPriest.healthAmount += 1;
        }

        StartCoroutine(HealSlowlyOverTime());
    }
    
    IEnumerator slowUpdate()
    {
        yield return new WaitForSeconds(0.3f);

        UpdateLifeBars();

        StartCoroutine(slowUpdate());
    }
}
