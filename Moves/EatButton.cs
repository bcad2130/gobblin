using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EatButton : MoveButton
{
    protected const int COST = 0;
    protected const int DAMAGE = 0;

    // protected List<Item> resourceCost = new List<Item>();

    protected override void SetUpMove() {
        // resourceCost.Add(new CobItem());
        // action.ResourceCost = resourceCost;

        action.TargetType = "Eat";

        // action.GutsCost = COST;
        // action.Damage = DAMAGE;
        // action.Heal = 10;
        // action.Sate = 10;
    }
}
