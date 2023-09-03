using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpoonWhack : MoveButton
{
    protected const int COST = 5;
    protected const int DAMAGE = 4;

    protected override void SetUpMove()
    {
        action.TargetType = "MeleeEnemy";
        action.GutsCost = COST;
        
        action.StrengthDamage = bm.getCurrentUnit().GetNetStrength() + DAMAGE;

        SetIsMove(true);
    }
}
