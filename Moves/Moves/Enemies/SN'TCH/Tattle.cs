using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tattle : MoveButton
{
    protected const int COST = 4;

    protected const string  STATCHANGE_TYPE     = "DEFENSE";
    protected const int     STATCHANGE_STACKS   = 3;

    protected override void SetUpMove() {
        action.TargetType = "Ally";

        action.GutsCost = COST;

        action.AddStatChange(STATCHANGE_TYPE, STATCHANGE_STACKS);
    }
}
