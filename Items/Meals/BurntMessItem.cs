using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurntMessItem : Item
{
    public override string Name { get { return "Burnt Mess"; } }

    public override bool Equipment { get { return false; } }

    public override bool Meal { get { return true; } }

    public override bool Treat { get { return false; } }

    public override bool Trick { get { return true; } }

    public override bool Drink { get { return true; } }

    public override bool Food { get { return true; } }

    public override Action ItemAction()
    {
        Action action = new Action();
        
        action.Damage = 5;
        action.Sate = 0;

        return action;
    }
}
