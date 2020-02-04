using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITown : MonoBehaviour
{
    [SerializeField] private List<AIPriest> AllRelatedAIOfThisTown = new List<AIPriest>();

    bool Revenge;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < AllRelatedAIOfThisTown.Count; i++)
        {
            if (AllRelatedAIOfThisTown[i].Health < AllRelatedAIOfThisTown[i].maxHealth)
            {
                Revenge = true;
            }
        }

        if (Revenge)
        {
            for (int i = 0; i < AllRelatedAIOfThisTown.Count; i++)
            {
                AllRelatedAIOfThisTown[i].PriestAttackerType = PriestAttackerType.rusher;
            }
        }
    }
}
