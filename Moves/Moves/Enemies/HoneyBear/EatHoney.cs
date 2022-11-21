using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatHoney : MoveButton
{
    protected const int COST = 5;

    protected override void SetUpMove()
    {
        action.TargetType = "MeleeEnemy";
        
        action.GutsCost = COST;

        action.Heal = 5;
        action.Sate = 10;
        // add one GLOW effect (+1 all stats)

        action.AddStatusEffect("GLOW", 1);
        SetIsMove(true);
        SetIsSkill(true);
    }
}
