using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LickWounds : MoveButton
{
    protected const int COST = 3;

    protected override void SetUpMove() {
        action.TargetType = "SelfOrAllyOrMeleeEnemy";

        action.GutsCost = COST;
        action.Heal = 25;
        
        action.AddStatusEffect("GROSS", 5);
    }
}
