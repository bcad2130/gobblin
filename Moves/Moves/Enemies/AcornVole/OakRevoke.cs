using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OakRevoke : MoveButton
{
    protected const int COST    = 3;

    private const string SKILLTYPE = "Cover";

    protected override void SetUpMove() {
        action.TargetType = "CoverAlly";
        action.SetSkillType(SKILLTYPE);


        Action selfAction = new Action();
        selfAction.AddStatusEffect("DEFEND", 1);

        action.SetSelfAction(selfAction);
        SetIsMove(true);
        SetIsSkill(true);
    }
}
