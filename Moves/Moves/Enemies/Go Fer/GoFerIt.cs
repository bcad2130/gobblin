using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoFerIt : MoveButton
{
    protected const int COST    = 5;

    protected const string  STATCHANGE_TYPE     = "SPEED";
    protected const int     STATCHANGE_STACKS   = 7;

    protected override void SetUpMove() {

        action.TargetType = "Self";

        action.GutsCost = COST;

        action.AddStatChange(STATCHANGE_TYPE, STATCHANGE_STACKS);
        SetIsMove(true);
        SetIsSkill(true);
    }
}
