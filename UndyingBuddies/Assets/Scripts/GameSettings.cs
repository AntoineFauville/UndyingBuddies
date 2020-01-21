using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameSettings : ScriptableObject
{
    public int demonLife = 10;
    public int demonRangeOfDetection = 20;
    public int demonRangeOfCloseBy = 5;
    public GameObject woodResourcePrefab;
    public int maxWoodCanCarry = 5;
    public GameObject foodResourcePrefab;
    public int maxFoodCanCarry = 5;
    public GameObject energyResourcePrefab;
}
