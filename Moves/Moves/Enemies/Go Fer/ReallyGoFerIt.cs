using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReallyGoFerIt : MoveButton
{
    protected const int COST    = 10;

    protected const string  STATCHANGE_TYPE     = "SPEED";
    protected const int     STATCHANGE_STACKS   = 13;

    protected override void SetUpMove() {

        action.TargetType = "Self";

        action.GutsCost = COST;

        action.AddStatChange(STATCHANGE_TYPE, STATCHANGE_STACKS);
        SetIsMove(true);
        SetIsSkill(true);
    }
}
