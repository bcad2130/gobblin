using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldButton : MoveButton
{
    protected const int COST = 4;

    protected override void SetUpMove() {
        action.TargetType = "Self";

        action.GutsCost = COST;

        action.AddStatusEffect("ARMOR", 5);
    }
}
