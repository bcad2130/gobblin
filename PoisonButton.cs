using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonButton : MoveButton
{
    protected const int COST = 5;
    protected const int DAMAGE = 0;

    protected override void SetUpMove() {
        bm.CurrentAction.TargetType = "AllEnemies";
        
        // manaCost = COST;

        bm.CurrentAction.ManaCost = COST;
        bm.CurrentAction.Damage = DAMAGE;
        bm.CurrentAction.AddStatusEffect("POISON", 3);

        // bm.CurrentAction.TargetedAction = true;

    }
}
