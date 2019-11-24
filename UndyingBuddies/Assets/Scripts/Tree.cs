using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    [SerializeField] private GameObject flamesObject;

    [SerializeField] private Animator treeAnimator;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.tag = "Tree";
        flamesObject.SetActive(false);

        if (treeAnimator != null)
        {
            treeAnimator.Play("TreeGrowing");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "FireZone")
        {
            StartCoroutine(waitToUnSetFire());
        }
    }

    IEnumerator waitToUnSetFire()
    {
        //print("bush in fire zone");

        if (treeAnimator != null)
        {
            treeAnimator.Play("TreeDying");
        }

        this.gameObject.tag = "NotATree";
        flamesObject.SetActive(true);

        if (GameObject.Find("GameController").GetComponent<Usables>().Tree.Contains(this.gameObject))
        {
            GameObject.Find("GameController").GetComponent<Usables>().Tree.Remove(this.gameObject);
        }
        yield return new WaitForSeconds(10f);

        this.transform.position = new Vector3(this.transform.position.x + Random.Range(-2f,2f), this.transform.position.y, this.transform.position.z + Random.Range(-2f, 2f));

        flamesObject.SetActive(false);
        this.gameObject.tag = "Tree";

        if (!GameObject.Find("GameController").GetComponent<Usables>().Tree.Contains(this.gameObject))
        {
            GameObject.Find("GameController").GetComponent<Usables>().Tree.Add(this.gameObject);
        }

        if (treeAnimator != null)
        {
            treeAnimator.Play("TreeGrowing");
        }
    }
}
