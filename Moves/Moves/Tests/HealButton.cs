using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealButton : MoveButton
{
    protected const int COST = 3;
    protected const int HEAL = 5;

    protected override void SetUpMove() {
        action.TargetType = "OneAlly";

        action.GutsCost = COST;
        action.Heal = 5;

        SetIsMove(true);

    }
}
