using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eat : MoveButton
{
    protected override void SetUpMove() {
        action.TargetType = "SelfOrAlly";
        action.SetSkillType("Eat");

        action.SetResourceCost(GetResourceCost());
        SetIsSkill(true);
    }
}
