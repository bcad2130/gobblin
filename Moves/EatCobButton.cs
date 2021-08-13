using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatCobButton : MoveButton
{
    protected const int COST = 0;
    protected const int DAMAGE = 0;

    protected List<Item> resourceCost = new List<Item>();

    protected override void SetUpMove() {
        resourceCost.Add(new CobItem());
        action.ResourceCost = resourceCost;
        bm.SetMealPicked(true);

        action.TargetType = "Ally";

        action.GutsCost = COST;
        action.Damage = DAMAGE;
        action.Heal = 10;
        action.Sate = 10;
    }
}
