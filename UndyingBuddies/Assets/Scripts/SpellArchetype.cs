﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpellArchetype : ScriptableObject
{
    public string spellName;

    public Spell spellPrefab;
    public GameObject spellLeftAfterSpawned;
    public GameObject PlacementShowDebug;

    public int DamageToEnemy = 2;
    public int DamageToDemons = 1;

    public spellCanvasView UIToSpawn;

    public int SpellCostInEnergy;
    public int CostToUnlockEnergy;

    public Sprite backGroundImage;
}
