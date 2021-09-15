using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collude : MoveButton
{
    protected const int COST = 2;

    protected const string  STATUSEFFECT_TYPE     = "ARMOR";
    protected const int     STATUSEFFECT_STACKS   = 10;

    protected override void SetUpMove() {
        action.TargetType = "CoverAlly";
        action.SetSkillType("Cover");

        action.GutsCost = COST;

        Action selfAction = new Action();
        selfAction.AddStatusEffect(STATUSEFFECT_TYPE, STATUSEFFECT_STACKS);

        action.SetSelfAction(selfAction);
    }
}
