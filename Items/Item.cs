using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [System.Serializable]
public abstract class Item
{
    public abstract string Name     { get; }

    public abstract bool Equipment  { get; }

    public abstract bool Meal       { get; }

    public abstract bool Treat      { get; }

    public abstract bool Trick      { get; }

    public abstract bool Drink      { get; }

    public abstract bool Food       { get; }

    private int quality = 0;

    // private string nameQuality = "";


    // public string   Description     { get; set; }

    // reminder: use virtual if you don't always need a subclassed method, use abstract if you always need one
    // for equipment (or ingredients), we're not gonna need a subclassed method for the non-existent action
    public virtual Action ItemAction() {
        Debug.Log("should you have overridden this?");
        return null;
    }

    // public string GetNameQuality()
    // {
    //     return nameQuality;
    // }

    // public void SetNameQuality(string nameToSet)
    // {
    //     nameQuality = nameToSet;
    // }

    public int GetQuality()
    {
        return quality;
    }

    public void SetQuality(int qualityToSet)
    {
        quality = qualityToSet;
    }

    protected string GetQualityString()
    {
        if (GetQuality() != 0) {
            return " +" + GetQuality().ToString();
        } else {
            return "";
        }
    }
}
