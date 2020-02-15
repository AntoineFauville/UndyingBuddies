using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    public bool grabbing;
    public bool conditionToReleaseMet;
    public GameObject grabbedItem;
    public bool notUsingSpell;
    [SerializeField] Vector3 posCurrentObject;
    [SerializeField] GameObject PointerWhereMouseAt;
    [SerializeField] Animator handAnim;

    [SerializeField] ResourceManager ResourceManager;
    [SerializeField] AiManager AiManager;

    [SerializeField] GameObject grabbedPosition;
    GameObject parentOfGrabbedObject;

    [SerializeField] private Sacrifice Sacrifice;
    [SerializeField] private bool canSacrificeAgain;

    // Start is called before the first frame update
    void Start()
    {
        if (PointerWhereMouseAt == null)
        {
            PointerWhereMouseAt = GameObject.Find("HoldingAnything");
        }

        PointerWhereMouseAt.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //if you over on top of resource building then you can sacrifice resource for energy
        if (Input.GetMouseButton(1) && !grabbing && !notUsingSpell && !canSacrificeAgain)
        {
            canSacrificeAgain = true;

            StartCoroutine(waitToBeAbleToSacrifice());

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "whiteSoulsStock")
                {
                    Sacrifice.TransformIntoEnergy(ResourceType.whiteSoul, hit.transform.gameObject, handAnim);
                }
                else if (hit.collider.tag == "blueVioletSoulsStock")
                {
                    Sacrifice.TransformIntoEnergy(ResourceType.blueVioletSoul, hit.transform.gameObject, handAnim);
                }

                if (hit.collider.transform.GetComponent<Building>() == null)
                {
                    if (hit.collider.tag == "Resource" || hit.collider.tag == "demon" || hit.collider.tag == "priestHouse")
                    {
                        if (AiManager.Demons.Count <= 1 && hit.collider.GetComponent<AIDemons>() != null) // if we have a demon and we only have one demon or less lol don't comit suicide
                        {
                            Debug.Log("Really man ? don't kill yourself like that");
                        }
                        else
                        {
                            Debug.Log("sacrifice");

                            handAnim.Play("hand anim Sacrifice");

                            for (int i = 0; i < AiManager.Buildings.Count; i++)
                            {
                                if (AiManager.Buildings[i].GetComponent<Building>().AiAttributedToBuilding.Contains(hit.collider.gameObject))
                                {
                                    AiManager.Buildings[i].GetComponent<Building>().AiAttributedToBuilding.Remove(hit.collider.gameObject);
                                    hit.collider.transform.GetComponent<AIDemons>().ResetVisuals();
                                }
                            }

                            parentOfGrabbedObject = null;

                            PointerWhereMouseAt.SetActive(false);

                            grabbing = false;

                            Sacrifice.SacrificeForLordSavior(hit.collider.gameObject, AiManager);

                            grabbedItem = null;
                        }
                    }
                }
            }
        }

        //this manage is you can click or not on the objects
        if (Input.GetMouseButtonDown(0) && !grabbing && !notUsingSpell)
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
                        StartCoroutine(waitForMouseToUnderstand(hit.transform.gameObject));
                    }
                }
            }
        }

        //this is when you hold the object and you have one
        if (grabbedItem != null)
        {
            RaycastHit hitPos;
            Ray rayPos = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(rayPos, out hitPos))
            {
                if (hitPos.collider.tag != "boundingBox")
                {
                    posCurrentObject = hitPos.point;
                }

                if (hitPos.collider.tag == "terrainWarFlagOnly")
                {
                    if (grabbedItem.transform.tag == "flag" || grabbedItem.transform.tag == "FireZone")
                    {
                        conditionToReleaseMet = true;
                    }
                    else
                    {
                        conditionToReleaseMet = false;
                    }
                }
                else if (grabbedItem.transform.GetComponent<Building>() != null)//if i'm a building i want to make sure the ground is working to be placed
                {
                    for (int i = 0; i < AiManager.Buildings.Count; i++) // activate all the bouding box if it's a building
                    {
                        AiManager.Buildings[i].GetComponent<Building>().BoudingBoxTag.SetActive(true);
                    }

                    grabbedItem.transform.GetComponent<Building>().detectPlacement.gameObject.SetActive(true);
                    grabbedItem.transform.GetComponent<Building>().BoudingBoxTag.SetActive(false);

                    if (!grabbedItem.transform.GetComponent<Building>().detectPlacement.Detected)
                    {
                        grabbedItem.transform.GetComponent<Building>().detectPlacement.GetComponent<MeshRenderer>().material.color = Color.white;
                        conditionToReleaseMet = true;
                    }
                    else
                    {
                        grabbedItem.transform.GetComponent<Building>().detectPlacement.GetComponent<MeshRenderer>().material.color = Color.red;
                        conditionToReleaseMet = false;
                    }
                }
                else
                {
                    if (hitPos.collider.tag == "Floor")
                    {
                        conditionToReleaseMet = true;
                    }
                    else
                    {
                        conditionToReleaseMet = false;
                    }
                }
            }
        }
        else
        {
            grabbing = false;
        }

        if (grabbing)
        {
            handAnim.Play("hand anim hold");
            grabbedItem.GetComponent<Grabable>().grabbed = true;
            if (grabbedItem.transform.GetComponent<Building>() != null)
            {
                Vector3 newVector = new Vector3(Mathf.Round(posCurrentObject.x), 0, Mathf.Round(posCurrentObject.z));

                grabbedItem.transform.position = newVector;
            }
            else
            {
                grabbedItem.transform.SetParent(grabbedPosition.transform);
                grabbedItem.transform.position = grabbedPosition.transform.position;
                grabbedItem.transform.rotation = grabbedPosition.transform.rotation;
                grabbedItem.transform.localScale = new Vector3(1, 1, 1);
            }
            grabbedItem.layer = 2;
            PointerWhereMouseAt.SetActive(true);
            PointerWhereMouseAt.transform.position = posCurrentObject;

            if (grabbedItem.transform.GetComponent<Building>() != null && Input.GetMouseButtonDown(1))
            {
                DestroyImmediate(grabbedItem);
                grabbedItem = null;
                grabbing = false;
                Debug.Log("canceled building placement");
                handAnim.Play("hand anim holdrelease");
                PointerWhereMouseAt.SetActive(false);
            }
        }

        if (Input.GetMouseButtonDown(1) && grabbing)
        {
            for (int i = 0; i < AiManager.Buildings.Count; i++) // DE-activate all the bouding box in case they would be activated
            {
                AiManager.Buildings[i].GetComponent<Building>().BoudingBoxTag.SetActive(false);
            }

            if (grabbedItem.transform.GetComponent<Building>() == null)
            {
                if (grabbedItem.tag == "wood" || grabbedItem.tag == "food" || grabbedItem.tag == "demon" || grabbedItem.tag == "priestHouse")
                {
                    if (AiManager.Demons.Count <= 1 && grabbedItem.GetComponent<AIDemons>() != null) // if we have a demon and we only have one demon or less lol don't comit suicide
                    {
                        Debug.Log("Really man ? don't kill yourself like that");
                    }
                    else
                    {
                        Debug.Log("sacrifice");

                        handAnim.Play("hand anim Sacrifice");

                        for (int i = 0; i < AiManager.Buildings.Count; i++)
                        {
                            if (AiManager.Buildings[i].GetComponent<Building>().AiAttributedToBuilding.Contains(grabbedItem))
                            {
                                AiManager.Buildings[i].GetComponent<Building>().AiAttributedToBuilding.Remove(grabbedItem);
                                grabbedItem.transform.GetComponent<AIDemons>().ResetVisuals();
                            }
                        }

                        parentOfGrabbedObject = null;

                        PointerWhereMouseAt.SetActive(false);

                        grabbing = false;

                        Sacrifice.SacrificeForLordSavior(grabbedItem, AiManager);

                        grabbedItem = null;
                    }
                }
            }

        }

        if (Input.GetMouseButtonDown(0) && grabbing && conditionToReleaseMet)
        {
            if (grabbedItem.transform.GetComponent<Rigidbody>() != null)
            {
                grabbedItem.transform.GetComponent<Rigidbody>().useGravity = true;
            }

            for (int i = 0; i < AiManager.Buildings.Count; i++) // DE-activate all the bouding box in case they would be activated
            {
                AiManager.Buildings[i].GetComponent<Building>().BoudingBoxTag.SetActive(false);
            }

            if (grabbedItem.transform.GetComponent<Building>() != null)
            {
                ResourceManager.ManageCostOfPurchaseForBuilding();

                if (!AiManager.Buildings.Contains(grabbedItem))
                {
                    AiManager.Buildings.Add(grabbedItem);
                }

                if (grabbedItem.transform.GetComponent<Building>().amountOfActiveWorker < grabbedItem.transform.GetComponent<Building>().amountOfWorkerAllowed 
                    && !AiManager.BuildingWithJobs.Contains(grabbedItem))
                {
                    AiManager.BuildingWithJobs.Add(grabbedItem);
                }

                if (grabbedItem.transform.GetComponent<Building>().detectPlacement != null)
                {
                    grabbedItem.transform.GetComponent<Building>().detectPlacement.gameObject.SetActive(false);
                }

                if (grabbedItem.transform.GetComponent<Building>().resourceProducedAtBuilding == ResourceType.whiteSoul)
                {
                    AiManager.WhiteSoulStockage.Add(grabbedItem);
                }

                if (grabbedItem.transform.GetComponent<Building>().resourceProducedAtBuilding == ResourceType.blueVioletSoul)
                {
                    AiManager.BlueVioletSoulStockage.Add(grabbedItem);
                }
            }

            handAnim.Play("hand anim holdrelease");

            grabbedItem.GetComponent<Grabable>().grabbed = false;
            
            grabbedItem.layer = 0;

            if (parentOfGrabbedObject != null)
            {
                grabbedItem.transform.SetParent(parentOfGrabbedObject.transform);
            }
            grabbedItem.transform.position = new Vector3(posCurrentObject.x, 0, posCurrentObject.z);
            grabbedItem.transform.rotation = new Quaternion();
            grabbedItem.transform.localScale = new Vector3(1,1,1);
            
            parentOfGrabbedObject = null;

            grabbedItem = null;

            PointerWhereMouseAt.SetActive(false);

            grabbing = false;
        }
    }

    IEnumerator waitForMouseToUnderstand(GameObject hit)
    {
        yield return new WaitForSeconds(0.05f);

        PointerWhereMouseAt.SetActive(true);

        grabbing = true;
        
        grabbedItem = hit.transform.gameObject;

        if (hit.transform.GetComponent<Rigidbody>() != null)
        {
            hit.transform.GetComponent<Rigidbody>().useGravity = false;
        }

        if(grabbedItem.transform.parent != null)
            parentOfGrabbedObject = grabbedItem.transform.parent.gameObject;
    }

    IEnumerator waitToBeAbleToSacrifice()
    {
        GameObject.Find("Main Camera").GetComponent<CameraDrag>().enabled = false;

        yield return new WaitForSeconds(0.2f);

        GameObject.Find("Main Camera").GetComponent<CameraDrag>().enabled = true;
        canSacrificeAgain = false;
    }
}
