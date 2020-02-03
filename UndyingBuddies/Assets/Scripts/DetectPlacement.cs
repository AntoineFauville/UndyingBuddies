using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlacement : MonoBehaviour
{
    public bool Detected;

    void OnTriggerStay(Collider coll)
    {
        //Debug.Log(this.gameObject.name + "triggered" + "  " + coll.name);

        if (coll.transform.tag == "boundingBox" || coll.transform.tag == "tree" 
            || coll.transform.tag == "flag" || coll.transform.tag == "wood" 
            || coll.transform.tag == "food" || coll.transform.tag == "relic" 
            || coll.transform.tag == "priest" || coll.transform.tag == "demon" 
            || coll.transform.tag == "foodStock" || coll.transform.tag == "woodStock" 
            || coll.transform.tag == "terrainWarFlagOnly" || coll.transform.tag == "Untagged")
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
