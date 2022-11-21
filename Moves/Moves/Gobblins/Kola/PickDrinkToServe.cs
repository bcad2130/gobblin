using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickDrinkToServe : MoveButton
{
    protected override void SetUpMove() {
        action.TargetType = "Pick";
        action.SetSkillType("PickDrinkToServe");
        // action.TargetType = "PickDrinkToServe";
        SetIsMove(true);
    }
}
