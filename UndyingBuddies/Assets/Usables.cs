using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Usables : MonoBehaviour
{
    public List <GameObject> Bush = new List<GameObject>();

    int nameID;

    void Awake()
    {
        AddBush();
    }

    void AddBush()
    {
        foreach (var bush in GameObject.FindGameObjectsWithTag("Bush"))
        {
            Bush.Add(bush);
            bush.name = "Bush_" + nameID;
            nameID++;
        }
    }
}
