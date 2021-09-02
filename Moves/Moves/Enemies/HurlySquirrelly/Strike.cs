using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strike : MoveButton
{
    protected override void SetUpMove() {
        action.TargetType = "Enemy";

        action.StrengthDamage   = bm.CurrentUnit.GetNetStrength();
        action.Speed            = bm.CurrentUnit.GetNetSpeed();

        action.isBasicAttack = true;
    }
}
