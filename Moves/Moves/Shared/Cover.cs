using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover : MoveButton
{
    private const string SKILLTYPE = "Cover";

    protected override void SetUpMove() {
        action.TargetType = "CoverAlly";
        action.SetSkillType(SKILLTYPE);

        SetIsMove(true);
    }
}
