﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public int Health;
    public BuildingType BuildingType;

    public bool canBeInteractable;

    public GameObject PreGround;
    public GameObject Ground;

    public int BuildingCreation = 0;

    public GameObject SpawningPoint;

    public List<GameObject> AiAttributedToBuilding = new List<GameObject>();

    public List<GameObject> StockPile = new List<GameObject>();

    void Start()
    {
        if (BuildingType == BuildingType.Barrack)
            Health = GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.Barrack.BuildingHealth;
        if (BuildingType == BuildingType.CityHall)
            Health = GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.cityhall.BuildingHealth;
        if (BuildingType == BuildingType.FoodHouse)
            Health = GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.foodHouse.BuildingHealth;
        if (BuildingType == BuildingType.WoodHouse)
            Health = GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.woodHouse.BuildingHealth;
        if (BuildingType == BuildingType.SpellHouse)
            Health = GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.spellHouse.BuildingHealth;

        this.gameObject.GetComponent<CharacterTypeTagger>().characterType = CharacterType.neutral;

        GameObject.Find("Main Camera").GetComponent<AiManager>().Buildings.Add(this.gameObject);
    }

    void Update()
    {
        if (BuildingCreation >= 100)
        {
            PreGround.SetActive(false);
            Ground.SetActive(true);

            if (GameObject.Find("Main Camera").GetComponent<AiManager>().Buildables.Contains(this.gameObject))
            {
                GameObject.Find("Main Camera").GetComponent<AiManager>().Buildables.Remove(this.gameObject);
            }
        }
    }
}
