using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickFoodToServe : MoveButton
{
    protected override void SetUpMove() {
        action.TargetType = "PickFoodToServe";
    }
}
