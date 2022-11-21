using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gust : MoveButton
{
    protected const int COST    = 6;

    protected const string  STATCHANGE_TYPE     = "SPEED";
    protected const int     STATCHANGE_STACKS   = 2;

    protected override void SetUpMove() {

        action.TargetType = "AllAllies";

        action.GutsCost = COST;

        action.AddStatChange(STATCHANGE_TYPE, STATCHANGE_STACKS);
        SetIsMove(true);
        SetIsSkill(true);
    }
}
