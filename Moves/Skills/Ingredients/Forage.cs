using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forage : MoveButton
{
    protected override void SetUpMove() {
        action.TargetType = "Targetless";
        
        action.SetSkillType("Forage");
        SetIsSkill(true);
    }
}
