using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Usables : MonoBehaviour
{
    public List <GameObject> Bush = new List<GameObject>();
    public List<GameObject> House = new List<GameObject>();
    public List<GameObject> Tree = new List<GameObject>();

    int nameID;
    int nameIDHouse;
    int nameIDTree;

    void Awake()
    {
        AddBush();
        AddHouse();
        AddTree();
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

    void AddHouse()
    {
        foreach (var house in GameObject.FindGameObjectsWithTag("House"))
        {
            House.Add(house);
            house.name = "House_" + nameIDHouse;
            nameIDHouse++;
        }
    }

    void AddTree()
    {
        foreach (var tree in GameObject.FindGameObjectsWithTag("Tree"))
        {
            Tree.Add(tree);
            tree.name = "Tree_" + nameIDTree;
            nameIDTree++;
        }
    }
}
