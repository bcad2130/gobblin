using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonButton : MoveButton
{
    protected const int COST = 5;
    protected const int DAMAGE = 0;

    protected override void SetUpMove() {
        action.TargetType = "AllEnemies";
        
        // manaCost = COST;

        action.ManaCost = COST;
        action.Damage = DAMAGE;
        action.AddStatusEffect("POISON", 3);

        // action.TargetedAction = true;

    }
}
