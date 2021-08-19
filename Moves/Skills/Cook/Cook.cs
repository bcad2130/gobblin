using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cook : MoveButton
{
    protected override void SetUpMove() {
        action.TargetType = "Cook";

        AddResourceCost(new CauldronItem());
        action.SetResourceCost(GetResourceCost());

        // action.SetResourceCost(new CauldronItem());
    }
}
