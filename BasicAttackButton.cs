using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttackButton : Action
{
    protected const int COST = 0;

    public override void SetUpMove() {
        TargetType = "OneEnemy";

        ManaCost = COST;
        Damage = bm.CurrentUnit.attack;
        // Damage = 5;
    }
}
