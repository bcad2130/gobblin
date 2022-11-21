using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Speech
{
    public string name;
    public Sprite icon;

    [TextArea(3, 10)]
    public string sentence;
}
