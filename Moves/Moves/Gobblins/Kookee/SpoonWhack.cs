using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpoonWhack : MoveButton
{
    protected const int COST = 3;
    protected const int DAMAGE = 3;

    protected override void SetUpMove()
    {
        action.TargetType = "MeleeEnemy";
        action.GutsCost = COST;
        
        action.StrengthDamage = bm.CurrentUnit.GetNetStrength() + DAMAGE;

        SetIsMove(true);
    }
}
