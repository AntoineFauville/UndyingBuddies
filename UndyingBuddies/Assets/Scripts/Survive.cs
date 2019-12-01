using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Survive : MonoBehaviour
{
    public MovingBoy MovingBoy;

    public GameObject Body;
    public GameObject Dead;

    public bool dieded;

    public float MaxFood = 100;
    public float food;
    public float foodLossPerTicMin = 0.5f;
    public float foodLossPerTicMax = 3;
    private float foodLoss;
    public float FoodBoySated = 60;
    public float foodEatedEnoughState = 90;
    private float foodFeedingSpeed;

    public BoyNeedState boyNeedState;
    public bool bushAround;
    public GameObject NeirbyBush;

    public List<GameObject> BoyList = new List<GameObject>();

    [SerializeField] private GameObject Fire;
    [SerializeField] private bool fireActivation;
    public GameObject NeirbyHouse;
    public GameObject NeirbyTree;
    public int AmountOfWoodCarriedMax = 5;

    public int WoodAmount;
    [SerializeField] private GameObject _woodLogBackPack;
    
    void Start()
    {
        foodLoss = Random.Range(foodLossPerTicMin, foodLossPerTicMax);

        foodFeedingSpeed = 5 * foodLoss;

        StartCoroutine(waitToDie());

        MovingBoy.BoyState = BoyState.Idle;

        Fire.SetActive(false);

        //SetupTreeAndHouse();

        StartCoroutine(DebugCheckRestart());
    }

    void SetupTreeAndHouse()
    {
        NeirbyHouse = GameObject.Find("GameController").GetComponent<Usables>().House[0];
        NeirbyTree = GameObject.Find("GameController").GetComponent<Usables>().Tree[0];
    }

    void FindFood()
    {
        if (!dieded)
        {
            MovingBoy.anim.Play("ArmsFeed");
            MovingBoy.BoyState = BoyState.FindingFood;
            boyNeedState = BoyNeedState.NeedFood;
        }
    }

    void Eat()
    {
        if (!dieded)
        {
            if (food < foodEatedEnoughState)
            {
                food += foodFeedingSpeed;
                NeirbyBush.GetComponent<BushLife>().ReduceLife(foodFeedingSpeed);
                MovingBoy.anim.Play("ArmsEating");
            }
            else
            {
                boyNeedState = BoyNeedState.EnoughtOfEverything;
                MovingBoy.BoyState = BoyState.Idle;
            }

            if (food >= MaxFood)
            {
                food = MaxFood;
            }
        }
    }

    void VisualWood()
    {
        if (WoodAmount > 0)
        {
            _woodLogBackPack.SetActive(true);
        }
        else
        {
            _woodLogBackPack.SetActive(false);
        }
    }

    void GetWood()
    {
        if (!dieded)
        {
            if (WoodAmount < AmountOfWoodCarriedMax)
            {
                WoodAmount++;
            }
            else
            {
                boyNeedState = BoyNeedState.EnoughtOfEverything;
                MovingBoy.BoyState = BoyState.Idle;
            }

            if (WoodAmount >= AmountOfWoodCarriedMax)
            {
                WoodAmount = AmountOfWoodCarriedMax;
            }
        }
    }

    void GiveWood()
    {
        if (!dieded)
        {
            if (WoodAmount > 0)
            {
                WoodAmount--;
                NeirbyHouse.GetComponent<House>().AddWoodToBuilding(1);
            }
            else
            {
                boyNeedState = BoyNeedState.EnoughtOfEverything;
                MovingBoy.BoyState = BoyState.Idle;
            }

            if (WoodAmount <= 0)
            {
                WoodAmount = 0;
            }
        }
    }

    void Die()
    {
        dieded = true;

        Body.SetActive(false);
        Dead.SetActive(true);

        MovingBoy.Dead = true;

        WoodAmount = 0;

        GameObject.Find("GameController").GetComponent<BoyFactory>().TotalOfTheBoys.Remove(this.gameObject);
        GameObject.Find("GameController").GetComponent<GameManager>().score++;

        MovingBoy.enabled = false;
        this.enabled = false;

        this.GetComponent<CapsuleCollider>().isTrigger = true;
        this.GetComponent<NavMeshAgent>().enabled = false;
        this.GetComponent<Rigidbody>().isKinematic = false;
    }

    public bool LookAroundForOtherBoys()
    {
        bool aBoyAround = false;

        foreach (var boy in GameObject.FindGameObjectsWithTag("Boy"))
        {
            if ((this.transform.position - boy.transform.position).magnitude < 2)
            {
                if (boy != this.gameObject)
                {
                    if (boyNeedState == BoyNeedState.EnoughtOfEverything && boy.GetComponent<Survive>().boyNeedState == BoyNeedState.EnoughtOfEverything)
                    {
                        aBoyAround = true;
                        return aBoyAround;
                    }
                }
            }
        }

        return aBoyAround;
    }

    void DebugLookAroundBoys()
    {
        BoyList.Clear();

        foreach (var boy in GameObject.FindGameObjectsWithTag("Boy"))
        {
            if (boy != this.gameObject)
            {
                BoyList.Add(boy);
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "FireZone")
        {
            //print("in fire zone");
            boyNeedState = BoyNeedState.RunToSurviveFire;
        }
    }

    IEnumerator waitToDie()
    {
        VisualWood();

        yield return new WaitForSeconds(0.1f);

        DebugLookAroundBoys();

        if (!dieded)
        {
            food -= foodLoss;

            switch (boyNeedState)
            {
                case BoyNeedState.EnoughtOfEverything:
                    
                    if (food < FoodBoySated && !fireActivation)
                    {
                        FindFood();
                    }

                    //build a house
                    //find nearest wood, if not null
                    //get wood
                    //when enough wood,
                    //go to house and deposit
                    //loop if 

                    if (GameObject.Find("GameController").GetComponent<Usables>().House.Count == 0)
                    {
                        MovingBoy.BoyState = BoyState.Idle;
                        yield return 0;
                        break;
                    }
                    else
                    {
                        if (WoodAmount < AmountOfWoodCarriedMax)
                        {
                            MovingBoy.destinationToObjectif = NeirbyTree;
                            MovingBoy.BoyState = BoyState.WalkingToObjectif;
                            MovingBoy.typeOfUsableImLookingFor = UsableType.Tree;

                            if (NeirbyTree == null || MovingBoy.destinationToObjectif == null)
                                boyNeedState = BoyNeedState.EnoughtOfEverything;
                            else
                            {
                                if (Vector3.Distance(MovingBoy.destinationToObjectif.transform.position, this.transform.position) <= 5f)
                                {
                                    GetWood();
                                }
                            }

                            break;
                        }

                        if (WoodAmount >= AmountOfWoodCarriedMax)
                        {
                            MovingBoy.destinationToObjectif = NeirbyHouse;
                            MovingBoy.BoyState = BoyState.WalkingToObjectif;
                            MovingBoy.typeOfUsableImLookingFor = UsableType.House;

                            if (NeirbyHouse == null || MovingBoy.destinationToObjectif == null)
                                boyNeedState = BoyNeedState.EnoughtOfEverything;
                            else
                            {
                                if (Vector3.Distance(MovingBoy.destinationToObjectif.transform.position, this.transform.position) <= 5f)
                                {
                                    GiveWood();
                                }
                            }

                            break;
                        }
                    }

                    break;

                case BoyNeedState.NeedFood:
                    //eat if available bush around him
                    //then when enought go back to idle and wondering around
                    MovingBoy.typeOfUsableImLookingFor = UsableType.Bush;
                    MovingBoy.destinationToObjectif = NeirbyBush;
                    MovingBoy.BoyState = BoyState.WalkingToObjectif;
                    

                    if (NeirbyBush == null || MovingBoy.destinationToObjectif == null)
                        boyNeedState = BoyNeedState.EnoughtOfEverything;
                    else
                    {
                        if (Vector3.Distance(MovingBoy.destinationToObjectif.transform.position, this.transform.position) <= 2f)
                        {
                            Eat();
                        }
                    }
                    break;

                case BoyNeedState.RunToSurviveFire:
                    MovingBoy.BoyState = BoyState.RunAway;
                    food = 20;
                    if (!fireActivation)
                    {
                        fireActivation = true;
                        
                        StartCoroutine(SetOnFireDispatch());
                    }
                    break;
                    
            }
        }

        if (food <= 0)
        {
            Die();
            food = 0;
        }

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(waitToDie());
    }

    void BurnAway()
    {

    }

    IEnumerator DebugCheckRestart()
    {
        MovingBoy.BoyState = BoyState.Idle;
        boyNeedState = BoyNeedState.EnoughtOfEverything;

        yield return new WaitForSeconds(10f);

        //stop fire too
        Fire.SetActive(false);

        MovingBoy.BoyState = BoyState.Idle;
        boyNeedState = BoyNeedState.EnoughtOfEverything;

        fireActivation = false;

        StartCoroutine(DebugCheckRestart());
    }

    IEnumerator SetOnFireDispatch()
    {
        Fire.SetActive(true);
        yield return new WaitForSeconds(5f);
        Fire.SetActive(false);

        MovingBoy.BoyState = BoyState.Idle;
        boyNeedState = BoyNeedState.EnoughtOfEverything;

        fireActivation = false;
    }
}
