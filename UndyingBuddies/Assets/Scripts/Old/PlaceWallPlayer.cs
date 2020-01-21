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

    private bool ableToSpawnAgain;
    private bool canPlayF;

    private int state;
    
    [SerializeField] private GameObject ImageSpell0;
    [SerializeField] private GameObject ImageSpell1;
    
    [SerializeField] private Animator animRightHand;
    [SerializeField] private Animator animLeftHand;

    private bool rollMouseWheel;

    public float spellValue = 1;
    public int UpgradeSpellValue = 100;
    public int upgradeMAx = 3;

    void Start()
    {
        firePreview = Instantiate(_settingsData._cubePreviewPrefab);
        firePreview.transform.localScale = new Vector3(spellValue, spellValue, spellValue);
        firePreview.SetActive(false);

        deamonPreview = Instantiate(_settingsData.DeamonPreview);
        //deamonPreview.transform.localScale = new Vector3(spellValue, spellValue, spellValue);
        deamonPreview.SetActive(false);
    }

    public void SwitchState(int states)
    {
        state = states;
    }

    public void UpdateSpell()
    {
        if (GameObject.Find("GameController").GetComponent<GameManager>().score >= UpgradeSpellValue && spellValue <= upgradeMAx)
        {
            spellValue += 0.5f;
            GameObject.Find("GameController").GetComponent<GameManager>().score -= UpgradeSpellValue;
        }
    }

    void Update()
    {
        firePreview.transform.localScale = new Vector3(spellValue, spellValue, spellValue);
        //deamonPreview.transform.localScale = new Vector3(spellValue, spellValue, spellValue);

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

        if (Input.GetAxis("Mouse ScrollWheel") > 0f && !rollMouseWheel) // forward
        {
            StartCoroutine(waitToMouseScroll(state, false));
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f && !rollMouseWheel) // backwards
        {
            StartCoroutine(waitToMouseScroll(state, true));
        }

        if (Input.GetMouseButtonUp(0) && canPlayF)
        {
            animRightHand.Play("hand anim whilecooldown F");
        }

        if (Input.GetMouseButtonUp(0) && !ableToSpawnAgain)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Floor")
                {
                    StartCoroutine(CreateWallThenWait(hit.point));
                }
            }
        }
    }

    void ShowCube(int show)
    {
        firePreview.SetActive(false);
        deamonPreview.SetActive(false);

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
    }

    IEnumerator CreateWallThenWait(Vector3 hit)
    {
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

        animRightHand.Play("hand anim cooldownn recover");

        ableToSpawnAgain = false;
    }
    
    IEnumerator waitforFire(Vector3 hit)
    {
        animRightHand.Play("hand anim fire");
        yield return new WaitForSeconds(0.2f);
        _wallFactory.CreateFire(hit, spellValue);
    }

    IEnumerator waitforInvoke(Vector3 hit)
    {
        animRightHand.Play("hand anim invoke");
        yield return new WaitForSeconds(0.3f);
         _wallFactory.CreateDeamon(hit, spellValue);
    }

    IEnumerator waitToMouseScroll(int localState, bool operation)
    {
        rollMouseWheel = true;

        int tempState;

        tempState = localState;

        if (operation == false)
            localState++;
        if (operation == true)
            localState--;

        if (localState > 1)
            localState = 1;

        if (localState < 0)
            localState = 0;

        if(localState != tempState)
            animLeftHand.Play("Hand anim lefthand switchSpell");

        yield return new WaitForSeconds(0.1f);
        
        SwitchState(localState);

        yield return new WaitForSeconds(0.1f);

        rollMouseWheel = false;
    }
}
