using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KO : MoveButton
{
    protected const int COST = 4;
    protected const int DAMAGE = 0;

    protected override void SetUpMove() {

        action.TargetType = "MeleeEnemy";

        action.GutsCost = COST;
        action.StrengthDamage = bm.CurrentUnit.GetNetStrength() + DAMAGE;

        action.AddStatusEffect("STUN", 1);
        SetIsMove(true);
    }
}
