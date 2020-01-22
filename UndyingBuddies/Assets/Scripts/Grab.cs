using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    public bool grabbing;
    public bool conditionToReleaseMet;
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

            //hit is the object that i want to grab
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.GetComponent<Grabable>() != null && hit.transform.GetComponent<CharacterTypeTagger>() != null)
                {
                    if (hit.transform.GetComponent<CharacterTypeTagger>().characterType == CharacterType.demon)
                    {
                        //if it's a resource reset the relation with the building
                        if (hit.transform.GetComponent<TransformIntoResource>() != null)
                        {
                            //since we just moved the ai from 1 house to an other we need to clean that ai from whatever other building he was in
                            for (int i = 0; i < GameObject.Find("Main Camera").GetComponent<AiManager>().Buildings.Count; i++)
                            {
                                if (GameObject.Find("Main Camera").GetComponent<AiManager>().Buildings[i].GetComponent<Building>().StockPile.Contains(hit.transform.gameObject))
                                {
                                    GameObject.Find("Main Camera").GetComponent<AiManager>().Buildings[i].GetComponent<Building>().StockPile.Remove(hit.transform.gameObject);
                                }
                            }

                            hit.transform.GetComponent<TransformIntoResource>().BuildingWhereImPlaced = null;
                            hit.transform.GetComponent<TransformIntoResource>().spawnPoint = null;
                        }

                        StartCoroutine(waitForMouseToUnderstand(hit.transform.gameObject));
                    }
                }
            }
        }

        if (grabbedItem != null)
        {
            RaycastHit hitPos;
            Ray rayPos = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(rayPos, out hitPos))
            {
                if (hitPos.collider.tag == "switchJobArea" && grabbedItem.transform.GetComponent<AIDemons>() != null)
                {
                    conditionToReleaseMet = true;
                    posCurrentObject = hitPos.point;
                }
                else if(hitPos.collider.tag == "ResourceTransformer" && grabbedItem.transform.GetComponent<TransformIntoResource>() != null)
                {
                    if (hitPos.collider.GetComponentInParent<Building>().BuildingType == BuildingType.WoodCutter && grabbedItem.transform.GetComponent<TransformIntoResource>().myResourceType == ResourceType.wood)
                    {
                        conditionToReleaseMet = true;
                        posCurrentObject = hitPos.point;
                    }
                    else if (hitPos.collider.GetComponentInParent<Building>().BuildingType == BuildingType.FoodProcessor && grabbedItem.transform.GetComponent<TransformIntoResource>().myResourceType == ResourceType.food)
                    {
                        conditionToReleaseMet = true;
                        posCurrentObject = hitPos.point;
                    }
                }
                else
                {
                    if (hitPos.collider.tag == "Floor")
                    {
                        conditionToReleaseMet = true;
                        posCurrentObject = hitPos.point;
                    }
                    else
                    {
                        conditionToReleaseMet = false;
                    }
                }
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

        if (Input.GetMouseButtonDown(0) && grabbing && conditionToReleaseMet)
        {
            handAnim.Play("hand anim holdrelease");

            grabbedItem.GetComponent<Grabable>().grabbed = false;

            RaycastHit hitWhenRelease;
            Ray rayWhenRelease = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(rayWhenRelease, out hitWhenRelease))
            {
                //certain that i'm carrying a demon
                if (grabbedItem.transform.GetComponent<AIDemons>() != null && hitWhenRelease.collider.tag == "switchJobArea" && hitWhenRelease.collider.GetComponent<jobSwitcher>() != null)
                {
                    grabbedItem.transform.GetComponent<AIDemons>().SwitchJob(hitWhenRelease.collider.GetComponent<jobSwitcher>().jobSwitcherType);
                    grabbedItem.transform.GetComponent<AIDemons>().AssignedBuilding = hitWhenRelease.collider.gameObject;

                    //since we just moved the ai from 1 house to an other we need to clean that ai from whatever other building he was in
                    for (int i = 0; i < GameObject.Find("Main Camera").GetComponent<AiManager>().Buildings.Count; i++)
                    {
                        if (GameObject.Find("Main Camera").GetComponent<AiManager>().Buildings[i].GetComponent<Building>().AiAttributedToBuilding.Contains(grabbedItem))
                        {
                            GameObject.Find("Main Camera").GetComponent<AiManager>().Buildings[i].GetComponent<Building>().AiAttributedToBuilding.Remove(grabbedItem);
                        }
                    }

                    hitWhenRelease.collider.gameObject.transform.GetComponentInParent<Building>().AiAttributedToBuilding.Add(grabbedItem);
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
