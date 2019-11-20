using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagerDontDestroy : MonoBehaviour
{
    [SerializeField] private Text textSceneDebug;

    int scene;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        LoadScene();
    }
    
    void Update()
    {
        textSceneDebug.text = SceneManager.GetActiveScene().name;

        if (Input.GetButtonDown("Cancel"))
        {
            Application.Quit();
        }
    }

    public void LoadScene()
    {
        scene++;

        if (scene > 3)
        {
            scene = 1;
        }

        SceneManager.LoadScene(scene);
    }
}
