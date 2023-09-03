using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRangedAttack : MoveButton
{
    protected override void SetUpMove() {
        action.TargetType = "Enemy";

        action.StrengthDamage   = bm.getCurrentUnit().GetNetStrength();
        action.Speed            = bm.getCurrentUnit().GetNetSpeed();

        action.isBasicAttack = true;
        SetIsMove(true);
    }
}
