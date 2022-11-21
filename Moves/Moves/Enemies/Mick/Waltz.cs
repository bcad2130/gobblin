using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MoveButton
{
    protected const int COST    = 2;

    protected override void SetUpMove() {

        action.TargetType = "Self";
        action.GutsCost = COST;

        action.AddStatChange("STRENGTH", 3);
        action.AddStatChange("DEFENSE", -1);
        SetIsMove(true);
        SetIsSkill(true);
    }
}
