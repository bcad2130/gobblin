using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Copyright : MoveButton
{
    protected const int COST    = 2;

    private const string SKILLTYPE = "Cover";

    protected const string  STATUSEFFECT_TYPE     = "ARMOR";
    protected const int     STATUSEFFECT_STACKS   = 5;

    protected override void SetUpMove() {
        action.TargetType = "CoverAlly";
        action.SetSkillType(SKILLTYPE);


        Action selfAction = new Action();
        selfAction.AddStatusEffect(STATUSEFFECT_TYPE, STATUSEFFECT_STACKS);

        action.SetSelfAction(selfAction);
        SetIsMove(true);
        SetIsSkill(true);
    }
}
