using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurntHoney : MoveButton
{
    protected const int COST = 10;

    protected override void SetUpMove()
    {
        action.TargetType = "MeleeEnemy";

        action.GutsCost = COST;

        action.TumDamage = 10;
        SetIsMove(true);
        SetIsSkill(true);
    }
}