using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookCornCob : MoveButton
{
    public GameObject Cauldron;

    protected override void SetUpMove() {
        action.TargetType = "Recipe";

        AddResourceCost(new CauldronItem());
        action.ResourceCost = resourceCost;

        action.isSummon = true;
        action.Summon = Cauldron;

        action.isRecipe = true;
        // action.Recipe = recipe;
    }
}
