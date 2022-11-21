using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatGarbage : MoveButton
{
    protected const int COST = 6;

    protected override void SetUpMove() {
        action.TargetType = "Self";

        action.GutsCost = COST;
        action.Heal = 15;
        
        action.AddStatusEffect("GROSS", 2);
        SetIsMove(true);
        SetIsSkill(true);
    }
}
