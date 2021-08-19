using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover : MoveButton
{
    protected override void SetUpMove() {
        action.TargetType = "CoverAlly";
        // TODO handle if you want to cover someone that is already covered


    }
}
