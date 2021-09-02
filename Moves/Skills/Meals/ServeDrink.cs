using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServeDrink : MoveButton
{
    // protected const int COST = 4;
    protected const int DAMAGE = 0;

    protected override void SetUpMove()
    {
        action.TargetType = "Enemy";

        action.SetSkillType("Serve");

        // action.GutsCost = COST;

        action.SetResourceCost(GetResourceCost());
    }
}
