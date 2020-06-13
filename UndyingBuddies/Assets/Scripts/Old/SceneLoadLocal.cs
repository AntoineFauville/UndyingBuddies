using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadLocal : MonoBehaviour
{

    SceneManagerDontDestroy SceneManagerDontDestroy;

    [SerializeField] private Animator animator;

    [SerializeField] private GameObject LoadingScreen;

    void Start()
    {
        LoadingScreen.SetActive(false);

        animator.enabled = false;

        SceneManagerDontDestroy = GameObject.Find("SceneManager").GetComponent<SceneManagerDontDestroy>();
    }

    public void Play()
    {
        StartCoroutine(waitforSec());
    }

    IEnumerator waitforSec()
    {
        LoadingScreen.SetActive(true);

        yield return new WaitForSeconds(0.45f);
        SceneManagerDontDestroy.LoadScene();
    }
}
