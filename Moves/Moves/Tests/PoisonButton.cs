using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonButton : MoveButton
{
    protected const int COST = 5;

    protected override void SetUpMove() {
        action.TargetType = "Enemy";
        
        action.GutsCost = COST;
        action.AddStatusEffect("GROSS", 3);

        SetIsMove(true);
    }
}
