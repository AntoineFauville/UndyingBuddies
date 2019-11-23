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
    private bool canPlayF;

    private int state;

    private bool _stateOfHoldingMouse;
    private bool _launchOnceCoroutine;
    [SerializeField] private Canvas HideShowPanel;
    private bool _holding;

    [SerializeField] private GameObject ImageSpell0;
    [SerializeField] private GameObject ImageSpell1;

    //[SerializeField] private Image debugToShowCoolDownOfSpell;

    [SerializeField] private Animator anim;
    [SerializeField] private Animator animCamera;

    void Start()
    {
        animCamera.Play("camMovementDEZoom");

        HideShowPanel.enabled = false;

        firePreview = Instantiate(_settingsData._cubePreviewPrefab);
        firePreview.SetActive(false);

        deamonPreview = Instantiate(_settingsData.DeamonPreview);
        deamonPreview.SetActive(false);

        //debugToShowCoolDownOfSpell.fillAmount = 0;
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
                ImageSpell0.SetActive(true);
                ImageSpell1.SetActive(false);
                break;
            case 1: // invoke deamon
                ShowCube(1);
                ImageSpell0.SetActive(false);
                ImageSpell1.SetActive(true);
                break;
        }

        if (Input.GetMouseButtonUp(0) && canPlayF && !_stateOfHoldingMouse)
        {
            anim.Play("hand anim whilecooldown F");
        }

        if (Input.GetMouseButtonUp(0) && !ableToSpawnAgain && !_stateOfHoldingMouse)
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

        if (Input.GetMouseButtonUp(0))
        {
            StartCoroutine(removePanel());
        }

        if (Input.GetMouseButtonDown(0))
        {
            _holding = true;
        }

        if (Input.GetMouseButton(0))
        {
            if (!_launchOnceCoroutine)
            {
                _launchOnceCoroutine = true;
                StartCoroutine(holdingLongEnough());
            }
        }

        if (_stateOfHoldingMouse)
        {
            HideShowPanel.enabled = true;
        }
        else if (!_stateOfHoldingMouse)
        {
            HideShowPanel.enabled = false;
        }

    }

    void ShowCube(int show)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "Floor" && !_stateOfHoldingMouse)
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
        //debugToShowCoolDownOfSpell.fillAmount = 1;

        ableToSpawnAgain = true;

        if (state == 0)
        {
            StartCoroutine(waitforFire(hit));
        }
        else if (state == 1)
        {
            StartCoroutine(waitforInvoke(hit));
        }
        
        yield return new WaitForSeconds(1f);

        canPlayF = true;

        yield return new WaitForSeconds(1f);

        canPlayF = false;

        yield return new WaitForSeconds(1f);

        anim.Play("hand anim cooldownn recover");

        ableToSpawnAgain = false;
       
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

    IEnumerator holdingLongEnough()
    {
        yield return new WaitForSeconds(0.35f);
        if (_holding)
        {
            _stateOfHoldingMouse = true;
            animCamera.Play("camMovementZoom");
            yield return new WaitForSeconds(0.15f);
            Time.timeScale = 0.3f;
        }
    }

    IEnumerator removePanel()
    {
        Time.timeScale = 1f;
        animCamera.Play("camMovementDEZoom");

        yield return new WaitForSeconds(0.05f);

        _holding = false;
        _launchOnceCoroutine = false;
        _stateOfHoldingMouse = false;
    }
}
