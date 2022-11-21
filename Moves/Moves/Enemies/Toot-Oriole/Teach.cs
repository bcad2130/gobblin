using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teach : MoveButton
{
    protected const int COST    = 4;

    protected const string  STATCHANGE_TYPE     = "STRENGTH";
    protected const int     STATCHANGE_STACKS   = 5;

    protected override void SetUpMove() {

        action.TargetType = "Ally";

        action.GutsCost = COST;

        action.AddStatChange(STATCHANGE_TYPE, STATCHANGE_STACKS);
        SetIsMove(true);
        SetIsSkill(true);
    }
}
