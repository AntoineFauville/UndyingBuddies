using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameConditions : MonoBehaviour
{
    [SerializeField] private GameObject cityHall;

    [SerializeField] private AITown[] TownsToDestroy;

    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject loosePanel;

    [SerializeField] private int townLeft = 0;

    void Start()
    {
        winPanel.SetActive(false);
        loosePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (cityHall != null)
        {
            int townLeft = 0;
            for (int i = 0; i < TownsToDestroy.Length; i++)
            {
                if (TownsToDestroy[i].isTheVillageDestroyed == false)
                {
                    townLeft += 1;
                }
            }

            if (townLeft <= 0)
            {
                Win();
            }
        }
        else
        {
            Loose();
        }
    }

    void Win()
    {
        winPanel.SetActive(true);
        StartCoroutine(waitforSeconds());
    }

    void Loose()
    {
        loosePanel.SetActive(true);
        StartCoroutine(waitforSeconds());
    }

    IEnumerator waitforSeconds()
    {
        yield return new WaitForSeconds(3);

        SceneManager.LoadScene(0);
    }
}
