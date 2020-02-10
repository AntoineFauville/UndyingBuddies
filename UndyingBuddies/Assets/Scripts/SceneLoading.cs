using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoading : MonoBehaviour
{
    // Start is called before the first frame update
    public void Restart()
    {
        SceneManager.LoadScene(2);
    }

    // Update is called once per frame
    public void ExitMenu()
    {
        SceneManager.LoadScene(1);
    }
}
