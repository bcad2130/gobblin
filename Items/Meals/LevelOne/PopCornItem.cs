using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopCornItem : Item
{
    public override string Name     { get { return "Popcorn" + GetQualityString(); } }

    public override bool Equipment  { get { return false; } }

    public override bool Meal       { get { return true; } }

    public override bool Treat      { get { return false; } }

    public override bool Trick      { get { return true; } }

    public override bool Drink      { get { return false; } }

    public override bool Food       { get { return true; } }

    public override Action ItemAction()
    {
        Action action = new Action();
        
        action.TumDamage = 25;
        action.Sate = 3;

        return action;
    }
}
