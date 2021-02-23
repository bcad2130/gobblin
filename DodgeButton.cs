using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeButton : MoveButton
{
    protected const int COST = 3;
    protected const int DAMAGE = 0;

    protected override void SetUpMove() {
        bm.CurrentAction.TargetType = "Self";

        bm.CurrentAction.ManaCost = COST;
        bm.CurrentAction.Damage = DAMAGE;

        bm.CurrentAction.AddStatusEffect("DODGE", 1);
    }
}
