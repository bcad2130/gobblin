using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopCornItem : Item
{
    public override string Name     { get { return "Popcorn"; } }

    public override bool Equipment  { get { return false; } }

    public override bool Meal       { get { return true; } }

    public override bool Treat      { get { return false; } }

    public override bool Trick      { get { return true; } }

    public override bool Drink      { get { return false; } }

    public override bool Food       { get { return true; } }

    public override Action ItemAction()
    {
        Action action = new Action();
        
        action.Damage = 25;
        action.Sate = 3;

        // Debug.Log('h');
        return action;
    }

}
