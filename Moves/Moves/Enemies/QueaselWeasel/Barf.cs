using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barf : MoveButton
{
    protected const int COST = 4;

    protected override void SetUpMove() {
        action.TargetType = "MeleeEnemy";

        action.GutsCost = COST;
        
        action.AddStatusEffect("GROSS", 4);
        SetIsMove(true);
        SetIsSkill(true);
    }
}
