using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : MoveButton
{
    protected override void SetUpMove() {
        action.TargetType = "MeleeEnemy";

        action.StrengthDamage   = bm.getCurrentUnit().GetNetStrength();
        action.Speed            = bm.getCurrentUnit().GetNetSpeed();

        action.isBasicAttack = true;
        SetIsMove(true);
    }
}
