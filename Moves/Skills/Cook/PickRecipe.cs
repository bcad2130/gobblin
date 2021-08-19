using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickRecipe : MoveButton
{
    public GameObject Cauldron;

    protected override void SetUpMove() {
        action.TargetType = "Recipe";

        AddResourceCost(new CauldronItem());
        action.SetResourceCost(GetResourceCost());

        // action.SetResourceCost(new CauldronItem());

        action.isSummon = true;
        action.Summon = Cauldron;

        action.isRecipe = true;
        action.Recipe = GetRecipe();
    }
}
