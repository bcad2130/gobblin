using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strike : MoveButton
{
    protected override void SetUpMove() {
        action.TargetType = "Enemy";

        action.StrengthDamage   = bm.getCurrentUnit().GetNetStrength();
        action.Speed            = bm.getCurrentUnit().GetNetSpeed();

        action.isBasicAttack = true;
        SetIsMove(true);
        SetIsSkill(true);
    }
}
