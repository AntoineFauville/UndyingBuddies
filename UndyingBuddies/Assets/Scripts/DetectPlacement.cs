using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlacement : MonoBehaviour
{
    public bool Detected;

    void OnTriggerStay(Collider coll)
    {
        if (coll.transform.tag == "boundingBox")
        {
            Detected = true;
        }
        else
        {
            Detected = false;
        }
    }

    void OnTriggerExit()
    {
        Detected = false;
    }
}
