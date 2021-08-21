using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickDrinkToServe : MoveButton
{
    protected override void SetUpMove() {
        action.TargetType = "PickDrinkToServe";
    }
}
