using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonButton : MoveButton
{
    protected const int COST = 5;
    protected const int DAMAGE = 0;

    protected override void SetUpMove() {
        action.TargetType = "Enemy";
        
        action.GutsCost = COST;
        action.Damage = DAMAGE;
        action.AddStatusEffect("GROSS", 3);
    }
}
