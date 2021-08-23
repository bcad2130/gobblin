using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornCobItem : Item
{
    public override string Name     { get { return "Corn Cob" + GetQualityString(); } }

    public override bool Equipment  { get { return false; } }

    public override bool Meal       { get { return true; } }

    public override bool Treat      { get { return true; } }

    public override bool Trick      { get { return false; } }

    public override bool Drink      { get { return false; } }

    public override bool Food       { get { return true; } }



    public override Action ItemAction()
    {
        Action action = new Action();
        
        action.Heal = 10;
        action.Sate = 5;

        // Debug.Log('h');
        return action;
    }
    // do i need a constructor?
}
