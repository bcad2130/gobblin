using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotLid : MoveButton
{
    protected const int COST = 2;
    protected const int DAMAGE = 0;

    protected override void SetUpMove() {

        // TODO implement if you use it on the cauldron

        action.TargetType = "Targetless";

        action.GutsCost = COST;

        // action.AddStatusEffect("DEFEND", 1);
        action.SetSkillType("Lid");
        action.checkIfCookin = true;

        SetIsMove(true);
    }
}
