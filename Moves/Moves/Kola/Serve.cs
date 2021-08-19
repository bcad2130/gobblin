using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Serve : MoveButton
{
    protected const int COST = 4;
    protected const int DAMAGE = 0;

    protected override void SetUpMove() {

        action.TargetType = "Enemy";

        action.GutsCost = COST;
        action.Damage = bm.CurrentUnit.attack + DAMAGE;

        // action.SetResourceCost(GetResourceCost());
    }
}
