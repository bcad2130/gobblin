using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBottle : MoveButton
{
    protected const int COST = 2;
    protected const int DAMAGE = 0;

    protected override void SetUpMove()
    {
        action.TargetType = "Enemy";

        action.GutsCost = COST;
        action.StrengthDamage = bm.CurrentUnit.GetNetStrength() + DAMAGE;

        action.AddIngredientCost("Bottle");
    }
}
