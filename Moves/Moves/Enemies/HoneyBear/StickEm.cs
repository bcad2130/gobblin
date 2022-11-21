using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickEm : MoveButton
{
    protected const int COST = 6;

    protected override void SetUpMove() {

        action.TargetType = "MeleeEnemy";

        action.GutsCost = COST;

        action.AddStatChange("SPEED", -3);
        action.AddStatChange("NOS", -3);
        SetIsMove(true);
        SetIsSkill(true);
    }
}
