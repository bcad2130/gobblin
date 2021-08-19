using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wait : MoveButton
{
    protected override void SetUpMove() {
        action.TargetType = "Targetless";
    }
}

