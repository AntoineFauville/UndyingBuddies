using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameConditions : MonoBehaviour
{
    [SerializeField] private GameObject cityHall;

    [SerializeField] private GameObject DestroyToWin_01;
    [SerializeField] private GameObject DestroyToWin_02;

    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject loosePanel;

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
            if (DestroyToWin_01 == null && DestroyToWin_02 == null)
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
