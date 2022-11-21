using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public GameObject textObject;
    public int countdown;

    // void DamageText(int damage, string color)
    // {

    // }

    // Update is called once per frame
    void Update()
    {
        if (countdown > 0)
        {
            if(countdown % 100 == 0) {
                textObject.transform.position = new Vector3(textObject.transform.position.x, textObject.transform.position.y+3.0f, textObject.transform.position.z);
            }
            countdown--;
        } else {
            Destroy(textObject);
        }
    }
}
