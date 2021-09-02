using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcornSnack : MoveButton
{
    protected override void SetUpMove()
    {
        action.TargetType = "Targetless";

        action.SetSkillType("Eat");
        
        AddResourceCost(new Acorn());
        action.SetResourceCost(GetResourceCost());
    }
}
