using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBottle : MoveButton
{
    protected const int COST = 4;
    protected const int DAMAGE = 5;

    protected override void SetUpMove()
    {
        action.TargetType = "Enemy";

        action.GutsCost = COST;
        action.Damage = bm.CurrentUnit.GetNetStrength() + DAMAGE;

        action.AddIngredientCost("Bottle");
    }
}
