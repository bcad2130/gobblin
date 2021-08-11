using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttackButton : MoveButton
{
    protected override void SetUpMove() {
        action.TargetType = "MeleeEnemy";

        action.Damage = bm.CurrentUnit.attack;
    }
}
