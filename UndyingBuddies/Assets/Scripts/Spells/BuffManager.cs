using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public List<Buff> activeBuffs = new List<Buff>();

    [SerializeField] private GameSettings _gameSettings;

    public void CreateBuff(BuffType buffType)
    {
        if (activeBuffs.Count <= 0)
        {
            Buff newBuff = Instantiate(_gameSettings.BuffPrefab);
            newBuff.Setup(buffType);

            activeBuffs.Add(newBuff);
        }
        else
        {
            Debug.Log("Max Buff Active");
        }
    }

    void Update()
    {
        if (activeBuffs.Count > 0)
        {
            for (int i = 0; i < activeBuffs.Count; i++)
            {
                if (activeBuffs[i].LifeCounter <= 0)
                {
                    RemoveBuff(activeBuffs[i]);
                }
            }
        }

        for (int i = 0; i < activeBuffs.Count; i++)
        {
            if (activeBuffs[i] == null)
            {
                activeBuffs.Remove(activeBuffs[i]);
            }
        }
    }

    void RemoveBuff(Buff buff)
    {
        activeBuffs.Remove(buff);
        DestroyImmediate(buff.gameObject);
    }
}
