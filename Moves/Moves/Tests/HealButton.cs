using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealButton : MoveButton
{
    protected const int COST = 3;

    protected override void SetUpMove() {
        action.TargetType = "OneAlly";

        action.GutsCost = COST;
        action.Damage = 0;
        action.Heal = 5;
    }
}
