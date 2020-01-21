using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    public bool grabbing;
    public GameObject grabbedItem;
    [SerializeField] Vector3 posCurrentObject;
    [SerializeField] GameObject HoldingAnything;
    [SerializeField] Animator handAnim;

    // Start is called before the first frame update
    void Start()
    {
        if (HoldingAnything == null)
        {
            HoldingAnything = GameObject.Find("HoldingAnything");
        }

        HoldingAnything.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !grabbing)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.GetComponent<Grabable>() != null && hit.transform.GetComponent<CharacterTypeTagger>() != null)
                {
                    if (hit.transform.GetComponent<CharacterTypeTagger>().characterType == CharacterType.demon)
                    {
                        StartCoroutine(waitForMouseToUnderstand(hit.transform.gameObject));
                    }
                }
            }
        }

        RaycastHit hitPos;
        Ray rayPos = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(rayPos, out hitPos))
        {
            if (hitPos.collider.tag == "Floor" || hitPos.collider.tag == "switchJobArea")
            {
                posCurrentObject = hitPos.point;
            }
        }

        if (grabbing)
        {
            handAnim.Play("hand anim hold");
            grabbedItem.GetComponent<Grabable>().grabbed = true;
            grabbedItem.transform.position = posCurrentObject + new Vector3 (0,7,0);
            grabbedItem.layer = 2;
            HoldingAnything.SetActive(true);
            HoldingAnything.transform.position = posCurrentObject;
        }

        if (Input.GetMouseButtonDown(0) && grabbing)
        {
            handAnim.Play("hand anim holdrelease");

            grabbedItem.GetComponent<Grabable>().grabbed = false;

            RaycastHit hitWhenRelease;
            Ray rayWhenRelease = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(rayWhenRelease, out hitWhenRelease))
            {
                if (hitWhenRelease.collider.GetComponent<jobSwitcher>() == null)
                {
                    Debug.Log("couldn't find a jobswitcher on this area, please add one");
                }

                //certain that i'm carrying a demon
                if (grabbedItem.transform.GetComponent<AIDemons>() != null && hitWhenRelease.collider.tag == "switchJobArea" && hitWhenRelease.collider.GetComponent<jobSwitcher>() != null)
                {
                    grabbedItem.transform.GetComponent<AIDemons>().SwitchJob(hitWhenRelease.collider.GetComponent<jobSwitcher>().jobSwitcherType);
                    grabbedItem.transform.GetComponent<AIDemons>().AssignedBuilding = hitWhenRelease.collider.gameObject;
                }
            }

            grabbedItem.layer = 0;

            if (grabbedItem.transform.GetComponent<Rigidbody>() != null)
            {
                grabbedItem.transform.GetComponent<Rigidbody>().useGravity = true;
            }

            grabbedItem = null;

            HoldingAnything.SetActive(false);

            grabbing = false;
        }
    }

    IEnumerator waitForMouseToUnderstand(GameObject hit)
    {
        yield return new WaitForSeconds(0.1f);

        HoldingAnything.SetActive(true);

        grabbing = true;
        grabbedItem = hit.transform.gameObject;

        if (hit.transform.GetComponent<Rigidbody>() != null)
        {
            hit.transform.GetComponent<Rigidbody>().useGravity = false;
        }
    }
}
