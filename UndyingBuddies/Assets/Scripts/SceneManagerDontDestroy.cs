using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerDontDestroy : MonoBehaviour
{
    int scene;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        LoadScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene()
    {
        scene++;

        if (scene >= 3)
        {
            scene = 1;
        }

        SceneManager.LoadScene(scene);
    }
}
