using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkewerButton : MoveButton
{
    protected const int COST = 5;
    protected const int DAMAGE = 5;

    protected override void SetUpMove() {

        action.TargetType = "MeleeEnemyPierce";

        action.ManaCost = COST;
        action.Damage = bm.CurrentUnit.attack + DAMAGE;
    }
}
