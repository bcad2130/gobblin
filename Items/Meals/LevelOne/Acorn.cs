using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acorn : Item
{
    public override string Name     { get { return "Acorn" + GetQualityString(); } }

    public override bool Equipment  { get { return false; } }

    public override bool Meal       { get { return true; } }

    public override bool Treat      { get { return true; } }

    public override bool Trick      { get { return false; } }

    public override bool Drink      { get { return true; } }

    public override bool Food       { get { return false; } }

    public override Action ItemAction()
    {
        Action action = new Action();
        
        action.Heal = 20;
        action.Sate = 20;
        
        return action;
    }
}
