using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoulBall : MoveButton
{
    protected override void SetUpMove()
    {

        // TODO need to use melee and ranged instead of ServeDrink
        action.TargetType = "Enemy";

        action.SetSkillType("ServeFood");

        AddResourceCost(new RottenAcorn());
        action.SetResourceCost(GetResourceCost());
        SetIsMove(true);
        SetIsSkill(true);
    }
}
