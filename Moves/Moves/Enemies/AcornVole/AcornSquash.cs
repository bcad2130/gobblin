using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcornSquash : MoveButton
{
    protected const int COST    = 5;

    protected override void SetUpMove() {
        action.TargetType = "MeleeEnemy";
        action.GutsCost = COST;

        action.StrengthDamage   = bm.getCurrentUnit().GetNetStrength() + 5;
        SetIsMove(true);
        SetIsSkill(true);
    }
}
