using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurntCornItem  : Item
{
    public override string Name     { get { return "Burnt Corn" + GetQualityString(); } }

    public override bool Equipment  { get { return false; } }

    public override bool Meal       { get { return true; } }

    public override bool Treat      { get { return true; } }

    public override bool Trick      { get { return false; } }

    public override bool Drink      { get { return false; } }

    public override bool Food       { get { return true; } }

    public override Action ItemAction()
    {
        Action action = new Action();

        // action.TumDamage = 3;
        action.Heal = 5;
        action.Sate = 5;
        
        return action;
    }
}
