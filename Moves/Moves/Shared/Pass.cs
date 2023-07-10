using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pass : MoveButton
{
    protected override void SetUpMove() {
        action.TargetType = "Targetless";
        SetIsMove(true);
        SetIsSkill(true);   
    }
}

