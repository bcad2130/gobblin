using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigSniff : MoveButton
{
    protected const int COST    = 5;

    protected const string  STATCHANGE_TYPE     = "NOSE";
    protected const int     STATCHANGE_STACKS   = 5;

    protected override void SetUpMove() {
        action.TargetType = "Self";

        action.GutsCost = COST;

        action.AddStatChange(STATCHANGE_TYPE, STATCHANGE_STACKS);
        SetIsMove(true);
        SetIsSkill(true);
    }
}
