using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigIn : MoveButton
{
    protected const int COST = 1;

    protected override void SetUpMove() {
        action.TargetType = "MeleeEnemy";
        action.isBasicAttack = true;

        action.GutsCost = COST;
        
        action.AddStatusEffect("FURY", 1);
        SetIsMove(true);
    }
}
