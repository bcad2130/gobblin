using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defend : MoveButton
{
    protected override void SetUpMove() {
        action.TargetType = "Self";

        action.AddStatusEffect("DEFEND", 1);
    }
}
