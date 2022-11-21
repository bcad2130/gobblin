using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tootaloo : MoveButton
{
    protected const int COST = 8;
    protected const int DAMAGE = 0;

    protected const string  STATCHANGE_TYPE     = "SPEED";
    protected const int     STATCHANGE_STACKS   = 4;

    protected override void SetUpMove() {

        action.TargetType = "MeleeEnemy";

        action.GutsCost = COST;

        action.AddStatusEffect("STUN", 2);

        action.AddStatChange(STATCHANGE_TYPE, STATCHANGE_STACKS);
        SetIsMove(true);
        SetIsSkill(true);
    }
}
