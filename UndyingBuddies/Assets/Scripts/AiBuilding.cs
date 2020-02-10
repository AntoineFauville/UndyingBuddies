using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiBuilding : MonoBehaviour
{
    public GameObject normal;
    public GameObject destroyed;

    bool hasBeenDestroyedOnce;

    [SerializeField] private CharacterTypeTagger characterTypeTagger;

    // Start is called before the first frame update
    void Awake()
    {
        normal.SetActive(true);
        destroyed.SetActive(false);

        characterTypeTagger.characterType = CharacterType.neutral;
    }

    // Update is called once per frame
    public void Destroy()
    {
        if (!hasBeenDestroyedOnce)
        {
            hasBeenDestroyedOnce = true;
            this.gameObject.tag = "priestHouse";
            normal.SetActive(false);
            destroyed.SetActive(true);

            //i'm destroyed you can eat me now

            characterTypeTagger.characterType = CharacterType.demon;
        }
    }
}
