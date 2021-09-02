using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickIngredient : MoveButton
{
    protected override void SetUpMove() {
        action.TargetType = "Pick";

        action.SetSkillType("PickIngredient");
    }
}
