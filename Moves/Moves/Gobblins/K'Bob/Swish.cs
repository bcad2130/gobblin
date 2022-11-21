using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swish : MoveButton
{
    protected const int COST = 5;

    protected override void SetUpMove() {
        action.TargetType = "Self";

        action.GutsCost = COST;

        action.AddStatusEffect("DODGE", 1);
        SetIsMove(true);
    }
}
