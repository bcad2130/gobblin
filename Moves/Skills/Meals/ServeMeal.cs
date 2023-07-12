using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServeMeal : MoveButton
{
    protected const int COST = 3;
    protected const int DAMAGE = 0;

    protected override void SetUpMove()
    {
        action.TargetType = "MeleeEnemy";

        action.SetSkillType("Serve");

        // action.GutsCost = COST;

        action.SetResourceCost(GetResourceCost());
        SetIsMove(true);
    }
}
