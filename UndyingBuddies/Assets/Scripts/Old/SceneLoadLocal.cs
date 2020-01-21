using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadLocal : MonoBehaviour
{

    SceneManagerDontDestroy SceneManagerDontDestroy;

    [SerializeField] private Animator animator;

    void Start()
    {
        animator.enabled = false;
        SceneManagerDontDestroy = GameObject.Find("SceneManager").GetComponent<SceneManagerDontDestroy>();
    }

    public void Play()
    {
        StartCoroutine(waitforSec());
    }

    IEnumerator waitforSec()
    {
        animator.enabled = true;
        animator.Play("launchGame");

        yield return new WaitForSeconds(0.45f);
        SceneManagerDontDestroy.LoadScene();
    }
}
