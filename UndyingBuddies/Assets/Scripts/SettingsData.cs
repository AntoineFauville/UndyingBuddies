using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SettingsData : ScriptableObject
{
    public int BoyAmount = 5;
    public GameObject BoyPrefab;

    public BushLife BushlifePrefab;
    public float BushHealth = 50f;

    public float radius = 2;
    public int MaxBushOnMap = 10;

    public float BushSpawnTimeMinimum = 1f;
    public float BushSpawnTimeMaxium = 2f;

    public int BoyAmountToLoose = 30;

    public float ReproductionSpeedMin = 1f;
    public float ReproductionSpeedMax = 4f;

    public Wall PrefabWall;
    public GameObject _cubePreviewPrefab;

    public GameObject WoodHouse;
    public int woodNeedToFinishHouse;
    public int woodGetSpeed = 1;

    public int woodForFirstStateOfHouse = 10;
    public int woodForSecondStateOfHouse = 20;
    public int woodForThirdStateOfHouse = 30;
}
