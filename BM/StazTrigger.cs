using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StazTrigger : MonoBehaviour
{
    // public string LevelName;

    public void Trigger ()
    {
        // Debug.Log("I'm attached to " + gameObject.name);
        // Debug.Log("I'm attached to my parent " + this.transform.parent.gameObject.name);
        // Debug.Log("I'm attached to my grandparent " + this.transform.parent.parent.gameObject.name);
        // Debug.Log("I'm attached to my great-grandparent " + this.transform.parent.parent.parent.gameObject.name);

        // UnitStats unit = gameObject.GetComponent<UnitStats>();
        UnitStats unit = this.transform.parent.parent.parent.gameObject.GetComponent<UnitStats>();

        // Debug.Log(unit.name);

        FindObjectOfType<StazManager>().ShowStaz(unit);
    }
}

