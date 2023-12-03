using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pass : MoveButton
{
    protected override void SetUpMove() {
        action.TargetType = "Targetless";

        SetIsPass(true);
        SetIsMove(false);
        SetIsSkill(false);
    }
}

