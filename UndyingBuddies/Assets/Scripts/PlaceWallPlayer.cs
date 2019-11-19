using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceWallPlayer : MonoBehaviour
{
    [SerializeField] private WallFactory _wallFactory;
    [SerializeField] SettingsData _settingsData;

    private GameObject firePreview;
    private GameObject deamonPreview;
    private Quaternion cubeOrientation;

    private bool ableToSpawnAgain;

    private int state;

    [SerializeField] private Image debugToShowCoolDownOfSpell;

    [SerializeField] private Animator anim;

    void Start()
    {
        firePreview = Instantiate(_settingsData._cubePreviewPrefab);
        firePreview.GetComponent<MeshRenderer>().material.color = new Color(0,100,0,0.5f);
        firePreview.SetActive(false);

        deamonPreview = Instantiate(_settingsData.DeamonPreview);
        deamonPreview.GetComponent<MeshRenderer>().material.color = new Color(0, 100, 0, 0.5f);
        deamonPreview.SetActive(false);

        debugToShowCoolDownOfSpell.fillAmount = 0;
    }

    public void SwitchState(int states)
    {
        state = states;
    }

    void Update()
    {
        switch (state)
        {
            case 0: // invoke fire
                ShowCube(0);
                break;
            case 1: // invoke deamon
                ShowCube(1);
                break;
        }


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

                //if (hit.transform.tag == "Boy")
                //{
                //    StartCoroutine(SetBoyOnFire());
                //    hit.transform.GetComponent<Survive>().boyNeedState = BoyNeedState.RunToSurviveFire;
                //}
            }
        }

       
    }

    void ShowCube(int show)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "Floor")
            {
                if (show == 0)
                {
                    firePreview.SetActive(true);
                    firePreview.transform.position = hit.point;
                }
                else if (show == 1)
                {
                    deamonPreview.SetActive(true);
                    deamonPreview.transform.position = hit.point;
                }
                
            }
            else
            {
                firePreview.SetActive(false);
                deamonPreview.SetActive(false);
            }
        }

        //cubeObject.transform.Rotate(new Vector3(0,-1,0));

        cubeOrientation = firePreview.transform.rotation;
    }

    IEnumerator CreateWallThenWait(Vector3 hit, Quaternion cubeOrientation)
    {
        debugToShowCoolDownOfSpell.fillAmount = 1;

        ableToSpawnAgain = true;

        if (state == 0)
        {
            StartCoroutine(waitforFire(hit));
        }
        else if (state == 1)
        {
            StartCoroutine(waitforInvoke(hit));
        }

        firePreview.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 100, 0.1f);
        firePreview.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 100, 0.1f);

        for (float i = 0; i < 5; i++) // 5 sec
        {
            yield return new WaitForSeconds(1f);//1 each time
            debugToShowCoolDownOfSpell.fillAmount = 1 - (float)i / 5;
        }
        debugToShowCoolDownOfSpell.fillAmount = 0;

        ableToSpawnAgain = false;
        firePreview.GetComponent<MeshRenderer>().material.color = new Color(0, 100, 0, 0.5f);
        firePreview.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 100, 0.1f);
    }

    //IEnumerator SetBoyOnFire()
    //{
    //    ableToSpawnAgain = true;
    //    firePreview.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 100, 0.1f);
    //    firePreview.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 100, 0.1f);
    //    yield return new WaitForSeconds(4f);
    //    ableToSpawnAgain = false;
    //    firePreview.GetComponent<MeshRenderer>().material.color = new Color(0, 100, 0, 0.5f);
    //    firePreview.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 100, 0.1f);
    //}

    IEnumerator waitforFire(Vector3 hit)
    {
        anim.Play("hand anim fire");
        yield return new WaitForSeconds(0.2f);
        _wallFactory.CreateFire(hit, cubeOrientation);
    }

    IEnumerator waitforInvoke(Vector3 hit)
    {
        anim.Play("hand anim invoke");
        yield return new WaitForSeconds(0.3f);
         _wallFactory.CreateDeamon(hit, cubeOrientation);
        
    }
}
