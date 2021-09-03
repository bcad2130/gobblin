using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookPopCorn : MoveButton
{
    public GameObject Cauldron;

    protected override void SetUpMove() {
        Debug.Log("Is this used?");

        action.TargetType = "Recipe";

        AddResourceCost(new Pot());
        action.ResourceCost = resourceCost;

        action.isSummon = true;
        action.Summon = Cauldron;

        action.isRecipe = true;
        // action.Recipe = recipe;
    }
}
