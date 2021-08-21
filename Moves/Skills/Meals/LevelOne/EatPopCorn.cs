using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatPopCorn : MoveButton
{
    protected override void SetUpMove() {
        AddResourceCost(new PopCornItem());
        action.ResourceCost = resourceCost;
        bm.SetMealPicked(true);

        action.TargetType = "Ally";

        action.Heal = 10;
        action.Sate = 10;
    }
}
