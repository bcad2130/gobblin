using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover : MoveButton
{
    private const string SKILLTYPE = "Cover";

    protected override void SetUpMove() {
        action.TargetType = "CoverAlly";
        action.SetSkillType(SKILLTYPE);
        // TODO handle if you want to cover someone that is already covered
    }
}
