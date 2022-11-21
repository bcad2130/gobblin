using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CobFrappeItem : Item
{
    public override string Name     { get { return "Chunky Cob Frappe" + GetQualityString(); } }

    public override bool Equipment  { get { return false; } }

    public override bool Meal       { get { return true; } }

    public override bool Treat      { get { return false; } }

    public override bool Trick      { get { return true; } }

    public override bool Drink      { get { return true; } }

    public override bool Food       { get { return false; } }



    public override Action ItemAction()
    {
        Action action = new Action();
        
        action.TumDamage = 15;
        action.AddStatChange("TUM", -3);
        // action.Famish = 5;

        // Debug.Log('h');
        return action;
    }
    // do i need a constructor?
}