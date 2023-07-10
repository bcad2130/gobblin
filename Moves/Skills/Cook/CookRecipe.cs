using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookRecipe : MoveButton
{
    public GameObject Cauldron;

    protected override void SetUpMove() {
        action.TargetType = "Targetless";

        action.SetSkillType("CookRecipe");

        AddResourceCost(new Pot());
        action.SetResourceCost(GetResourceCost());

        action.isSummon = true;
        action.Summon = Cauldron;

        action.isRecipe = true;
        action.Recipe = GetRecipe();

        SetIsMove(true);
    }
}
