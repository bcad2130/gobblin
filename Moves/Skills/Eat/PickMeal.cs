using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickMeal : MoveButton
{
    protected override void SetUpMove() {
        action.TargetType = "PickMeal";
    }
}