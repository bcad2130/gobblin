using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickMealToServe : MoveButton
{
    protected override void SetUpMove() {
        action.TargetType = "PickMealToServe";
    }
}
