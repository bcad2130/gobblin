using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stir : MoveButton
{
    protected override void SetUpMove() {
        action.TargetType = "Targetless";
        action.SetSkillType("Stir");
        action.checkIfCookin = true;
        
        SetIsSkill(true);
    }
}
