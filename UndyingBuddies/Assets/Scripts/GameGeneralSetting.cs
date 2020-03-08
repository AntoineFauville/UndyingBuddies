using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameGeneralSetting : MonoBehaviour
{
    int _value;

    void Start()
    {
        _value = 1;
    }

    // Start is called before the first frame update
    public void ChangeTimeScale(int value)
    {
        _value = value;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_value == 0)
        {
            Time.timeScale = 0.1f;
        }
        if(_value == 1)
        {
            Time.timeScale = 1;
        }
        if (_value == 2)
        {
            Time.timeScale = 3;
        }
    }
}
