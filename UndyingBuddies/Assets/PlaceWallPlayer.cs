using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceWallPlayer : MonoBehaviour
{
    [SerializeField] private WallFactory _wallFactory;
    [SerializeField] private GameObject _cubePreviewPrefab;

    private GameObject cubeObject;
    private Quaternion cubeOrientation;

    private bool ableToSpawnAgain;

    void Start()
    {
        cubeObject = Instantiate(_cubePreviewPrefab);
        cubeObject.GetComponent<MeshRenderer>().material.color = new Color(0,100,0,0.5f);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !ableToSpawnAgain)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Floor")
                {
                    StartCoroutine(CreateWallThenWait(hit.point, cubeOrientation));
                }

                if (hit.transform.tag == "Boy")
                {
                    StartCoroutine(SetBoyOnFire());
                    hit.transform.GetComponent<Survive>().boyNeedState = BoyNeedState.RunToSurviveFire;
                }
            }
        }

        ShowCube();
    }

    void ShowCube()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "Floor")
            {
                cubeObject.SetActive(true);
                cubeObject.transform.position = hit.point;
            }
            else
            {
                cubeObject.SetActive(false);
            }
        }

        cubeObject.transform.Rotate(new Vector3(0,-1,0));

        cubeOrientation = cubeObject.transform.rotation;
    }

    IEnumerator CreateWallThenWait(Vector3 hit, Quaternion cubeOrientation)
    {
        ableToSpawnAgain = true;
        _wallFactory.CreateWall(hit, cubeOrientation);
        cubeObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 100, 0.1f);
        yield return new WaitForSeconds(2f);
        ableToSpawnAgain = false;
        cubeObject.GetComponent<MeshRenderer>().material.color = new Color(0, 100, 0, 0.5f);
    }

    IEnumerator SetBoyOnFire()
    {
        ableToSpawnAgain = true;
        cubeObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 100, 0.1f);
        yield return new WaitForSeconds(4f);
        ableToSpawnAgain = false;
        cubeObject.GetComponent<MeshRenderer>().material.color = new Color(0, 100, 0, 0.5f);
    }
}
