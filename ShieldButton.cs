using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldButton : MoveButton
{
    protected const int COST = 4;
    protected const int DAMAGE = 0;

    protected override void SetUpMove() {
        bm.CurrentAction.TargetType = "Self";
        // manaCost = COST;

        bm.CurrentAction.ManaCost = COST;
        bm.CurrentAction.Damage = DAMAGE;
        bm.CurrentAction.AddStatusEffect("SHIELD", 5);

        // bm.CurrentAction.TargetedAction = true;

    }
}
