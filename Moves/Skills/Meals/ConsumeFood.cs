using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsumeFood : MoveButton
{
    protected override void SetUpMove() {
        action.TargetType = "Self";
        action.SetSkillType("Eat");

        action.SetResourceCost(GetResourceCost());
        SetIsSkill(true);
    }
}
