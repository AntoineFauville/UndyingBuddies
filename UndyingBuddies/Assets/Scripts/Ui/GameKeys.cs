using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameKeys : MonoBehaviour
{
    public int state = 0;

    [SerializeField] private GameObject[] ObjectToActivate;
    [SerializeField] private GameObject Menu;

    // Start is called before the first frame update
    void Start()
    {
        state = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject.Find("Main Camera").transform.position = new Vector3(GameObject.Find("CityRef").transform.position.x, 20, GameObject.Find("CityRef").transform.position.z - 17);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("ActivateMenu");

            if (state == 1)
            {
                for (int i = 0; i < ObjectToActivate.Length; i++)
                {
                    ObjectToActivate[i].SetActive(false);
                }

                state = 0;
            }
            if (state == 0)
            {
                if (Menu.activeSelf == true)
                {
                    Menu.SetActive(false);
                }
                else if (Menu.activeSelf == false)
                {
                    Menu.SetActive(true);
                }
            }
        }
    }

    public void changeState(int statechanger)
    {
        state = statechanger;
    }

    public void exitApplication()
    {
        Application.CancelQuit();
    }
}
