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

        for (int i = 0; i < GameObject.Find("Main Camera").GetComponent<AiManager>().Buildings.Count; i++) // DE-activate all the bouding box in case they would be activated
        {
            GameObject.Find("Main Camera").GetComponent<AiManager>().Buildings[i].GetComponent<Building>().BoudingBoxTag.SetActive(false);
            GameObject.Find("Main Camera").GetComponent<AiManager>().Buildings[i].GetComponent<Building>().detectPlacement.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
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

                if (hit.transform.GetComponent<Energy>() != null)
                {
                    GameObject.Find("Main Camera").GetComponent<ResourceManager>().amountOfEnergy += 1;
                    DestroyImmediate(hit.transform.gameObject);
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
                else if (hitPos.collider.tag == "switchJobArea" && grabbedItem.transform.GetComponent<AIDemons>() != null)
                {
                    conditionToReleaseMet = true;
                }
                else if (hitPos.collider.tag == "ResourceTransformer" && grabbedItem.transform.GetComponent<TransformIntoResource>() != null)
                {
                    if (hitPos.collider.GetComponentInParent<Building>().BuildingType == BuildingType.WoodCutter && grabbedItem.transform.GetComponent<TransformIntoResource>().myResourceType == ResourceType.wood)
                    {
                        conditionToReleaseMet = true;
                    }
                    else if (hitPos.collider.GetComponentInParent<Building>().BuildingType == BuildingType.FoodProcessor && grabbedItem.transform.GetComponent<TransformIntoResource>().myResourceType == ResourceType.food)
                    {
                        conditionToReleaseMet = true;
                    }
                    else if (hitPos.collider.GetComponentInParent<Building>().BuildingType == BuildingType.SpellHouse && grabbedItem.transform.GetComponent<TransformIntoResource>().myResourceType == ResourceType.energy)
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
                    for (int i = 0; i < GameObject.Find("Main Camera").GetComponent<AiManager>().Buildings.Count; i++) // activate all the bouding box if it's a building
                    {
                        GameObject.Find("Main Camera").GetComponent<AiManager>().Buildings[i].GetComponent<Building>().BoudingBoxTag.SetActive(true);
                    }

                    if (!grabbedItem.transform.GetComponent<Building>().detectPlacement.Detected)
                    {
                        grabbedItem.transform.GetComponent<Building>().BoudingBoxTag.GetComponent<MeshRenderer>().material.color = Color.white;
                        conditionToReleaseMet = true;
                    }
                    else
                    {
                        grabbedItem.transform.GetComponent<Building>().BoudingBoxTag.GetComponent<MeshRenderer>().material.color = Color.red;
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
                grabbedItem.transform.position = posCurrentObject;

            }
            else
            {
                grabbedItem.transform.position = posCurrentObject + new Vector3(0, 7, 0);
            }
            grabbedItem.layer = 2;
            HoldingAnything.SetActive(true);
            HoldingAnything.transform.position = posCurrentObject;

            if (grabbedItem.transform.GetComponent<Building>() != null && Input.GetButtonDown("E"))
            {
                DestroyImmediate(grabbedItem);
                Debug.Log("canceled building placement");
                HoldingAnything.SetActive(false);
            }
        }

        if (Input.GetMouseButtonDown(0) && grabbing && conditionToReleaseMet)
        {

            if (grabbedItem.transform.GetComponent<Rigidbody>() != null)
            {
                grabbedItem.transform.GetComponent<Rigidbody>().useGravity = true;
            }

            for (int i = 0; i < GameObject.Find("Main Camera").GetComponent<AiManager>().Buildings.Count; i++) // DE-activate all the bouding box in case they would be activated
            {
                GameObject.Find("Main Camera").GetComponent<AiManager>().Buildings[i].GetComponent<Building>().BoudingBoxTag.SetActive(false);
            }

            if (grabbedItem.transform.GetComponent<Building>() != null)
            {
                //cost if you do want to apply the spell on the ground
                if (grabbedItem.transform.GetComponent<Building>().BuildingType == BuildingType.CityHall)
                {
                    GameObject.Find("Main Camera").GetComponent<ResourceManager>().amountOfWood -= GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.cityhall.BuildingCostInWood;
                    GameObject.Find("Main Camera").GetComponent<ResourceManager>().amountOfFood -= GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.cityhall.BuildingCostInFood;
                }
                else if(grabbedItem.transform.GetComponent<Building>().BuildingType == BuildingType.FoodHouse)
                {
                    GameObject.Find("Main Camera").GetComponent<ResourceManager>().amountOfWood -= GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.foodHouse.BuildingCostInWood;
                    GameObject.Find("Main Camera").GetComponent<ResourceManager>().amountOfFood -= GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.foodHouse.BuildingCostInFood;
                }
                else if (grabbedItem.transform.GetComponent<Building>().BuildingType == BuildingType.WoodHouse)
                {
                    GameObject.Find("Main Camera").GetComponent<ResourceManager>().amountOfWood -= GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.woodHouse.BuildingCostInWood;
                    GameObject.Find("Main Camera").GetComponent<ResourceManager>().amountOfFood -= GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.woodHouse.BuildingCostInFood;
                }
                else if (grabbedItem.transform.GetComponent<Building>().BuildingType == BuildingType.SpellHouse)
                {
                    GameObject.Find("Main Camera").GetComponent<ResourceManager>().amountOfWood -= GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.spellHouse.BuildingCostInWood;
                    GameObject.Find("Main Camera").GetComponent<ResourceManager>().amountOfFood -= GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.spellHouse.BuildingCostInFood;
                }
                else if (grabbedItem.transform.GetComponent<Building>().BuildingType == BuildingType.Barrack)
                {
                    GameObject.Find("Main Camera").GetComponent<ResourceManager>().amountOfWood -= GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.Barrack.BuildingCostInWood;
                    GameObject.Find("Main Camera").GetComponent<ResourceManager>().amountOfFood -= GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.Barrack.BuildingCostInFood;
                }
                else if (grabbedItem.transform.GetComponent<Building>().BuildingType == BuildingType.FoodProcessor)
                {
                    GameObject.Find("Main Camera").GetComponent<ResourceManager>().amountOfWood -= GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.foodProcessor.BuildingCostInWood;
                    GameObject.Find("Main Camera").GetComponent<ResourceManager>().amountOfFood -= GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.foodProcessor.BuildingCostInFood;
                }
                else if (grabbedItem.transform.GetComponent<Building>().BuildingType == BuildingType.WoodCutter)
                {
                    GameObject.Find("Main Camera").GetComponent<ResourceManager>().amountOfWood -= GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.woodCutter.BuildingCostInWood;
                    GameObject.Find("Main Camera").GetComponent<ResourceManager>().amountOfFood -= GameObject.Find("Main Camera").GetComponent<AiManager>().GameSettings.woodCutter.BuildingCostInFood;
                }

                GameObject.Find("Main Camera").GetComponent<AiManager>().Buildings.Add(grabbedItem);
                GameObject.Find("Main Camera").GetComponent<AiManager>().Buildables.Add(grabbedItem);

                if (grabbedItem.transform.GetComponent<Building>().detectPlacement != null)
                {
                    grabbedItem.transform.GetComponent<Building>().detectPlacement.gameObject.SetActive(false);
                }
            }

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

            grabbedItem.transform.position = posCurrentObject;

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
