using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealButton : MoveButton
{
    protected const int COST = 3;

    protected override void SetUpMove() {
        bm.CurrentAction.TargetType = "OneAlly";
        // manaCost = COST;

        bm.CurrentAction.ManaCost = COST;
        bm.CurrentAction.Damage = 0;
        bm.CurrentAction.Heal = 5;

        // bm.CurrentAction.TargetedAction = true;
    }
}
