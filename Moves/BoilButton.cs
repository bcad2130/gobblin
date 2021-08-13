using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoilButton : MoveButton
{
    protected const int COST = 0;
    protected const int DAMAGE = 0;

    protected override void SetUpMove() {
        action.TargetType = "Self";

        action.GutsCost = COST;
        action.Damage = DAMAGE;
    }
}
