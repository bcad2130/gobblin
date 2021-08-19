using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatCornCob : MoveButton
{
    protected override void SetUpMove() {
        AddResourceCost(new CornCobItem());
        action.ResourceCost = resourceCost;
        bm.SetMealPicked(true);

        action.TargetType = "Ally";

        action.Heal = 10;
        action.Sate = 10;
    }
}
