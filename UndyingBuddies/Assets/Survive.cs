using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    void Start()
    {
        foodLoss = Random.Range(foodLossPerTicMin, foodLossPerTicMax);

        foodFeedingSpeed = 5 * foodLoss;

        StartCoroutine(waitToDie());

        MovingBoy.BoyState = BoyState.Idle;

        StartCoroutine(DebugCheckRestart());
    }

    void FindFood()
    {
        if (!dieded)
        {
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

    void Die()
    {
        dieded = true;

        Body.SetActive(false);
        Dead.SetActive(true);

        MovingBoy.Dead = true;

        GameObject.Find("GameController").GetComponent<BoyFactory>().Boys.Remove(this.gameObject);
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

    IEnumerator waitToDie()
    {
        DebugLookAroundBoys();

        if (!dieded)
        {
            food -= foodLoss;

            switch (boyNeedState)
            {
                case BoyNeedState.EnoughtOfEverything:
                    MovingBoy.BoyState = BoyState.Idle;
                    if (food < FoodBoySated)
                    {
                        FindFood();
                    }
                    break;

                case BoyNeedState.NeedFood:
                    //eat if available bush around him
                    //then when enought go back to idle and wondering around
                    NeirbyBush = MovingBoy.destinationToObjectif;

                    if (NeirbyBush == null || MovingBoy.destinationToObjectif == null)
                        boyNeedState = BoyNeedState.EnoughtOfEverything;
                    else
                    {
                        if (Vector3.Distance(MovingBoy.destinationToObjectif.transform.position, this.transform.position) <= 4f)
                        {
                            Eat();
                        }
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

    IEnumerator DebugCheckRestart()
    {
        MovingBoy.BoyState = BoyState.Idle;
        boyNeedState = BoyNeedState.EnoughtOfEverything;

        yield return new WaitForSeconds(10f);
        StartCoroutine(DebugCheckRestart());
    }
}
