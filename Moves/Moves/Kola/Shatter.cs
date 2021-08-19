using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shatter : MoveButton
{
    protected const int COST = 6;
    protected const int DAMAGE = 0;

    protected override void SetUpMove() {

        action.TargetType = "AllMeleeEnemies";

        action.GutsCost = COST;
        action.Damage = bm.CurrentUnit.attack + DAMAGE;

        action.AddStatusEffect("BROKENBOTTLE", 1);
    }
}
