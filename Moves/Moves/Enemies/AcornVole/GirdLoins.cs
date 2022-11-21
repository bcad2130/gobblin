using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirdLoins : MoveButton
{
    protected const int COST    = 4;

    protected override void SetUpMove() {
        action.TargetType = "AllAllies";

        action.GutsCost = COST;

        action.AddStatusEffect("GIRD", 3);
        SetIsMove(true);
        SetIsSkill(true);
    }
}
