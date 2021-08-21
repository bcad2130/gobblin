using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KO : MoveButton
{
    protected const int COST = 4;
    protected const int DAMAGE = 0;

    protected override void SetUpMove() {

        action.TargetType = "MeleeEnemy";

        action.GutsCost = COST;
        action.Damage = bm.CurrentUnit.attack + bm.CurrentUnit.GetStatChangeStacks("STRENGTH") +  DAMAGE;

        action.AddStatusEffect("STUN", 1);
    }
}
