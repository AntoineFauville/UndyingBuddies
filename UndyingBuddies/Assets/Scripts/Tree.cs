using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    [SerializeField] private GameObject flamesObject;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.tag = "Tree";
        flamesObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "FireZone")
        {
            print("bush in fire zone");
            this.gameObject.tag = null;
            flamesObject.SetActive(true);
            StartCoroutine(waitToUnSetFire());
        }
    }

    IEnumerator waitToUnSetFire()
    {
        yield return new WaitForSeconds(5f);
        flamesObject.SetActive(false);
        this.gameObject.tag = "Tree";
    }
}
