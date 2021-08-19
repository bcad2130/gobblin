using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : MoveButton
{
    protected override void SetUpMove() {
        action.TargetType = "MeleeEnemy";

        action.Damage = bm.CurrentUnit.attack + bm.CurrentUnit.GetStatChangeStacks("STRENGTH");
        action.Speed = bm.CurrentUnit.speed  + bm.CurrentUnit.GetStatChangeStacks("SPEED");

        action.isBasicAttack = true;
    }
}
