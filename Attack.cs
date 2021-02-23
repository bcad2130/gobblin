using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MoveButton
{
    protected const int COST = 0;

    protected override void SetUpMove() {
        bm.CurrentAction.TargetType = "OneEnemy";
        // manaCost = COST;

        bm.CurrentAction.ManaCost = COST;
        bm.CurrentAction.Damage = parentUnit.attack;

        // bm.CurrentAction.TargetedAction = true;
    }
}
