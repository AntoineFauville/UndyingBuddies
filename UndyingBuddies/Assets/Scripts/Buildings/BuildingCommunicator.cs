using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCommunicator : MonoBehaviour
{
    public Building MyBuilding;

    public Building BuildingSittingHere;

    [SerializeField] private GameObject _art;
    public bool artShow;
    [SerializeField] private GameObject _button;

    public bool hasBeenAssigned;

    [SerializeField] private Transform RefToSpawn;

    [SerializeField] private GameObject _groundConnection;

    // Start is called before the first frame update
    void Start()
    {
        ManageArt();

        hasBeenAssigned = false;

        _groundConnection.SetActive(false);

        FindConnection();
    }

    public void CreateHereBuilding()
    {
        if (GameObject.Find("Main Camera").GetComponent<ResourceManager>().amountOfEnergy >= 10)
        {
            if (!hasBeenAssigned)
            {
                GameObject.Find("Main Camera").GetComponent<BuildingCreator>().CreateBuildingHere(RefToSpawn);
                AssignBuildingHere();
                ManageArt();
            }
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "boundingBox")
        {
            Debug.Log("dats it bois we've hit the max of the map");

            AssignBuildingHere();
            ManageArt();

            StartCoroutine(waitSplitToDestroy());
        }
    }

    public void FindConnection()
    {
        Collider[] HitCollider = Physics.OverlapSphere(this.transform.position, 5);

        for (int i = 0; i < HitCollider.Length; i++)
        {
            if (HitCollider[i].GetComponent<Building>() != null)
            {
                BuildingSittingHere = HitCollider[i].GetComponent<Building>();

                AssignBuildingHere();

                this.gameObject.name = "Com_To_" + HitCollider[i].gameObject.name;

                ManageArt();
            }
        }
    }

    public void AssignBuildingHere()
    {
        hasBeenAssigned = true;
    }

    public void ManageArt()
    {
        if (artShow && !hasBeenAssigned)
        {
            _art.SetActive(true);
            _button.SetActive(true);
        }
        else if(!artShow && !hasBeenAssigned)
        {
            _art.SetActive(false);
            _button.SetActive(false);
        }

        if (hasBeenAssigned)
        {
            _art.SetActive(false);
            _button.SetActive(false);
            _groundConnection.SetActive(true);
        }
    }

    IEnumerator waitSplitToDestroy()
    {
        yield return new WaitForSeconds(0.2f);

        DestroyImmediate(RefToSpawn.gameObject);
    }
}
