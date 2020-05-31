using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceSpell : MonoBehaviour
{
    public List<SpellArchetype> Spells = new List<SpellArchetype>();

    public SpellArchetype activeSpell;

    public GameObject SpellHandler;

    public bool activateShowOfSpell;
    public bool ableToSpawnAgain;

    public List<GameObject> debugGameObjects = new List<GameObject>();

    GameObject Preview;

    private ResourceManager resourceManager;
    private Grab grab;
    private spellCanvasView currentSpellCanvas;

    [SerializeField] private AudioSource AudioSource;
    [SerializeField] private AudioClip audioClipFire, audioClipHorror, audioClipPoison;

    void Start()
    {
        resourceManager = GameObject.Find("Main Camera").GetComponent<ResourceManager>();
        grab = GameObject.Find("Main Camera").GetComponent<Grab>();
    }

    public void SetupSpell(SpellArchetype spellArchetype)
    {
        Spells.Add(spellArchetype);
        spellCanvasView spellCanvasView;
        spellCanvasView = Instantiate(spellArchetype.UIToSpawn, SpellHandler.transform);
        spellCanvasView.spellArchetype = spellArchetype;
        currentSpellCanvas = spellCanvasView;
    }

    public void SetSpellToActive(SpellArchetype spellArchetype)
    {
        activeSpell = spellArchetype;
    }

    void Update()
    {
        if (activeSpell != null)
        {
            if (resourceManager.amountOfEnergy >= activeSpell.SpellCostInEnergy)
            {
                if (activateShowOfSpell && Input.GetMouseButtonUp(0) && !ableToSpawnAgain)
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.transform.tag == "Floor")
                        {
                            StartCoroutine(CreateSpellThanWait(hit.point));
                        }
                    }
                }

                if (activateShowOfSpell && !ableToSpawnAgain)
                {
                    ShowSpellDebug(true);
                    grab.notUsingSpell = true;
                }
                else//if i still have enough money to click again this doesn't show the spell
                {
                    ShowSpellDebug(false);
                    grab.notUsingSpell = false;
                }
            }
            else //if i've no money then don't show debug
            {
                ShowSpellDebug(false);
                grab.notUsingSpell = false;
            }
        }

        if (Input.GetMouseButtonUp(1) && activeSpell != null)
        {
            for (int i = 0; i < GameObject.Find("Main Camera").GetComponent<SpellManager>().spellCanvases.Count; i++)
            {
                GameObject.Find("Main Camera").GetComponent<SpellManager>().spellCanvases[i].isShowing = false;
            }

            ShowSpellDebug(false);
            grab.notUsingSpell = false;
            ableToSpawnAgain = false;
            activateShowOfSpell = false;
            activeSpell = null;
        }
    }

    void ShowSpellDebug(bool show)
    {
        if (!debugGameObjects.Contains(GameObject.Find(activeSpell.PlacementShowDebug.name + "(Clone)")))
        {
             Preview = Instantiate(activeSpell.PlacementShowDebug);
            debugGameObjects.Add(Preview);
        }
        else
        {
           Preview = GameObject.Find(activeSpell.PlacementShowDebug.name + "(Clone)");
        }

        if (show)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Floor")
                {
                    Preview.transform.GetChild(0).gameObject.SetActive(true);
                    Preview.transform.position = hit.point;
                }
                else
                {
                    Preview.transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }
        else
        {
            Preview.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public Spell CreateSpell(Vector3 hit)
    {
        Spell newPrefab = Instantiate(activeSpell.spellPrefab, hit, new Quaternion());

        if (AudioSource != null)
        {
            if (activeSpell.aiStatusDamageType == AiStatus.Physical)
            {
                AudioSource.PlayOneShot(audioClipFire);
            }
            if (activeSpell.aiStatusDamageType == AiStatus.MentalHealth)
            {
                AudioSource.PlayOneShot(audioClipHorror);
            }
            if (activeSpell.aiStatusDamageType == AiStatus.IntestineStatus)
            {
                AudioSource.PlayOneShot(audioClipPoison);
            }
        }
        if (activeSpell.spellLeftAfterSpawned != null)
        {
            GameObject stainsBlack = Instantiate(activeSpell.spellLeftAfterSpawned, hit, new Quaternion());
        }

        return newPrefab;
    }

    IEnumerator CreateSpellThanWait(Vector3 hit)
    {
        ableToSpawnAgain = true;

        StartCoroutine(waitforFire(hit));

        GameObject.Find("RightHand").GetComponent<Animator>().Play("hand anim fire");

        yield return new WaitForSeconds(1f);

        ableToSpawnAgain = false;
    }

    IEnumerator waitforFire(Vector3 hit)
    {
        yield return new WaitForSeconds(0.2f);
        CreateSpell(hit);
        resourceManager.amountOfEnergy -= activeSpell.SpellCostInEnergy;
    }
}
