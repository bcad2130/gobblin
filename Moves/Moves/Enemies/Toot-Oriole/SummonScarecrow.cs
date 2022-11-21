using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonScarecrow : MoveButton
{
    public GameObject Scarecrow;

    protected override void SetUpMove() {
        action.TargetType = "Targetless";

        action.isSummon = true;
        action.Summon = Scarecrow;
        SetIsMove(true);
        SetIsSkill(true);
    }
}
